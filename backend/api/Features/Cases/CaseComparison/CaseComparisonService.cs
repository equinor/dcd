using api.Features.Cases.Recalculation.Types.TotalProductionVolume;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
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

            var totalOilProduction = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.Values.Sum() / 1_000_000 ?? 0;
            var additionalOilProduction = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.Values.Sum() / 1_000_000 ?? 0;
            var totalGasProduction = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values.Sum() / 1_000_000_000 ?? 0;
            var additionalGasProduction = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values.Sum() / 1_000_000_000 ?? 0;
            var totalExportedVolumes = TotalExportedVolumesProfileService.GetTotalExportedVolumes(caseItem).Values.Sum() / 1_000_000;

            var explorationCosts = CalculateExplorationWellCosts(caseItem);
            var developmentCosts = SumWellCostWithPreloadedData(caseItem);

            var co2EmissionsProfile = caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride)?.Override == true
                ? caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride)
                : caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions);

            var generateCo2EmissionsProfile = new TimeSeries(co2EmissionsProfile);

            var totalCo2Emissions = generateCo2EmissionsProfile.Values.Sum();

            var co2IntensityProfile = caseItem.GetProfileOrNull(ProfileTypes.Co2IntensityOverride)?.Override == true
                ? caseItem.GetProfileOrNull(ProfileTypes.Co2IntensityOverride)
                : caseItem.GetProfileOrNull(ProfileTypes.Co2Intensity);

            var co2Intensity = co2IntensityProfile?.Values.Sum() ?? 0;

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

    private static double CalculateTotalStudyCostsPlusOpex(Case caseItem)
    {
        var feasibilityProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies);

        var feasibility = new TimeSeries(feasibilityProfile);

        var feedProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalFeedStudiesOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.TotalFeedStudiesOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.TotalFeedStudies);

        var feed = new TimeSeries(feedProfile);

        var totalOtherStudiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile);
        var otherStudies = new TimeSeries(totalOtherStudiesCostProfile);

        var studyTimeSeries = TimeSeriesMerger.MergeTimeSeries(feasibility, feed, otherStudies);

        var wellInterventionProfile = caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile);

        var wellIntervention = new TimeSeries(wellInterventionProfile);

        var offshoreFacilitiesProfile = caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile);

        var offshoreFacilities = new TimeSeries(offshoreFacilitiesProfile);

        var historicCostCostProfile = caseItem.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile);
        var historicCost = new TimeSeries(historicCostCostProfile);

        var onshoreRelatedOpexCostProfile = caseItem.GetProfileOrNull(ProfileTypes.OnshoreRelatedOpexCostProfile);
        var onshoreOpex = new TimeSeries(onshoreRelatedOpexCostProfile);

        var additionalOpexCostProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalOpexCostProfile);
        var additionalOpex = new TimeSeries(additionalOpexCostProfile);

        var opexTimeSeries = TimeSeriesMerger.MergeTimeSeries(
            wellIntervention,
            offshoreFacilities,
            historicCost,
            onshoreOpex,
            additionalOpex
        );

        var sumStudyValues = studyTimeSeries.Values.Sum();
        var sumOpexValues = opexTimeSeries.Values.Sum();

        return sumStudyValues + sumOpexValues;
    }

    private static double CalculateTotalCessationCosts(Case caseItem)
    {
        var cessationWellsCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost);

        var cessationWellsCost = new TimeSeries(cessationWellsCostProfile);

        var cessationOffshoreFacilitiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)?.Override == true
            ? caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)
            : caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost);

        var cessationOffshoreFacilitiesCost = new TimeSeries(cessationOffshoreFacilitiesCostProfile);

        var cessationOnshoreFacilitiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile);
        var cessationOnshoreFacilitiesCost = new TimeSeries(cessationOnshoreFacilitiesCostProfile);

        var cessationTimeSeries = TimeSeriesMerger.MergeTimeSeries(cessationWellsCost, cessationOffshoreFacilitiesCost, cessationOnshoreFacilitiesCost);

        return cessationTimeSeries.Values.Sum();
    }

    private static double CalculateExplorationWellCosts(Case caseItem)
    {
        var sumExplorationWellCost = 0.0;

        if (caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost) != null)
        {
            sumExplorationWellCost += caseItem.GetProfile(ProfileTypes.GAndGAdminCost).Values.Sum();
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost) != null)
        {
            sumExplorationWellCost += caseItem.GetProfile(ProfileTypes.CountryOfficeCost).Values.Sum();
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing) != null)
        {
            sumExplorationWellCost += caseItem.GetProfile(ProfileTypes.SeismicAcquisitionAndProcessing).Values.Sum();
        }

        sumExplorationWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile), caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfileOverride));
        sumExplorationWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile), caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfileOverride));
        sumExplorationWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile), caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfileOverride));

        return sumExplorationWellCost;
    }

    private static double SumWellCostWithPreloadedData(Case caseItem)
    {
        var sumWellCost = 0.0;

        sumWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfile), caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride));
        sumWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfile), caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride));
        sumWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfile), caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride));
        sumWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfile), caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride));

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
