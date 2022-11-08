using System.Globalization;

using api.Adapters;
using api.Dtos;
using api.Models;

namespace api.Services;

public class GenerateGAndGAdminCostProfile
{
    private readonly CaseService _caseService;
    private readonly ExplorationService _explorationService;
    private readonly ILogger<CaseService> _logger;
    private readonly ProjectService _projectService;

    public GenerateGAndGAdminCostProfile(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _logger = loggerFactory.CreateLogger<CaseService>();
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _explorationService = serviceProvider.GetRequiredService<ExplorationService>();
    }

    public async Task<GAndGAdminCostDto> Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);

        Exploration exploration;
        try
        {
            exploration = await _explorationService.GetExploration(caseItem.ExplorationLink);
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Exploration {0} not found", caseItem.ExplorationLink.ToString());
            return new GAndGAdminCostDto();
        }

        var linkedWells = exploration.ExplorationWells
            ?.Where(ew => ew.Well.WellCategory == WellCategory.Exploration_Well).ToList();
        if (exploration != null && linkedWells?.Count > 0)
        {
            var drillingSchedules = linkedWells.Select(lw => lw.DrillingSchedule);
            var earliestYear = drillingSchedules.Select(ds => ds?.StartYear)?.Min() + caseItem.DG4Date.Year;
            var dG1Date = caseItem.DG1Date;
            if (earliestYear != null && dG1Date.Year >= earliestYear)
            {
                var project = _projectService.GetProject(caseItem.ProjectId);
                var country = project.Country;
                var countryCost = MapCountry(country);
                var lastYear = new DateTimeOffset(dG1Date.Year, 1, 1, 0, 0, 0, 0, new GregorianCalendar(),
                    TimeSpan.Zero);
                var lastYearMinutes = (dG1Date - lastYear).TotalMinutes;

                var totalMinutesLastYear = new TimeSpan(365, 0, 0, 0).TotalMinutes;
                var percentageOfLastYear = lastYearMinutes / totalMinutesLastYear;

                var gAndGAdminCost = new GAndGAdminCost
                {
                    StartYear = (int)earliestYear - caseItem.DG4Date.Year,
                };
                var years = lastYear.Year - (int)earliestYear;
                var values = new List<double>();
                for (var i = 0; i < years; i++)
                {
                    values.Add(countryCost);
                }

                values.Add(countryCost * percentageOfLastYear);
                gAndGAdminCost.Values = values.ToArray();
                return ExplorationDtoAdapter.Convert(gAndGAdminCost);
            }
        }

        return new GAndGAdminCostDto();
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
