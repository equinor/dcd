using api.Dtos;
using api.Models;

namespace api.Services;

public class CompareCasesService(
    ILogger<CompareCasesService> logger,
    IStudyCostProfileService generateStudyCostProfile,
    CompareCaseRepository compareCaseRepository)
    : ICompareCasesService
{
    public async Task<List<CompareCasesDto>> Calculate(Guid projectId)
    {
        var project = await compareCaseRepository.LoadProject(projectId);

        var caseList = new List<CompareCasesDto>();

        foreach (var caseItem in project.Cases)
        {
            if (caseItem.Archived)
            {
                continue;
            }

            var drainageStrategy = caseItem.DrainageStrategy!;
            var exploration = caseItem.Exploration!;

            var totalOilProduction = drainageStrategy.ProductionProfileOil?.Values.Sum() / 1000000 ?? 0;
            var additionalOilProduction = drainageStrategy.AdditionalProductionProfileOil?.Values.Sum() / 1000000 ?? 0;
            var totalGasProduction = drainageStrategy.ProductionProfileGas?.Values.Sum() / 1000000000 ?? 0;
            var additionalGasProduction = drainageStrategy.AdditionalProductionProfileGas?.Values.Sum() / 1000000000 ?? 0;
            var totalExportedVolumes = CalculateTotalExportedVolumes(caseItem, project, drainageStrategy, false);

            var explorationCosts = CalculateExplorationWellCosts(caseItem, exploration);
            var developmentCosts = CalculateDevelopmentWellCosts(caseItem);

            TimeSeriesMass? generateCo2EmissionsProfile = drainageStrategy.Co2EmissionsOverride?.Override == true ? drainageStrategy.Co2EmissionsOverride : drainageStrategy.Co2Emissions;

            var totalCo2Emissions = generateCo2EmissionsProfile?.Values.Sum() ?? 0;
            var co2Intensity = CalculateCo2Intensity(caseItem, project, drainageStrategy, totalCo2Emissions);

            var totalCessationCosts = CalculateTotalCessationCosts(caseItem);

            var totalStudyCostsPlusOpex = CalculateTotalStudyCostsPlusOpex(caseItem);
            var offshorePlusOnshoreFacilityCosts = CalculateOffshorePlusOnshoreFacilityCosts(caseItem);

            caseList.Add(new CompareCasesDto
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
            });
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
            logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
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
            logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
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
        TimeSeriesCost? feasibility = caseItem.TotalFeasibilityAndConceptStudiesOverride?.Override == true
            ? caseItem.TotalFeasibilityAndConceptStudiesOverride
            : caseItem.TotalFeasibilityAndConceptStudies;
        TimeSeriesCost? feed = caseItem.TotalFEEDStudiesOverride?.Override == true ? caseItem.TotalFEEDStudiesOverride : caseItem.TotalFEEDStudies;
        TimeSeriesCost? otherStudies = caseItem.TotalOtherStudiesCostProfile;

        var studyTimeSeries = TimeSeriesCost.MergeCostProfilesList(
        [
            feasibility,
            feed,
            otherStudies
        ]);

        TimeSeriesCost? wellIntervention = caseItem.WellInterventionCostProfileOverride?.Override == true ? caseItem.WellInterventionCostProfileOverride : caseItem.WellInterventionCostProfile;
        TimeSeriesCost? offshoreFacilities = caseItem.OffshoreFacilitiesOperationsCostProfileOverride?.Override == true
            ? caseItem.OffshoreFacilitiesOperationsCostProfileOverride
            : caseItem.OffshoreFacilitiesOperationsCostProfile;
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

        var sumStudyValues = studyTimeSeries.Values.Sum();
        var sumOpexValues = opexTimeSeries.Values.Sum();

        return sumStudyValues + sumOpexValues;
    }

    private static double CalculateTotalCessationCosts(Case caseItem)
    {
        TimeSeriesCost? cessationWellsCost = caseItem.CessationWellsCostOverride?.Override == true ? caseItem.CessationWellsCostOverride : caseItem.CessationWellsCost;
        TimeSeriesCost? cessationOffshoreFacilitiesCost = caseItem.CessationOffshoreFacilitiesCostOverride?.Override == true
            ? caseItem.CessationOffshoreFacilitiesCostOverride
            : caseItem.CessationOffshoreFacilitiesCost;
        TimeSeriesCost? cessationOnshoreFacilitiesCostProfile = caseItem.CessationOnshoreFacilitiesCostProfile;

        var cessationTimeSeries = TimeSeriesCost.MergeCostProfilesList(
        [
            cessationWellsCost,
            cessationOffshoreFacilitiesCost,
            cessationOnshoreFacilitiesCostProfile
        ]);

        return cessationTimeSeries.Values.Sum();
    }

    private double CalculateOffshorePlusOnshoreFacilityCosts(Case caseItem)
    {
        return generateStudyCostProfile.SumAllCostFacilityWithPreloadedData(caseItem);
    }

    private double CalculateDevelopmentWellCosts(Case caseItem)
    {
        return generateStudyCostProfile.SumWellCostWithPreloadedData(caseItem);
    }

    private double CalculateExplorationWellCosts(Case caseItem, Exploration exploration)
    {
        var sumExplorationWellCost = 0.0;

        try
        {
            if (exploration.GAndGAdminCost != null)
            {
                sumExplorationWellCost += exploration.GAndGAdminCost.Values.Sum();
            }

            if (exploration.CountryOfficeCost != null)
            {
                sumExplorationWellCost += exploration.CountryOfficeCost.Values.Sum();
            }

            if (exploration.SeismicAcquisitionAndProcessing != null)
            {
                sumExplorationWellCost += exploration.SeismicAcquisitionAndProcessing.Values.Sum();
            }

            if (exploration.ExplorationWellCostProfile != null)
            {
                sumExplorationWellCost += exploration.ExplorationWellCostProfile.Values.Sum();
            }

            if (exploration.AppraisalWellCostProfile != null)
            {
                sumExplorationWellCost += exploration.AppraisalWellCostProfile.Values.Sum();
            }

            if (exploration.SidetrackCostProfile != null)
            {
                sumExplorationWellCost += exploration.SidetrackCostProfile.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            logger.LogInformation("Exploration {0} not found.", caseItem.ExplorationLink);
        }

        return sumExplorationWellCost;
    }

    private double CalculateCo2Intensity(Case caseItem, Project project, DrainageStrategy drainageStrategy, double totalCo2Emissions)
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
