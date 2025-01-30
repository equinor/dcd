using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.TimeSeriesCalculators;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;

public static class CalculateTotalCostService
{
    public static void RunCalculation(Case caseItem)
    {
        var totalStudyCost = CalculateStudyCost(caseItem);

        var studiesProfile = new TimeSeriesCost
        {
            StartYear = totalStudyCost.StartYear,
            Values = totalStudyCost.Values
        };

        var totalOpexCost = CalculateOpexCost(caseItem);
        var opexProfile = new TimeSeriesCost
        {
            StartYear = totalOpexCost.StartYear,
            Values = totalOpexCost.Values
        };

        var totalCessationCost = CalculateCessationCost(caseItem);
        var cessationProfile = new TimeSeriesCost
        {
            StartYear = totalCessationCost.StartYear,
            Values = totalCessationCost.Values
        };

        var totalOffshoreFacilityCost = CalculateTotalOffshoreFacilityCost(caseItem);
        var totalOffshoreFacilityProfile = new TimeSeriesCost
        {
            StartYear = totalOffshoreFacilityCost.StartYear,
            Values = totalOffshoreFacilityCost.Values
        };

        var totalDevelopmentCost = CalculateTotalDevelopmentCost(caseItem);
        var developmentProfile = new TimeSeriesCost
        {
            StartYear = totalDevelopmentCost.StartYear,
            Values = totalDevelopmentCost.Values
        };

        var explorationCost = CalculateTotalExplorationCost(caseItem);
        var explorationProfile = new TimeSeriesCost
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

        var calculatedTotalCostCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalCostCostProfile);

        calculatedTotalCostCostProfile.Values = totalCost.Values;
        calculatedTotalCostCostProfile.StartYear = totalCost.StartYear;
    }

    private static TimeSeriesCost CalculateStudyCost(Case caseItem)
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

        var otherStudies = new TimeSeriesCost(totalOtherStudiesCostProfile);

        var totalStudyCost = TimeSeriesMerger.MergeTimeSeries(feasibilityProfile, feedProfile, otherStudies);
        return totalStudyCost;
    }

    private static TimeSeriesCost CalculateOpexCost(Case caseItem)
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
        var historicCost = new TimeSeriesCost(historicCostProfile);

        var onshoreRelatedOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.OnshoreRelatedOPEXCostProfile);
        var onshoreRelatedOpex = new TimeSeriesCost(onshoreRelatedOpexProfile);

        var additionalOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalOPEXCostProfile);
        var additionalOpex = new TimeSeriesCost(additionalOpexProfile);

        var totalOpexCost = TimeSeriesMerger.MergeTimeSeries(
            historicCost,
            wellInterventionProfile,
            offshoreFacilitiesProfile,
            onshoreRelatedOpex,
            additionalOpex
        );
        return totalOpexCost;
    }

    private static TimeSeriesCost CalculateCessationCost(Case caseItem)
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
        var cessationOnshoreFacilitiesCost = new TimeSeriesCost(cessationOnshoreFacilitiesProfile);

        var totalCessationCost = TimeSeriesMerger.MergeTimeSeries(
            cessationWellsProfile,
            cessationOffshoreFacilitiesProfile,
            cessationOnshoreFacilitiesCost
        );
        return totalCessationCost;
    }

    private static TimeSeriesCost CalculateTotalOffshoreFacilityCost(Case caseItem)
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

    private static TimeSeriesCost CalculateTotalDevelopmentCost(Case caseItem)
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

    public static TimeSeriesCost CalculateTotalExplorationCost(Case caseItem)
    {
        var gAndGAdminCostProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost),
            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)
        );

        var seismicAcquisitionAndProcessingTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing);

        var seismicAcquisitionAndProcessingProfile = new TimeSeriesCost(seismicAcquisitionAndProcessingTimeSeries);

        var countryOfficeTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost);

        var countryOfficeCostProfile = new TimeSeriesCost(countryOfficeTimeSeries);

        var explorationWellTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile);

        var explorationWellCostProfile = new TimeSeriesCost(explorationWellTimeSeries);

        var appraisalWellTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile);

        var appraisalWellCostProfile = new TimeSeriesCost(appraisalWellTimeSeries);

        var sidetrackTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile);

        var sidetrackCostProfile = new TimeSeriesCost(sidetrackTimeSeries);

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

    private static TimeSeriesCost UseOverrideOrProfile(TimeSeriesProfile? profile, TimeSeriesProfile? profileOverride)
    {
        if (profileOverride?.Override == true)
        {
            return new TimeSeriesCost
            {
                StartYear = profileOverride.StartYear,
                Values = profileOverride.Values
            };
        }

        if (profile != null)
        {
            return new TimeSeriesCost
            {
                StartYear = profile.StartYear,
                Values = profile.Values
            };
        }

        return new TimeSeriesCost { StartYear = 0, Values = [] };
    }
}
