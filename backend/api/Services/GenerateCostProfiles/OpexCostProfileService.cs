using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;

using AutoMapper;

namespace api.Services;

public class OpexCostProfileService : IOpexCostProfileService
{
    private readonly ICaseService _caseService;
    private readonly IProjectService _projectService;
    private readonly ILogger<OpexCostProfileService> _logger;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IWellProjectService _wellProjectService;
    private readonly IWellProjectWellService _wellProjectWellService;
    private readonly ITopsideService _topsideService;
    private readonly DcdDbContext _context;
    private readonly IMapper _mapper;

    public OpexCostProfileService(
        DcdDbContext context,
        ILoggerFactory loggerFactory,
        ICaseService caseService,
        IProjectService projectService,
        IDrainageStrategyService drainageStrategyService,
        IWellProjectService wellProjectService,
        IWellProjectWellService wellProjectWellService,
        ITopsideService topsideService,
        IMapper mapper)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger<OpexCostProfileService>();
        _projectService = projectService;
        _drainageStrategyService = drainageStrategyService;
        _caseService = caseService;
        _wellProjectService = wellProjectService;
        _wellProjectWellService = wellProjectWellService;
        _topsideService = topsideService;
        _mapper = mapper;
    }

    public async Task<OpexCostProfileWrapperDto> Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);
        var project = await _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);

        var result = new OpexCostProfileWrapperDto();

        if (drainageStrategy == null)
        {
            throw new NotFoundInDBException(string.Format("DrainageStrategy {0} not found in database.", caseItem.DrainageStrategyLink));
        }

        var newWellInterventionCost = await CalculateWellInterventionCostProfile(caseItem, project, drainageStrategy);
        var newOffshoreFacilitiesOperationsCost = await CalculateOffshoreFacilitiesOperationsCostProfile(caseItem, drainageStrategy);

        WellInterventionCostProfile wellInterventionCost;
        if (caseItem.WellInterventionCostProfileOverride != null)
        {
            wellInterventionCost = new WellInterventionCostProfile
            {
                StartYear = caseItem.WellInterventionCostProfileOverride.StartYear,
                Values = caseItem.WellInterventionCostProfileOverride.Values,
            };
        }
        else
        {
            wellInterventionCost = newWellInterventionCost;
        }

        OffshoreFacilitiesOperationsCostProfile offshoreFacilitiesOperationsCost;
        if (caseItem.OffshoreFacilitiesOperationsCostProfileOverride != null)
        {
            offshoreFacilitiesOperationsCost = new OffshoreFacilitiesOperationsCostProfile
            {
                StartYear = caseItem.OffshoreFacilitiesOperationsCostProfileOverride.StartYear,
                Values = caseItem.OffshoreFacilitiesOperationsCostProfileOverride.Values,
            };
        }
        else
        {
            offshoreFacilitiesOperationsCost = newOffshoreFacilitiesOperationsCost;
        }

        var historicCost = caseItem.HistoricCostCostProfile ?? new HistoricCostCostProfile();
        var onshoreRelatedOPEXCost = caseItem.OnshoreRelatedOPEXCostProfile ?? new OnshoreRelatedOPEXCostProfile();
        var additionalOPEXCost = caseItem.AdditionalOPEXCostProfile ?? new AdditionalOPEXCostProfile();

        await UpdateCaseAndSave(caseItem, wellInterventionCost, offshoreFacilitiesOperationsCost, historicCost, onshoreRelatedOPEXCost, additionalOPEXCost);

        var wellInterventionCostDto = _mapper.Map<WellInterventionCostProfileDto>(wellInterventionCost);
        var offshoreFacilitiesOperationsCostDto = _mapper.Map<OffshoreFacilitiesOperationsCostProfileDto>(offshoreFacilitiesOperationsCost);
        var historicCostCostDto = _mapper.Map<HistoricCostCostProfileDto>(historicCost);
        var onshoreRelatedOPEXCostDto = _mapper.Map<OnshoreRelatedOPEXCostProfileDto>(onshoreRelatedOPEXCost);
        var additionalOPEXCostDto = _mapper.Map<AdditionalOPEXCostProfileDto>(additionalOPEXCost);

        result.WellInterventionCostProfileDto = wellInterventionCostDto;
        result.OffshoreFacilitiesOperationsCostProfileDto = offshoreFacilitiesOperationsCostDto;
        result.HistoricCostCostProfileDto = historicCostCostDto;
        result.OnshoreRelatedOPEXCostProfileDto = onshoreRelatedOPEXCostDto;
        result.AdditionalOPEXCostProfileDto = additionalOPEXCostDto;

        var OPEX = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>> {
        wellInterventionCost,
        offshoreFacilitiesOperationsCost,
        historicCost,
        onshoreRelatedOPEXCost,
        additionalOPEXCost
    });

        var opexCostProfile = new OpexCostProfile
        {
            StartYear = OPEX.StartYear,
            Values = OPEX.Values
        };

        var opexDto = _mapper.Map<OpexCostProfileDto>(opexCostProfile);

        result.OpexCostProfileDto = opexDto;
        return result;
    }


    private async Task<int> UpdateCaseAndSave(
        Case caseItem,
        WellInterventionCostProfile wellInterventionCostProfile,
        OffshoreFacilitiesOperationsCostProfile offshoreFacilitiesOperationsCostProfile,
        HistoricCostCostProfile historicCostCostProfile,
        OnshoreRelatedOPEXCostProfile onshoreRelatedOPEXCostProfile,
        AdditionalOPEXCostProfile additionalOPEXCostProfile)
    {
        caseItem.WellInterventionCostProfile = wellInterventionCostProfile;
        caseItem.OffshoreFacilitiesOperationsCostProfile = offshoreFacilitiesOperationsCostProfile;
        caseItem.HistoricCostCostProfile = historicCostCostProfile;
        caseItem.OnshoreRelatedOPEXCostProfile = onshoreRelatedOPEXCostProfile;
        caseItem.AdditionalOPEXCostProfile = additionalOPEXCostProfile;
        return await _context.SaveChangesAsync();
    }

    public async Task<WellInterventionCostProfile> CalculateWellInterventionCostProfile(Case caseItem, Project project, DrainageStrategy drainageStrategy)
    {
        var lastYear = drainageStrategy?.ProductionProfileOil == null ? 0 : drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length;

        WellProject wellProject;
        try
        {
            wellProject = await _wellProjectService.GetWellProject(caseItem.WellProjectLink);
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
            return new WellInterventionCostProfile();
        }
        var linkedWells = await _wellProjectWellService.GetWellProjectWellsForWellProject(wellProject.Id);
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

    public async Task<OffshoreFacilitiesOperationsCostProfile> CalculateOffshoreFacilitiesOperationsCostProfile(Case caseItem, DrainageStrategy drainageStrategy)
    {
        if (drainageStrategy.ProductionProfileOil == null || drainageStrategy.ProductionProfileOil.Values.Length == 0)
        {
            return new OffshoreFacilitiesOperationsCostProfile { Values = [] };
        }
        var firstYear = drainageStrategy.ProductionProfileOil.StartYear;
        var lastYear = drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length;

        Topside topside;
        try
        {
            topside = await _topsideService.GetTopside(caseItem.TopsideLink);
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Topside {0} not found.", caseItem.TopsideLink);
            return new OffshoreFacilitiesOperationsCostProfile { Values = [] };
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
