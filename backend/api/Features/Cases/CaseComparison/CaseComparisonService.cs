using api.Dtos;
using api.Models;

namespace api.Services;

public class CaseComparisonService(CaseComparisonRepository caseComparisonRepository)
{
    public async Task<List<CompareCasesDto>> Calculate(Guid projectId)
    {
        var project = await caseComparisonRepository.LoadProject(projectId);

        return CompareCases(project);
    }

    private static List<CompareCasesDto> CompareCases(Project project)
    {
        var caseList = new List<CompareCasesDto>();

        foreach (var caseItem in project.Cases)
        {
            if (caseItem.Archived)
            {
                continue;
            }

            var drainageStrategy = caseItem.DrainageStrategy!;
            var exploration = caseItem.Exploration!;

            var totalOilProduction = drainageStrategy.ProductionProfileOil?.Values.Sum() / 1_000_000 ?? 0;
            var additionalOilProduction = drainageStrategy.AdditionalProductionProfileOil?.Values.Sum() / 1_000_000 ?? 0;
            var totalGasProduction = drainageStrategy.ProductionProfileGas?.Values.Sum() / 1_000_000_000 ?? 0;
            var additionalGasProduction = drainageStrategy.AdditionalProductionProfileGas?.Values.Sum() / 1_000_000_000 ?? 0;
            var totalExportedVolumes = CalculateTotalExportedVolumes(project, drainageStrategy, false);

            var explorationCosts = CalculateExplorationWellCosts(exploration);
            var developmentCosts = SumWellCostWithPreloadedData(caseItem);

            TimeSeriesMass? generateCo2EmissionsProfile = drainageStrategy.Co2EmissionsOverride?.Override == true ? drainageStrategy.Co2EmissionsOverride : drainageStrategy.Co2Emissions;

            var totalCo2Emissions = generateCo2EmissionsProfile?.Values.Sum() ?? 0;
            var co2Intensity = CalculateCo2Intensity(project, drainageStrategy, totalCo2Emissions);

            var totalCessationCosts = CalculateTotalCessationCosts(caseItem);

            var totalStudyCostsPlusOpex = CalculateTotalStudyCostsPlusOpex(caseItem);
            var offshorePlusOnshoreFacilityCosts = SumAllCostFacilityWithPreloadedData(caseItem);

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
                Co2Intensity = co2Intensity
            });
        }

        return caseList;
    }

    private static double CalculateTotalOilProduction(Project project, DrainageStrategy drainageStrategy, bool excludeOilFieldConversion)
    {
        const double million = 1E6;
        const double bblConversionFactor = 6.29;

        var sumOilProduction = 0.0;

        if (drainageStrategy.ProductionProfileOil != null)
        {
            sumOilProduction += drainageStrategy.ProductionProfileOil.Values.Sum();
        }

        if (drainageStrategy.AdditionalProductionProfileOil != null)
        {
            sumOilProduction += drainageStrategy.AdditionalProductionProfileOil.Values.Sum();
        }

        if (project.PhysicalUnit != 0 && !excludeOilFieldConversion)
        {
            return sumOilProduction * bblConversionFactor / million;
        }

        return sumOilProduction / million;
    }

    private static double CalculateTotalGasProduction(Project project, DrainageStrategy drainageStrategy, bool excludeOilFieldConversion)
    {
        const double billion = 1E9;
        const double scfConversionFactor = 35.315;

        var sumGasProduction = 0.0;

        if (drainageStrategy.ProductionProfileGas != null)
        {
            sumGasProduction += drainageStrategy.ProductionProfileGas.Values.Sum();
        }

        if (drainageStrategy.AdditionalProductionProfileGas != null)
        {
            sumGasProduction += drainageStrategy.AdditionalProductionProfileGas.Values.Sum();
        }

        if (project.PhysicalUnit != 0 && !excludeOilFieldConversion)
        {
            return sumGasProduction * scfConversionFactor / billion;
        }

        return sumGasProduction / billion;
    }

    private static double CalculateTotalExportedVolumes(Project project, DrainageStrategy drainageStrategy, bool excludeOilFieldConversion)
    {
        const double oilEquivalentFactor = 5.61;

        if (project.PhysicalUnit != 0 && !excludeOilFieldConversion)
        {
            return CalculateTotalOilProduction(project, drainageStrategy, false) + CalculateTotalGasProduction(project, drainageStrategy, false) / oilEquivalentFactor;
        }

        return CalculateTotalOilProduction(project, drainageStrategy, true) + CalculateTotalGasProduction(project, drainageStrategy, true);
    }

    private static double CalculateTotalStudyCostsPlusOpex(Case caseItem)
    {
        TimeSeriesCost? feasibility = caseItem.TotalFeasibilityAndConceptStudiesOverride?.Override == true
            ? caseItem.TotalFeasibilityAndConceptStudiesOverride
            : caseItem.TotalFeasibilityAndConceptStudies;
        TimeSeriesCost? feed = caseItem.TotalFEEDStudiesOverride?.Override == true ? caseItem.TotalFEEDStudiesOverride : caseItem.TotalFEEDStudies;
        TimeSeriesCost? otherStudies = caseItem.TotalOtherStudiesCostProfile;

        var studyTimeSeries = TimeSeriesCost.MergeCostProfilesList([feasibility, feed, otherStudies]);

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

        var cessationTimeSeries = TimeSeriesCost.MergeCostProfilesList([cessationWellsCost, cessationOffshoreFacilitiesCost, cessationOnshoreFacilitiesCostProfile]);

        return cessationTimeSeries.Values.Sum();
    }

    private static double CalculateExplorationWellCosts(Exploration exploration)
    {
        var sumExplorationWellCost = 0.0;

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

        return sumExplorationWellCost;
    }

    private static double CalculateCo2Intensity(Project project, DrainageStrategy drainageStrategy, double totalCo2Emissions)
    {
        const int tonnesToKgFactor = 1000;
        const double boeConversionFactor = 6.29;

        var totalExportedVolumes = CalculateTotalExportedVolumes(project, drainageStrategy, true);

        if (totalExportedVolumes != 0 && totalCo2Emissions != 0)
        {
            return totalCo2Emissions / totalExportedVolumes / boeConversionFactor * tonnesToKgFactor;
        }

        return 0;
    }

    private static double SumWellCostWithPreloadedData(Case caseItem)
    {
        var sumWellCost = 0.0;

        var wellProject = caseItem.WellProject!;

        sumWellCost += SumOverrideOrProfile(wellProject.OilProducerCostProfile, wellProject.OilProducerCostProfileOverride);
        sumWellCost += SumOverrideOrProfile(wellProject.GasProducerCostProfile, wellProject.GasProducerCostProfileOverride);
        sumWellCost += SumOverrideOrProfile(wellProject.WaterInjectorCostProfile, wellProject.WaterInjectorCostProfileOverride);
        sumWellCost += SumOverrideOrProfile(wellProject.GasInjectorCostProfile, wellProject.GasInjectorCostProfileOverride);

        return sumWellCost;
    }

    private static double SumAllCostFacilityWithPreloadedData(Case caseItem)
    {
        var sumFacilityCost = 0.0;

        var substructure = caseItem.Substructure!;
        var surf = caseItem.Surf!;
        var topside = caseItem.Topside!;
        var transport = caseItem.Transport!;

        sumFacilityCost += SumOverrideOrProfile(substructure.CostProfile, substructure.CostProfileOverride);
        sumFacilityCost += SumOverrideOrProfile(surf.CostProfile, surf.CostProfileOverride);
        sumFacilityCost += SumOverrideOrProfile(topside.CostProfile, topside.CostProfileOverride);
        sumFacilityCost += SumOverrideOrProfile(transport.CostProfile, transport.CostProfileOverride);

        return sumFacilityCost;
    }

    private static double SumOverrideOrProfile<T>(TimeSeries<double>? profile, T? profileOverride)
        where T : TimeSeries<double>, ITimeSeriesOverride
    {
        if (profileOverride?.Override == true)
        {
            return profileOverride.Values.Sum();
        }

        if (profile != null)
        {
            return profile.Values.Sum();
        }

        return 0;
    }
}
