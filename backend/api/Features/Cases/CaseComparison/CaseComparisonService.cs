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

            var co2EmissionsProfile = caseItem.GetOverrideProfileOrProfile(ProfileTypes.Co2Emissions);

            var generateCo2EmissionsProfile = new TimeSeries(co2EmissionsProfile);

            var totalCo2Emissions = generateCo2EmissionsProfile.Values.Sum();

            var co2IntensityProfile = caseItem.GetOverrideProfileOrProfile(ProfileTypes.Co2Intensity);

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
        var feasibilityProfile = caseItem.GetOverrideProfileOrProfile(ProfileTypes.TotalFeasibilityAndConceptStudies);

        var feasibility = new TimeSeries(feasibilityProfile);

        var feedProfile = caseItem.GetOverrideProfileOrProfile(ProfileTypes.TotalFeedStudies);

        var feed = new TimeSeries(feedProfile);

        var totalOtherStudiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile);
        var otherStudies = new TimeSeries(totalOtherStudiesCostProfile);

        var studyTimeSeries = TimeSeriesMerger.MergeTimeSeries(feasibility, feed, otherStudies);

        var wellInterventionProfile = caseItem.GetOverrideProfileOrProfile(ProfileTypes.WellInterventionCostProfile);

        var wellIntervention = new TimeSeries(wellInterventionProfile);

        var offshoreFacilitiesProfile = caseItem.GetOverrideProfileOrProfile(ProfileTypes.OffshoreFacilitiesOperationsCostProfile);

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
        var cessationWellsCostProfile = caseItem.GetOverrideProfileOrProfile(ProfileTypes.CessationWellsCost);

        var cessationWellsCost = new TimeSeries(cessationWellsCostProfile);

        var cessationOffshoreFacilitiesCostProfile = caseItem.GetOverrideProfileOrProfile(ProfileTypes.CessationOffshoreFacilitiesCost);

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

        sumExplorationWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.ExplorationWellCostProfile)?.Values.Sum() ?? 0;
        sumExplorationWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.AppraisalWellCostProfile)?.Values.Sum() ?? 0;
        sumExplorationWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.SidetrackCostProfile)?.Values.Sum() ?? 0;

        return sumExplorationWellCost;
    }

    private static double SumWellCostWithPreloadedData(Case caseItem)
    {
        var sumWellCost = 0.0;

        sumWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.OilProducerCostProfile)?.Values.Sum() ?? 0;
        sumWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.GasProducerCostProfile)?.Values.Sum() ?? 0;
        sumWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.WaterInjectorCostProfile)?.Values.Sum() ?? 0;
        sumWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.GasInjectorCostProfile)?.Values.Sum() ?? 0;

        return sumWellCost;
    }

    private static double SumAllCostFacilityWithPreloadedData(Case caseItem)
    {
        var sumFacilityCost = 0.0;

        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.SubstructureCostProfile)?.Values.Sum() ?? 0;
        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.SurfCostProfile)?.Values.Sum() ?? 0;
        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.TopsideCostProfile)?.Values.Sum() ?? 0;
        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.TransportCostProfile)?.Values.Sum() ?? 0;
        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.OnshorePowerSupplyCostProfile)?.Values.Sum() ?? 0;

        return sumFacilityCost;
    }
}
