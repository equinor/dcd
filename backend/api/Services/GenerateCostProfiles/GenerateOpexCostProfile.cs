using api.Adapters;
using api.Dtos;
using api.Models;

namespace api.Services;

public class GenerateOpexCostProfile
{
    private readonly CaseService _caseService;
    private readonly ProjectService _projectService;
    private readonly ILogger<CaseService> _logger;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly WellProjectService _wellProjectService;
    private readonly TopsideService _topsideService;

    public GenerateOpexCostProfile(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _logger = loggerFactory.CreateLogger<CaseService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _wellProjectService = serviceProvider.GetRequiredService<WellProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
    }

    public OpexCostProfileWrapperDto Generate(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var project = _projectService.GetProjectWithoutAssets(caseItem.ProjectId);

        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);

        var result = new OpexCostProfileWrapperDto();
        var wellInterventionCost = CalculateWellInterventionCostProfile(caseItem, project, drainageStrategy);
        var wellInterventionCostDto = CaseDtoAdapter.Convert(wellInterventionCost);

        var offshoreFacilitiesOperationsCost = CalculateOffshoreFacilitiesOperationsCostProfile(caseItem, drainageStrategy);
        var offshoreFacilitiesOperationsCostDto = CaseDtoAdapter.Convert(offshoreFacilitiesOperationsCost);

        result.WellInterventionCostProfileDto = wellInterventionCostDto;
        result.OffshoreFacilitiesOperationsCostProfileDto = offshoreFacilitiesOperationsCostDto;

        if (drainageStrategy != null)
        {
            wellInterventionCost = CalculateWellInterventionCostProfile(caseItem, project, drainageStrategy);
            offshoreFacilitiesOperationsCost = CalculateOffshoreFacilitiesOperationsCostProfile(caseItem, drainageStrategy);
        }
        else
        {
            wellInterventionCost = CalculateWellInterventionCostProfile(caseItem, project, new DrainageStrategy());
            offshoreFacilitiesOperationsCost = new OffshoreFacilitiesOperationsCostProfile();
        }

        var OPEX = TimeSeriesCost.MergeCostProfiles(wellInterventionCost, offshoreFacilitiesOperationsCost);
        var opexCostProfile = new OpexCostProfile
        {
            StartYear = OPEX.StartYear,
            Values = OPEX.Values
        };
        var opexDto = CaseDtoAdapter.Convert(opexCostProfile);
        result.OpexCostProfileDto = opexDto;
        return result;
    }

    public WellInterventionCostProfile CalculateWellInterventionCostProfile(Case caseItem, Project project, DrainageStrategy drainageStrategy)
    {
        var lastYear = drainageStrategy?.ProductionProfileOil == null ? 0 : drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length;

        WellProject wellProject;
        try
        {
            wellProject = _wellProjectService.GetWellProject(caseItem.WellProjectLink);
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
            return new WellInterventionCostProfile();
        }
        var linkedWells = wellProject.WellProjectWells?.Where(ew => Well.IsWellProjectWell(ew.Well.WellCategory)).ToList();
        if (linkedWells == null) { return new WellInterventionCostProfile(); }

        var wellInterventionCostsFromDrillingSchedule = new TimeSeries<double>();
        foreach (var linkedWell in linkedWells)
        {
            if (linkedWell.DrillingSchedule == null) { continue; }

            var timeSeries = new TimeSeries<double>
            {
                StartYear = linkedWell.DrillingSchedule.StartYear,
                Values = linkedWell.DrillingSchedule.Values.Select(v => (double)v).ToArray()
            };
            wellInterventionCostsFromDrillingSchedule = TimeSeriesCost.MergeCostProfiles(wellInterventionCostsFromDrillingSchedule, timeSeries);
        }

        var tempSeries = new TimeSeries<int>
        {
            StartYear = wellInterventionCostsFromDrillingSchedule.StartYear,
            Values = wellInterventionCostsFromDrillingSchedule.Values.Select(v => (int)v).ToArray()
        };
        var cumulativeDrillingSchedule = GetCumulativeDrillingSchedule(tempSeries);
        cumulativeDrillingSchedule.StartYear = tempSeries.StartYear;

        var interventionCost = project.DevelopmentOperationalWellCosts?.AnnualWellInterventionCostPerWell ?? 0;

        var wellInterventionCostValues = cumulativeDrillingSchedule.Values.Select(v => v * interventionCost).ToArray();

        wellInterventionCostsFromDrillingSchedule.Values = wellInterventionCostValues;
        wellInterventionCostsFromDrillingSchedule.StartYear = cumulativeDrillingSchedule.StartYear;

        var totalValuesCount = lastYear == 0 ? wellInterventionCostsFromDrillingSchedule.Values.Length : lastYear - wellInterventionCostsFromDrillingSchedule.StartYear;
        var additionalValuesCount = totalValuesCount - wellInterventionCostsFromDrillingSchedule.Values.Length;

        var additionalValues = new List<double>();
        for (int i = 0; i < additionalValuesCount; i++)
        {
            if (wellInterventionCostsFromDrillingSchedule.Values.Length > 0)
            {
                additionalValues.Add(wellInterventionCostsFromDrillingSchedule.Values.Last());
            }
        }

        var valuesList = wellInterventionCostsFromDrillingSchedule.Values.ToList();
        valuesList.AddRange(additionalValues);

        wellInterventionCostsFromDrillingSchedule.Values = valuesList.ToArray();

        var result = new WellInterventionCostProfile
        {
            Values = wellInterventionCostsFromDrillingSchedule.Values,
            StartYear = wellInterventionCostsFromDrillingSchedule.StartYear,
        };

        return result;
    }

    public OffshoreFacilitiesOperationsCostProfile CalculateOffshoreFacilitiesOperationsCostProfile(Case caseItem, DrainageStrategy drainageStrategy)
    {
        if (drainageStrategy.ProductionProfileOil == null) { return new OffshoreFacilitiesOperationsCostProfile(); }
        var firstYear = drainageStrategy.ProductionProfileOil.StartYear;
        var lastYear = drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length;

        Topside topside;
        try
        {
            topside = _topsideService.GetTopside(caseItem.TopsideLink);
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Topside {0} not found.", caseItem.TopsideLink);
            return new OffshoreFacilitiesOperationsCostProfile();
        }
        var facilityOpex = topside.FacilityOpex;
        var values = new List<double>
        {
            (facilityOpex - 1) / 8,
            (facilityOpex - 1) / 4,
            (facilityOpex - 1) / 2
        };

        for (int i = firstYear; i < lastYear; i++)
        {
            values.Add(facilityOpex);
        }
        const int preOpexCostYearOffset = 3;

        var offshoreFacilitiesOperationsCost = new OffshoreFacilitiesOperationsCostProfile
        {
            StartYear = firstYear - preOpexCostYearOffset,
            Values = values.ToArray()
        };
        return offshoreFacilitiesOperationsCost;
    }

    private static TimeSeries<double> GetCumulativeDrillingSchedule(TimeSeries<int> drillingSchedule)
    {
        var cumulativeSchedule = new TimeSeries<double>
        {
            StartYear = drillingSchedule.StartYear
        };
        var values = new List<double>();
        var sum = 0.0;
        for (int i = 0; i < drillingSchedule.Values.Length; i++)
        {
            sum += drillingSchedule.Values[i];
            values.Add(sum);
        }

        cumulativeSchedule.Values = values.ToArray();

        return cumulativeSchedule;
    }
}
