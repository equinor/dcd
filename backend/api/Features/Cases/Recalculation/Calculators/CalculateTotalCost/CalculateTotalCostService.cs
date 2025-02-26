using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;

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
            Values = totalCost.Values.Select(v => v / caseItem.Project.ExchangeRateUSDToNOK).ToArray()
        };

        var CalculatedTotalCostCostProfileUsd = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalCostCostProfileUsd);

        CalculatedTotalCostCostProfileUsd.Values = totalCostUsd.Values;
        CalculatedTotalCostCostProfileUsd.StartYear = totalCostUsd.StartYear;
    }

    private static TimeSeries CalculateStudyCost(Case caseItem)
    {
        var feasibilityProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies),
            caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)
        );

        var feedProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies),
            caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)
        );

        var totalOtherStudiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile);

        var otherStudies = new TimeSeries(totalOtherStudiesCostProfile);

        var totalStudyCost = TimeSeriesMerger.MergeTimeSeries(feasibilityProfile, feedProfile, otherStudies);
        return totalStudyCost;
    }

    private static TimeSeries CalculateOpexCost(Case caseItem)
    {
        var wellInterventionProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)
        );
        var offshoreFacilitiesProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)
        );

        var historicCostProfile = caseItem.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile);
        var historicCost = new TimeSeries(historicCostProfile);

        var onshoreRelatedOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.OnshoreRelatedOPEXCostProfile);
        var onshoreRelatedOpex = new TimeSeries(onshoreRelatedOpexProfile);

        var additionalOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalOPEXCostProfile);
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
        var cessationWellsProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost),
            caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)
        );
        var cessationOffshoreFacilitiesProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost),
            caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)
        );

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
        var substructureProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride)
        );
        var surfProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride)
        );
        var topsideProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride)
        );
        var transportProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride)
        );
        var onshorePowerSupplyProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride)
        );

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
        var oilProducerProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride)
        );
        var gasProducerProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride)
        );
        var waterInjectorProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride)
        );
        var gasInjectorProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride)
        );

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
        var gAndGAdminCostProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost),
            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)
        );

        var seismicAcquisitionAndProcessingTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing);

        var seismicAcquisitionAndProcessingProfile = new TimeSeries(seismicAcquisitionAndProcessingTimeSeries);

        var countryOfficeTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost);

        var countryOfficeCostProfile = new TimeSeries(countryOfficeTimeSeries);

        var explorationWellTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile);

        var explorationWellCostProfile = new TimeSeries(explorationWellTimeSeries);

        var appraisalWellTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile);

        var appraisalWellCostProfile = new TimeSeries(appraisalWellTimeSeries);

        var sidetrackTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile);

        var sidetrackCostProfile = new TimeSeries(sidetrackTimeSeries);

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

    private static TimeSeries UseOverrideOrProfile(TimeSeriesProfile? profile, TimeSeriesProfile? profileOverride)
    {
        return profileOverride?.Override == true
            ? new TimeSeries(profileOverride)
            : new TimeSeries(profile);
    }
}
