using api.Dtos;
using api.Models;
using api.Services.GenerateCostProfiles;


namespace api.Services;

public class CompareCasesService : ICompareCasesService
{
    private readonly IProjectService _projectService;
    private readonly ILogger<CompareCasesService> _logger;
    private readonly IExplorationService _explorationService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IStudyCostProfileService _generateStudyCostProfile;
    private readonly ICaseService _caseService;


    public CompareCasesService(
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IExplorationService explorationService,
        IDrainageStrategyService drainageStrategyService,
        IStudyCostProfileService generateStudyCostProfile,
        ICaseService caseService)
    {
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<CompareCasesService>();
        _explorationService = explorationService;
        _drainageStrategyService = drainageStrategyService;
        _generateStudyCostProfile = generateStudyCostProfile;
        _caseService = caseService;
    }

    public async Task<IEnumerable<CompareCasesDto>> Calculate(Guid projectId)
    {
        var project = await _projectService.GetProjectWithoutAssetsNoTracking(projectId);
        var caseList = new List<CompareCasesDto>();
        if (project.Cases != null)
        {
            DrainageStrategy drainageStrategy;
            Exploration exploration;
            foreach (var caseItem in project.Cases)
            {
                if (caseItem.Archived) { continue; }
                var caseWithProfiles = await _caseService.GetCaseWithIncludes(
                    caseItem.Id,
                    c => c.CessationWellsCostOverride!,
                    c => c.CessationWellsCost!,
                    c => c.CessationOffshoreFacilitiesCostOverride!,
                    c => c.CessationOffshoreFacilitiesCost!,
                    c => c.CessationOnshoreFacilitiesCostProfile!,
                    c => c.TotalFeasibilityAndConceptStudies!,
                    c => c.TotalFeasibilityAndConceptStudiesOverride!,
                    c => c.TotalFEEDStudies!,
                    c => c.TotalFEEDStudiesOverride!,
                    c => c.TotalOtherStudiesCostProfile!,
                    c => c.WellInterventionCostProfile!,
                    c => c.WellInterventionCostProfileOverride!,
                    c => c.OffshoreFacilitiesOperationsCostProfile!,
                    c => c.OffshoreFacilitiesOperationsCostProfileOverride!,
                    c => c.HistoricCostCostProfile!,
                    c => c.OnshoreRelatedOPEXCostProfile!,
                    c => c.AdditionalOPEXCostProfile!);

                drainageStrategy = await _drainageStrategyService.GetDrainageStrategyWithIncludes(
                    caseItem.DrainageStrategyLink,
                    d => d.ProductionProfileOil!,
                    d => d.AdditionalProductionProfileOil!,
                    d => d.ProductionProfileGas!,
                    d => d.AdditionalProductionProfileGas!,
                    d => d.Co2EmissionsOverride!,
                    d => d.Co2Emissions!);

                exploration = await _explorationService.GetExplorationWithIncludes(
                    caseItem.ExplorationLink,
                    e => e.GAndGAdminCost!,
                    e => e.CountryOfficeCost!,
                    e => e.SeismicAcquisitionAndProcessing!,
                    e => e.ExplorationWellCostProfile!,
                    e => e.AppraisalWellCostProfile!,
                    e => e.SidetrackCostProfile!);

                var totalOilProduction = drainageStrategy.ProductionProfileOil?.Values.Sum()/1000000 ?? 0;
                var additionalOilProduction = drainageStrategy.AdditionalProductionProfileOil?.Values.Sum()/1000000 ?? 0;
                var totalGasProduction = drainageStrategy.ProductionProfileGas?.Values.Sum()/1000000000 ?? 0;
                var additionalGasProduction = drainageStrategy.AdditionalProductionProfileGas?.Values.Sum()/1000000000 ?? 0;
                var totalExportedVolumes = CalculateTotalExportedVolumes(caseItem, project, drainageStrategy, false);

                var explorationCosts = CalculateExplorationWellCosts(caseItem, exploration);
                var developmentCosts = await CalculateDevelopmentWellCosts(caseItem);

                TimeSeriesMass? generateCo2EmissionsProfile = drainageStrategy.Co2EmissionsOverride?.Override == true ? drainageStrategy.Co2EmissionsOverride : drainageStrategy.Co2Emissions;

                var totalCo2Emissions = generateCo2EmissionsProfile?.Values.Sum() ?? 0;
                var co2Intensity = CalculateCO2Intensity(caseItem, project, drainageStrategy, totalCo2Emissions);

                var totalCessationCosts = CalculateTotalCessationCosts(caseWithProfiles);

                var totalStudyCostsPlusOpex = CalculateTotalStudyCostsPlusOpex(caseWithProfiles);
                var offshorePlusOnshoreFacilityCosts = await CalculateOffshorePlusOnshoreFacilityCosts(caseItem);

                var compareCases = new CompareCasesDto
                {
                    CaseId = caseItem.Id,
                    TotalOilProduction = totalOilProduction,
                    AdditionalOilProduction = additionalOilProduction,
                    TotalGasProduction = totalGasProduction,
                    AdditionalGasProduction = additionalGasProduction,
                    TotalExportedVolumes = totalExportedVolumes,
                    TotalStudyCostsPlusOpex = totalStudyCostsPlusOpex,
                    TotalCessationCosts = totalCessationCosts,
                    OffshorePlusOnshoreFacilityCosts = offshorePlusOnshoreFacilityCosts,
                    DevelopmentWellCosts = developmentCosts,
                    ExplorationWellCosts = explorationCosts,
                    TotalCo2Emissions = totalCo2Emissions,
                    Co2Intensity = co2Intensity,
                };
                caseList.Add(compareCases);
            }
        }
        return caseList;
    }

