using api.Adapters;
using api.Dtos;
using api.Models;

namespace api.Services;

public class GenerateCessationCostProfile
{
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ILogger<CaseService> _logger;
    private readonly SurfService _surfService;
    private readonly WellProjectService _wellProjectService;

    public GenerateCessationCostProfile(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _logger = loggerFactory.CreateLogger<CaseService>();
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
        _wellProjectService = serviceProvider.GetRequiredService<WellProjectService>();
        _surfService = serviceProvider.GetRequiredService<SurfService>();
    }

    public async Task<CessationCostDto> Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);

        var cessationWells = new TimeSeries<double>();
        var cessationOffshoreFacilities = new TimeSeries<double>();

        var lastYear = GetRelativeLastYearOfProduction(caseItem);
        if (lastYear == null)
        {
            return new CessationCostDto();
        }

        WellProject wellProject;
        try
        {
            wellProject = await _wellProjectService.GetWellProject(caseItem.WellProjectLink);
            var linkedWells = wellProject.WellProjectWells?.Where(ew => Well.IsWellProjectWell(ew.Well.WellCategory))
                .ToList();
            if (linkedWells != null)
            {
                var pluggingAndAbandonment = wellProject.PluggingAndAbandonment;

                var sumDrilledWells = 0;
                foreach (var well in linkedWells)
                {
                    sumDrilledWells += well.DrillingSchedule?.Values.Sum() ?? 0;
                }

                var totalCost = sumDrilledWells * pluggingAndAbandonment;
                cessationWells.StartYear = (int)lastYear;
                var cessationWellsValues = new[] { totalCost / 2, totalCost / 2 };
                cessationWells.Values = cessationWellsValues;
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("WellProject {0} not found", caseItem.WellProjectLink.ToString());
        }

        Surf surf;
        try
        {
            surf = await _surfService.GetSurf(caseItem.SurfLink);
            var surfCessationCost = surf.CessationCost;

            cessationOffshoreFacilities.StartYear = (int)lastYear + 1;
            var cessationOffshoreFacilitiesValues = new[] { surfCessationCost / 2, surfCessationCost / 2 };
            cessationOffshoreFacilities.Values = cessationOffshoreFacilitiesValues;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Surf {0} not found", caseItem.SurfLink.ToString());
        }

        var cessationTimeSeries = TimeSeriesCost.MergeCostProfiles(cessationWells, cessationOffshoreFacilities);
        var cessation = new CessationCost
        {
            StartYear = cessationTimeSeries.StartYear,
            Values = cessationTimeSeries.Values,
        };
        var dto = CaseDtoAdapter.Convert(cessation);
        return dto;
    }

    private int? GetRelativeLastYearOfProduction(Case caseItem)
    {
        var drainageStrategy = new DrainageStrategy();
        try
        {
            drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink).Result;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found", caseItem.DrainageStrategyLink.ToString());
            return null;
        }

        if (drainageStrategy.ProductionProfileOil == null)
        {
            return null;
        }

        var lastYear = drainageStrategy.ProductionProfileOil.StartYear +
            drainageStrategy.ProductionProfileOil.Values.Length - 1;
        return lastYear;
    }
}
