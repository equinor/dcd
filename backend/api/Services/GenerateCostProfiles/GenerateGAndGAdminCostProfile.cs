using System.Globalization;

using api.Models;


namespace api.Services;

public class GenerateGAndGAdminCostProfile : IGenerateGAndGAdminCostProfile
{
    private readonly IProjectService _projectService;
    private readonly ICaseService _caseService;
    private readonly ILogger<GenerateGAndGAdminCostProfile> _logger;
    private readonly IExplorationService _explorationService;
    private readonly IExplorationWellService _explorationWellService;

    public GenerateGAndGAdminCostProfile(
        ILoggerFactory loggerFactory,
        IProjectService projectService,
        ICaseService caseService,
        IExplorationService explorationService,
        IExplorationWellService explorationWellService
    )
    {
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<GenerateGAndGAdminCostProfile>();
        _caseService = caseService;
        _explorationService = explorationService;
        _explorationWellService = explorationWellService;
    }

    public async Task Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCaseWithIncludes(caseId);

        var exploration = await _explorationService.GetExplorationWithIncludes(
            caseItem.ExplorationLink,
            e => e.GAndGAdminCost!,
            e => e.GAndGAdminCostOverride!
        );

        if (exploration.GAndGAdminCostOverride?.Override == true)
        {
            return;
        }

        var linkedWells = await _explorationWellService.GetExplorationWellsForExploration(exploration.Id);
        if (linkedWells?.Count > 0)
        {
            var drillingSchedules = linkedWells.Select(lw => lw.DrillingSchedule);
            var earliestYear = drillingSchedules.Select(ds => ds?.StartYear)?.Min() + caseItem.DG4Date.Year;
            var dG1Date = caseItem.DG1Date;
            if (earliestYear != null && dG1Date.Year >= earliestYear)
            {
                var project = await _projectService.GetProject(caseItem.ProjectId);
                var countryCost = MapCountry(project.Country);
                var lastYear = new DateTimeOffset(dG1Date.Year, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
                var lastYearMinutes = (dG1Date - lastYear).TotalMinutes;

                var totalMinutesLastYear = new TimeSpan(365, 0, 0, 0).TotalMinutes;
                var percentageOfLastYear = lastYearMinutes / totalMinutesLastYear;

                var gAndGAdminCost = new GAndGAdminCost();

                gAndGAdminCost.StartYear = (int)earliestYear - caseItem.DG4Date.Year;

                var years = lastYear.Year - (int)earliestYear;
                var values = new List<double>();
                for (int i = 0; i < years; i++)
                {
                    values.Add(countryCost);
                }
                values.Add(countryCost * percentageOfLastYear);
                gAndGAdminCost.Values = values.ToArray();

                if (exploration.GAndGAdminCost != null)
                {
                    exploration.GAndGAdminCost.Values = gAndGAdminCost.Values;
                    exploration.GAndGAdminCost.StartYear = gAndGAdminCost.StartYear;
                }
                else
                {
                    exploration.GAndGAdminCost = gAndGAdminCost;
                }
            }
        }
    }

    private static double MapCountry(string country)
    {
        return country switch
        {
            "NORWAY" => 1,
            "UK" => 1,
            "BRAZIL" => 3,
            "CANADA" => 3,
            "UNITED STATES" => 3,
            _ => 7.0,
        };
    }
}
