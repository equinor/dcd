using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Cost;

public static class CalculateTotalCostService
{
    public static void RunCalculation(Case caseItem)
    {
        var totalStudyCost = CalculateStudyCost(caseItem);

        var studiesProfile = new TimeSeries
        {
            StartYear = totalStudyCost.StartYear,
            Values = totalStudyCost.Values
        };

        var totalOpexCost = CalculateOpexCost(caseItem);

        var opexProfile = new TimeSeries
        {
            StartYear = totalOpexCost.StartYear,
            Values = totalOpexCost.Values
        };

        var totalCessationCost = CalculateCessationCost(caseItem);

        var cessationProfile = new TimeSeries
        {
            StartYear = totalCessationCost.StartYear,
            Values = totalCessationCost.Values
        };

        var totalOffshoreFacilityCost = CalculateTotalOffshoreFacilityCost(caseItem);

        var totalOffshoreFacilityProfile = new TimeSeries
        {
            StartYear = totalOffshoreFacilityCost.StartYear,
            Values = totalOffshoreFacilityCost.Values
        };

        var totalDevelopmentCost = CalculateTotalDevelopmentCost(caseItem);

        var developmentProfile = new TimeSeries
        {
            StartYear = totalDevelopmentCost.StartYear,
            Values = totalDevelopmentCost.Values
        };

        var explorationCost = CalculateTotalExplorationCost(caseItem);

        var explorationProfile = new TimeSeries
        {
            StartYear = explorationCost.StartYear,
            Values = explorationCost.Values
        };

        var totalCost = TimeSeriesMerger.MergeTimeSeries(
            studiesProfile,
            opexProfile,
            cessationProfile,
            totalOffshoreFacilityProfile,
            developmentProfile,
            explorationProfile
        );

        var totalCostUsd = new TimeSeries
        {
            StartYear = totalCost.StartYear,
            Values = totalCost.Values.Select(v => v / caseItem.Project.ExchangeRateUsdToNok).ToArray()
        };

        var calculatedTotalCostCostProfileUsd = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalCostCostProfileUsd);

        calculatedTotalCostCostProfileUsd.Values = totalCostUsd.Values;
        calculatedTotalCostCostProfileUsd.StartYear = totalCostUsd.StartYear;
    }

    private static TimeSeries CalculateStudyCost(Case caseItem)
    {
        var feasibilityProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TotalFeasibilityAndConceptStudies));

        var feedProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TotalFeedStudies));

        var totalOtherStudiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile);

        var otherStudies = new TimeSeries(totalOtherStudiesCostProfile);

        var totalStudyCost = TimeSeriesMerger.MergeTimeSeries(feasibilityProfile, feedProfile, otherStudies);

        return totalStudyCost;
    }

    private static TimeSeries CalculateOpexCost(Case caseItem)
    {
        var wellInterventionProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.WellInterventionCostProfile));

        var offshoreFacilitiesProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.OffshoreFacilitiesOperationsCostProfile));

        var historicCostProfile = caseItem.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile);
        var historicCost = new TimeSeries(historicCostProfile);

        var onshoreRelatedOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.OnshoreRelatedOpexCostProfile);
        var onshoreRelatedOpex = new TimeSeries(onshoreRelatedOpexProfile);

        var additionalOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalOpexCostProfile);
        var additionalOpex = new TimeSeries(additionalOpexProfile);

        var totalOpexCost = TimeSeriesMerger.MergeTimeSeries(
            historicCost,
            wellInterventionProfile,
            offshoreFacilitiesProfile,
            onshoreRelatedOpex,
            additionalOpex
        );

        return totalOpexCost;
    }

    private static TimeSeries CalculateCessationCost(Case caseItem)
    {
        var cessationWellsProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.CessationWellsCost));

        var cessationOffshoreFacilitiesProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.CessationOffshoreFacilitiesCost));

        var cessationOnshoreFacilitiesProfile = caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile);
        var cessationOnshoreFacilitiesCost = new TimeSeries(cessationOnshoreFacilitiesProfile);

        var totalCessationCost = TimeSeriesMerger.MergeTimeSeries(
            cessationWellsProfile,
            cessationOffshoreFacilitiesProfile,
            cessationOnshoreFacilitiesCost
        );

        return totalCessationCost;
    }

    private static TimeSeries CalculateTotalOffshoreFacilityCost(Case caseItem)
    {
        var substructureProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.SubstructureCostProfile));

        var surfProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.SurfCostProfile));

        var topsideProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TopsideCostProfile));

        var transportProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TransportCostProfile));

        var onshorePowerSupplyProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.OnshorePowerSupplyCostProfile));

        var totalOffshoreFacilityCost = TimeSeriesMerger.MergeTimeSeries(
            substructureProfile,
            surfProfile,
            topsideProfile,
            transportProfile,
            onshorePowerSupplyProfile
        );

        return totalOffshoreFacilityCost;
    }

    private static TimeSeries CalculateTotalDevelopmentCost(Case caseItem)
    {
        var oilProducerProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.OilProducerCostProfile));

        var gasProducerProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.GasProducerCostProfile));

        var waterInjectorProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.WaterInjectorCostProfile));

        var gasInjectorProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.GasInjectorCostProfile));

        var totalDevelopmentCost = TimeSeriesMerger.MergeTimeSeries(
            oilProducerProfile,
            gasProducerProfile,
            waterInjectorProfile,
            gasInjectorProfile
        );

        return totalDevelopmentCost;
    }

    public static TimeSeries CalculateTotalExplorationCost(Case caseItem)
    {
        var gAndGAdminCostProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.GAndGAdminCost));

        var seismicAcquisitionAndProcessingTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing);

        var seismicAcquisitionAndProcessingProfile = new TimeSeries(seismicAcquisitionAndProcessingTimeSeries);

        var countryOfficeTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost);

        var countryOfficeCostProfile = new TimeSeries(countryOfficeTimeSeries);

        var explorationWellCostProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ExplorationWellCostProfile));

        var appraisalWellCostProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.AppraisalWellCostProfile));

        var sidetrackCostProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.SidetrackCostProfile));

        var totalExploration = TimeSeriesMerger.MergeTimeSeries(
            gAndGAdminCostProfile,
            seismicAcquisitionAndProcessingProfile,
            countryOfficeCostProfile,
            explorationWellCostProfile,
            appraisalWellCostProfile,
            sidetrackCostProfile
        );

        return totalExploration;
    }
}
