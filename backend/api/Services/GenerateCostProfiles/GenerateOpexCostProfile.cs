using api.Adapters;
using api.Dtos;
using api.Models;

namespace api.Services;

public class GenerateOpexCostProfile
{
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ILogger<CaseService> _logger;
    private readonly TopsideService _topsideService;
    private readonly WellProjectService _wellProjectService;

    public GenerateOpexCostProfile(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _logger = loggerFactory.CreateLogger<CaseService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _wellProjectService = serviceProvider.GetRequiredService<WellProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
    }

    public OpexCostProfileDto Generate(Guid caseId)
    {
        var wellInterventionCost = CalculateWellInterventionCostProfile(caseId);

        var offshoreFacilitiesOperationsCost = CalculateOffshoreFacilitiesOperationsCostProfile(caseId);

        var OPEX = TimeSeriesCost.MergeCostProfiles(wellInterventionCost, offshoreFacilitiesOperationsCost);
        if (OPEX == null)
        {
            return new OpexCostProfileDto();
        }

        var opexCostProfile = new OpexCostProfile
        {
            StartYear = OPEX.StartYear,
            Values = OPEX.Values,
        };
        var opexDto = CaseDtoAdapter.Convert(opexCostProfile);
        return opexDto ?? new OpexCostProfileDto();
    }

    public TimeSeries<double> CalculateWellInterventionCostProfile(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId).Result;

        var drainageStrategy = new DrainageStrategy();
        try
        {
            drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink).Result;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink.ToString());
        }

        var lastYear = drainageStrategy?.ProductionProfileOil == null
            ? 0
            : drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length;

        WellProject wellProject;
        try
        {
            wellProject = _wellProjectService.GetWellProject(caseItem.WellProjectLink).Result;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink.ToString());
            return new TimeSeries<double>();
        }

        var linkedWells = wellProject.WellProjectWells?.Where(ew => Well.IsWellProjectWell(ew.Well.WellCategory))
            .ToList();
        if (linkedWells == null)
        {
            return new TimeSeries<double>();
        }

        var wellInterventionCostsFromDrillingSchedule = new TimeSeries<double>();
        foreach (var linkedWell in linkedWells)
        {
            if (linkedWell.DrillingSchedule == null)
            {
                continue;
            }

            var timeSeries = new TimeSeries<double>
            {
                StartYear = linkedWell.DrillingSchedule.StartYear,
                Values = linkedWell.DrillingSchedule.Values.Select(v => (double)v).ToArray(),
            };
            wellInterventionCostsFromDrillingSchedule =
                TimeSeriesCost.MergeCostProfiles(wellInterventionCostsFromDrillingSchedule, timeSeries);
        }

        var tempSeries = new TimeSeries<int>
        {
            StartYear = wellInterventionCostsFromDrillingSchedule.StartYear,
            Values = wellInterventionCostsFromDrillingSchedule.Values.Select(v => (int)v).ToArray(),
        };
        var cumulativeDrillingSchedule = GetCumulativeDrillingSchedule(tempSeries);
        cumulativeDrillingSchedule.StartYear = tempSeries.StartYear;

        var interventionCost = wellProject.AnnualWellInterventionCost;

        var wellInterventionCostValues = cumulativeDrillingSchedule.Values.Select(v => v * interventionCost).ToArray();

        wellInterventionCostsFromDrillingSchedule.Values = wellInterventionCostValues;
        wellInterventionCostsFromDrillingSchedule.StartYear = cumulativeDrillingSchedule.StartYear;

        var totalValuesCount = lastYear == 0
            ? wellInterventionCostsFromDrillingSchedule.Values.Length
            : lastYear - wellInterventionCostsFromDrillingSchedule.StartYear;
        var additionalValuesCount = totalValuesCount - wellInterventionCostsFromDrillingSchedule.Values.Length;

        var additionalValues = new List<double>();
        for (var i = 0; i < additionalValuesCount; i++)
        {
            if (wellInterventionCostsFromDrillingSchedule.Values.Length > 0)
            {
                additionalValues.Add(wellInterventionCostsFromDrillingSchedule.Values.Last());
            }
        }

        var valuesList = wellInterventionCostsFromDrillingSchedule.Values.ToList();
        valuesList.AddRange(additionalValues);

        wellInterventionCostsFromDrillingSchedule.Values = valuesList.ToArray();

        return wellInterventionCostsFromDrillingSchedule;
    }

    public TimeSeries<double> CalculateOffshoreFacilitiesOperationsCostProfile(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId).Result;

        DrainageStrategy drainageStrategy;
        try
        {
            drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink).Result;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found", caseItem.DrainageStrategyLink.ToString());
            return new TimeSeries<double>();
        }

        if (drainageStrategy?.ProductionProfileOil == null)
        {
            return new TimeSeries<double>();
        }

        var firstYear = drainageStrategy.ProductionProfileOil.StartYear;
        var lastYear = drainageStrategy.ProductionProfileOil.StartYear +
                       drainageStrategy.ProductionProfileOil.Values.Length;

        Topside topside;
        try
        {
            topside = _topsideService.GetTopside(caseItem.TopsideLink).Result;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Topside {0} not found", caseItem.TopsideLink.ToString());
            return new TimeSeries<double>();
        }

        var facilityOpex = topside.FacilityOpex;
        var values = new List<double>
        {
            (facilityOpex - 1) / 8,
            (facilityOpex - 1) / 4,
            (facilityOpex - 1) / 2,
        };

        for (var i = firstYear; i < lastYear; i++)
        {
            values.Add(facilityOpex);
        }

        const int preOpexCostYearOffset = 3;

        var offshoreFacilitiesOperationsCost = new TimeSeries<double>
        {
            StartYear = firstYear - preOpexCostYearOffset,
            Values = values.ToArray(),
        };
        return offshoreFacilitiesOperationsCost;
    }

    private static TimeSeries<double> GetCumulativeDrillingSchedule(TimeSeries<int> drillingSchedule)
    {
        var cumulativeSchedule = new TimeSeries<double>
        {
            StartYear = drillingSchedule.StartYear,
        };
        var values = new List<double>();
        var sum = 0.0;
        for (var i = 0; i < drillingSchedule.Values.Length; i++)
        {
            sum += drillingSchedule.Values[i];
            values.Add(sum);
        }

        cumulativeSchedule.Values = values.ToArray();

        return cumulativeSchedule;
    }
}
