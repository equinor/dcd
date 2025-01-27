using api.Features.Profiles;
using api.Features.TimeSeriesCalculators;
using api.Models;

namespace api.Features.Cases.CaseComparison;

public class CaseComparisonService(CaseComparisonRepository caseComparisonRepository)
{
    public async Task<List<CompareCasesDto>> Calculate(Guid projectId)
    {
        var projectPk = await caseComparisonRepository.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        var project = await caseComparisonRepository.LoadProject(projectPk);

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

            var explorationCosts = CalculateExplorationWellCosts(caseItem, exploration);
            var developmentCosts = SumWellCostWithPreloadedData(caseItem);

            TimeSeriesMass? generateCo2EmissionsProfile = drainageStrategy.Co2EmissionsOverride?.Override == true ? drainageStrategy.Co2EmissionsOverride : drainageStrategy.Co2Emissions;

            var totalCo2Emissions = generateCo2EmissionsProfile?.Values.Sum() ?? 0;
            var co2Intensity = drainageStrategy.Co2Intensity?.Values.Sum() ?? 0;

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
        TimeSeriesProfile? feasibilityProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies);

        TimeSeriesCost? feasibility = feasibilityProfile == null ? null : new TimeSeriesCost(feasibilityProfile);

        TimeSeriesProfile? feedProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies);

        TimeSeriesCost? feed = feedProfile == null ? null : new TimeSeriesCost(feedProfile);

        var totalOtherStudiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile);
        TimeSeriesCost? otherStudies = totalOtherStudiesCostProfile == null ? null : new TimeSeriesCost(totalOtherStudiesCostProfile);

        var studyTimeSeries = CostProfileMerger.MergeCostProfiles(feasibility, feed, otherStudies);

        TimeSeriesProfile? wellInterventionProfile = caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile);

        TimeSeriesCost? wellIntervention = wellInterventionProfile == null ? null : new TimeSeriesCost(wellInterventionProfile);

        TimeSeriesProfile? offshoreFacilitiesProfile = caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile);

        TimeSeriesCost? offshoreFacilities = offshoreFacilitiesProfile == null ? null : new TimeSeriesCost(offshoreFacilitiesProfile);

        var historicCostCostProfile = caseItem.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile);
        TimeSeriesCost? historicCost = historicCostCostProfile == null ? null : new TimeSeriesCost(historicCostCostProfile);

        var onshoreRelatedOpexCostProfile = caseItem.GetProfileOrNull(ProfileTypes.OnshoreRelatedOPEXCostProfile);
        TimeSeriesCost? onshoreOpex = onshoreRelatedOpexCostProfile == null ? null : new TimeSeriesCost(onshoreRelatedOpexCostProfile);

        var additionalOpexCostProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalOPEXCostProfile);
        TimeSeriesCost? additionalOpex = additionalOpexCostProfile == null ? null : new TimeSeriesCost(additionalOpexCostProfile);

        var opexTimeSeries = CostProfileMerger.MergeCostProfiles(
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
        var cessationWellsCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost);

        TimeSeriesCost? cessationWellsCost = cessationWellsCostProfile == null ? null : new TimeSeriesCost(cessationWellsCostProfile);

        var cessationOffshoreFacilitiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost);

        TimeSeriesCost? cessationOffshoreFacilitiesCost = cessationOffshoreFacilitiesCostProfile == null ? null : new TimeSeriesCost(cessationOffshoreFacilitiesCostProfile);

        var cessationOnshoreFacilitiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile);
        TimeSeriesCost? cessationOnshoreFacilitiesCost = cessationOnshoreFacilitiesCostProfile == null ? null : new TimeSeriesCost(cessationOnshoreFacilitiesCostProfile);

        var cessationTimeSeries = CostProfileMerger.MergeCostProfiles(cessationWellsCost, cessationOffshoreFacilitiesCost, cessationOnshoreFacilitiesCost);

        return cessationTimeSeries.Values.Sum();
    }

    private static double CalculateExplorationWellCosts(Case caseItem, Exploration exploration)
    {
        var sumExplorationWellCost = 0.0;

        if (caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost) != null)
        {
            sumExplorationWellCost += caseItem.GetProfile(ProfileTypes.GAndGAdminCost).Values.Sum();
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

        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile), caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride));
        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile), caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride));
        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile), caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride));
        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile), caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride));
        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile), caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride));

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

    private static double SumOverrideOrProfile(TimeSeriesProfile? profile, TimeSeriesProfile? profileOverride)
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