    private double CalculateTotalOilProduction(Case caseItem, Project project, DrainageStrategy drainageStrategy, Boolean excludeOilFieldConversion)
    {
        var sumOilProduction = 0.0;
        var million = 1E6;
        var bblConversionFactor = 6.29;
        try
        {
            if (drainageStrategy.ProductionProfileOil != null)
            {
                sumOilProduction += drainageStrategy.ProductionProfileOil.Values.Sum();
            }

            if (drainageStrategy.AdditionalProductionProfileOil != null)
            {
                sumOilProduction += drainageStrategy.AdditionalProductionProfileOil.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
        }

        if (project.PhysicalUnit != 0 && !excludeOilFieldConversion)
        {
            return sumOilProduction * bblConversionFactor / million;
        }

        return sumOilProduction / million;
    }

    private double CalculateTotalGasProduction(Case caseItem, Project project, DrainageStrategy drainageStrategy, Boolean excludeOilFieldConversion)
    {
        var sumGasProduction = 0.0;
        var billion = 1E9;
        var scfConversionFactor = 35.315;
        try
        {
            if (drainageStrategy.ProductionProfileGas != null)
            {
                sumGasProduction += drainageStrategy.ProductionProfileGas.Values.Sum();
            }

            if (drainageStrategy.AdditionalProductionProfileGas != null)
            {
                sumGasProduction += drainageStrategy.AdditionalProductionProfileGas.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
        }

        if (project.PhysicalUnit != 0 && !excludeOilFieldConversion)
        {
            return sumGasProduction * scfConversionFactor / billion;
        }

        return sumGasProduction / billion;
    }

    private double CalculateTotalExportedVolumes(Case caseItem, Project project, DrainageStrategy drainageStrategy, Boolean excludeOilFieldConversion)
    {
        var oilEquivalentFactor = 5.61;
        if (project.PhysicalUnit != 0 && !excludeOilFieldConversion)
        {
            return CalculateTotalOilProduction(caseItem, project, drainageStrategy, false) + CalculateTotalGasProduction(caseItem, project, drainageStrategy, false) / oilEquivalentFactor;
        }
        return CalculateTotalOilProduction(caseItem, project, drainageStrategy, true) + CalculateTotalGasProduction(caseItem, project, drainageStrategy, true);
    }

    private double CalculateTotalStudyCostsPlusOpex(Case caseItem)
    {
        TimeSeriesCost? feasibility = caseItem.TotalFeasibilityAndConceptStudiesOverride?.Override == true ? caseItem.TotalFeasibilityAndConceptStudiesOverride : caseItem.TotalFeasibilityAndConceptStudies;
        TimeSeriesCost? feed = caseItem.TotalFEEDStudiesOverride?.Override == true ? caseItem.TotalFEEDStudiesOverride : caseItem.TotalFEEDStudies;
        TimeSeriesCost? otherStudies = caseItem.TotalOtherStudiesCostProfile;

        var studyTimeSeries = TimeSeriesCost.MergeCostProfilesList(
            [
                feasibility,
                feed,
                otherStudies
            ]);

        TimeSeriesCost? wellIntervention = caseItem.WellInterventionCostProfileOverride?.Override == true ? caseItem.WellInterventionCostProfileOverride : caseItem.WellInterventionCostProfile;
        TimeSeriesCost? offshoreFacilities = caseItem.OffshoreFacilitiesOperationsCostProfileOverride?.Override == true ? caseItem.OffshoreFacilitiesOperationsCostProfileOverride : caseItem.OffshoreFacilitiesOperationsCostProfile;
        TimeSeriesCost? historicCost = caseItem.HistoricCostCostProfile;
        TimeSeriesCost? onshoreOpex = caseItem.OnshoreRelatedOPEXCostProfile;
        TimeSeriesCost? additionalOpex = caseItem.AdditionalOPEXCostProfile;

        var opexTimeSeries = TimeSeriesCost.MergeCostProfilesList(
            [
                wellIntervention,
                offshoreFacilities,
                historicCost,
                onshoreOpex,
                additionalOpex
            ]);

        var sumStudyValues = studyTimeSeries?.Values.Sum() ?? 0;
        var sumOpexValues = opexTimeSeries?.Values.Sum() ?? 0;

        return sumStudyValues + sumOpexValues;
    }

    private double CalculateTotalCessationCosts(Case caseItem)
    {
        TimeSeriesCost? cessationWellsCost = caseItem.CessationWellsCostOverride?.Override == true ? caseItem.CessationWellsCostOverride : caseItem.CessationWellsCost;
        TimeSeriesCost? cessationOffshoreFacilitiesCost = caseItem.CessationOffshoreFacilitiesCostOverride?.Override == true ? caseItem.CessationOffshoreFacilitiesCostOverride : caseItem.CessationOffshoreFacilitiesCost;
        TimeSeriesCost? cessationOnshoreFacilitiesCostProfile = caseItem.CessationOnshoreFacilitiesCostProfile;

        var cessationTimeSeries = TimeSeriesCost.MergeCostProfilesList(
            [
                cessationWellsCost,
                cessationOffshoreFacilitiesCost,
                cessationOnshoreFacilitiesCostProfile
            ]);

        return cessationTimeSeries.Values.Sum();
    }

    private async Task<double> CalculateOffshorePlusOnshoreFacilityCosts(Case caseItem)
    {
        return await _generateStudyCostProfile.SumAllCostFacility(caseItem);
    }

    private async Task<double> CalculateDevelopmentWellCosts(Case caseItem)
    {
        return await _generateStudyCostProfile.SumWellCost(caseItem);
    }

    private double CalculateExplorationWellCosts(Case caseItem, Exploration exploration)
    {
        var sumExplorationWellCost = 0.0;

        try
        {
            if (exploration?.GAndGAdminCost != null)
            {
                sumExplorationWellCost += exploration.GAndGAdminCost.Values.Sum();
            }
            if (exploration?.CountryOfficeCost != null)
            {
                sumExplorationWellCost += exploration.CountryOfficeCost.Values.Sum();
            }
            if (exploration?.SeismicAcquisitionAndProcessing != null)
            {
                sumExplorationWellCost += exploration.SeismicAcquisitionAndProcessing.Values.Sum();
            }
            if (exploration?.ExplorationWellCostProfile != null)
            {
                sumExplorationWellCost += exploration.ExplorationWellCostProfile.Values.Sum();
            }
            if (exploration?.AppraisalWellCostProfile != null)
            {
                sumExplorationWellCost += exploration.AppraisalWellCostProfile.Values.Sum();
            }
            if (exploration?.SidetrackCostProfile != null)
            {
                sumExplorationWellCost += exploration.SidetrackCostProfile.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Exploration {0} not found.", caseItem.ExplorationLink);
        }

        return sumExplorationWellCost;
    }

    private double CalculateCO2Intensity(Case caseItem, Project project, DrainageStrategy drainageStrategy, double totalCo2Emissions)
    {
        var tonnesToKgFactor = 1000;
        var boeConversionFactor = 6.29;
        var totalExportedVolumes = CalculateTotalExportedVolumes(caseItem, project, drainageStrategy, true);
        if (totalExportedVolumes != 0 && totalCo2Emissions != 0)
        {
            return totalCo2Emissions / totalExportedVolumes / boeConversionFactor * tonnesToKgFactor;
        }
        return 0;
    }
}
