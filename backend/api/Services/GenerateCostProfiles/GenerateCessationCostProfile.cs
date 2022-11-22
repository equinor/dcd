using api.Adapters;
using api.Dtos;
using api.Models;

namespace api.Services;

public class GenerateCessationCostProfile
{
    private readonly CaseService _caseService;
    private readonly ILogger<CaseService> _logger;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly WellProjectService _wellProjectService;
    private readonly SurfService _surfService;
    private readonly ProjectService _projectService;

    public GenerateCessationCostProfile(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _logger = loggerFactory.CreateLogger<CaseService>();
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
        _wellProjectService = serviceProvider.GetRequiredService<WellProjectService>();
        _surfService = serviceProvider.GetRequiredService<SurfService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
    }

    public CessationCostDto Generate(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var project = _projectService.GetProjectWithoutAssets(caseItem.ProjectId);

        var cessationWells = new TimeSeries<double>();
        var cessationOffshoreFacilities = new TimeSeries<double>();

        var lastYear = GetRelativeLastYearOfProduction(caseItem);
        if (lastYear == null) { return new CessationCostDto(); }

        WellProject wellProject;
        try
        {
            wellProject = _wellProjectService.GetWellProject(caseItem.WellProjectLink);
            var linkedWells = wellProject.WellProjectWells?.Where(ew => Well.IsWellProjectWell(ew.Well.WellCategory)).ToList();
            if (linkedWells != null)
            {
                var pluggingAndAbandonment = project.DevelopmentOperationalWellCosts?.PluggingAndAbandonment ?? 0;

                var sumDrilledWells = 0;
                foreach (var well in linkedWells)
                {
                    sumDrilledWells += well.DrillingSchedule?.Values.Sum() ?? 0;
                }
                var totalCost = sumDrilledWells * (double)pluggingAndAbandonment;
                cessationWells.StartYear = (int)lastYear;
                var cessationWellsValues = new double[] { totalCost / 2, totalCost / 2 };
                cessationWells.Values = cessationWellsValues;
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
        }

        Surf surf;
        try
        {
            surf = _surfService.GetSurf(caseItem.SurfLink);
            var surfCessationCost = surf.CessationCost;

            cessationOffshoreFacilities.StartYear = (int)lastYear + 1;
            var cessationOffshoreFacilitiesValues = new double[] { (double)surfCessationCost / 2, (double)surfCessationCost / 2 };
            cessationOffshoreFacilities.Values = cessationOffshoreFacilitiesValues;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Surf {0} not found.", caseItem.SurfLink);
        }

        var cessationTimeSeries = TimeSeriesCost.MergeCostProfiles(cessationWells, cessationOffshoreFacilities);
        var cessation = new CessationCost
        {
            StartYear = cessationTimeSeries.StartYear,
            Values = cessationTimeSeries.Values
        };
        var dto = CaseDtoAdapter.Convert(cessation);
        return dto;
    }

    private int? GetRelativeLastYearOfProduction(Case caseItem)
    {
        var drainageStrategy = new DrainageStrategy();
        try
        {
            drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
            return null;
        }
        if (drainageStrategy.ProductionProfileOil == null) { return null; }
        var lastYear = drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length - 1;
        return lastYear;
    }
}
