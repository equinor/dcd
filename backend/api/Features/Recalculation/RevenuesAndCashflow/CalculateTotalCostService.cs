using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Recalculation.Cost;

public static class CalculateTotalCostService
{
    /// <summary>
    /// sum cost for study, opex, cessation, offshore facility, development and exploration
    /// Note: missing Co2 Tax, tariffs oil, tariffs ngl, tariffs sales gas, cost of electricity.
    /// </summary>
    public static void RunCalculation(Case caseItem)
    {
        var totalStudyCost = CalculateStudyCost(caseItem);
        var totalOpexCost = CalculateOpexCost(caseItem);
        var totalCessationCost = CalculateCessationCost(caseItem);//TODO

        var totalOffshoreFacilityCost = CalculateTotalOffshoreFacilityCost(caseItem);
        var totalDevelopmentCost = CalculateTotalDevelopmentCost(caseItem);
        var explorationCost = CalculateTotalExplorationCost(caseItem);

        var totalCostMega = TimeSeriesMerger.MergeTimeSeries(
            totalStudyCost,
            totalOpexCost,
            totalCessationCost,
            totalOffshoreFacilityCost,
            totalDevelopmentCost,
            explorationCost
        );

        var totalCost = new TimeSeries(
                totalCostMega.StartYear,
                totalCostMega.Values.Select(x => x * Mega).ToArray()
            );

        var calculatedTotalCostCostProfileUsd = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalCostCostProfile);

        calculatedTotalCostCostProfileUsd.Values = totalCost.Values;
        calculatedTotalCostCostProfileUsd.StartYear = totalCost.StartYear;
    }

    private static TimeSeries CalculateStudyCost(Case caseItem)
    {
        var feasibilityProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TotalFeasibilityAndConceptStudies));
        var feedProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TotalFeedStudies));
        var totalOtherStudiesCostProfile = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile));

        var totalStudyCost = TimeSeriesMerger.MergeTimeSeries(feasibilityProfile, feedProfile, totalOtherStudiesCostProfile);

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
