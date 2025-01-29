using api.Context;
using api.Features.Profiles;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;

public class CalculateTotalCostService(DcdDbContext context)
{
    public async Task CalculateTotalCost(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(c => c.TimeSeriesProfiles)
            .SingleAsync(x => x.Id == caseId);

        CalculateTotalCost(caseItem);
    }

    public static void CalculateTotalCost(Case caseItem)
    {
        var totalStudyCost = CalculateStudyCost(caseItem);

        var studiesProfile = new TimeSeriesCost
        {
            StartYear = totalStudyCost.StartYear,
            Values = totalStudyCost.Values ?? []
        };

        var totalOpexCost = CalculateOpexCost(caseItem);
        var opexProfile = new TimeSeriesCost
        {
            StartYear = totalOpexCost.StartYear,
            Values = totalOpexCost.Values ?? []
        };

        var totalCessationCost = CalculateCessationCost(caseItem);
        var cessationProfile = new TimeSeriesCost
        {
            StartYear = totalCessationCost.StartYear,
            Values = totalCessationCost.Values ?? []
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

        var totalCost = CostProfileMerger.MergeCostProfiles(
        [
            studiesProfile,
            opexProfile,
            cessationProfile,
            totalOffshoreFacilityProfile,
            developmentProfile,
            explorationProfile
        ]);

        var calculatedTotalCostCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalCostCostProfile);

        calculatedTotalCostCostProfile.Values = totalCost.Values;
        calculatedTotalCostCostProfile.StartYear = totalCost.StartYear;
    }

    private static TimeSeriesCost CalculateStudyCost(Case caseItem)
    {
        TimeSeriesCost feasibilityProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies),
            caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)
        );
        TimeSeriesCost feedProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies),
            caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)
        );

        var totalOtherStudiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile);

        TimeSeriesCost otherStudies = totalOtherStudiesCostProfile != null
            ? new TimeSeriesCost(totalOtherStudiesCostProfile)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var totalStudyCost = CostProfileMerger.MergeCostProfiles(
        [
            feasibilityProfile,
            feedProfile,
            otherStudies
        ]);
        return totalStudyCost;
    }

    private static TimeSeriesCost CalculateOpexCost(Case caseItem)
    {
        TimeSeriesCost wellInterventionProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)
        );
        TimeSeriesCost offshoreFacilitiesProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)
        );

        var historicCostProfile = caseItem.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile);
        TimeSeriesCost historicCost = historicCostProfile != null
            ? new TimeSeriesCost(historicCostProfile)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var onshoreRelatedOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.OnshoreRelatedOPEXCostProfile);
        TimeSeriesCost onshoreRelatedOpex = onshoreRelatedOpexProfile != null
            ? new TimeSeriesCost(onshoreRelatedOpexProfile)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var additionalOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalOPEXCostProfile);
        TimeSeriesCost additionalOpex = additionalOpexProfile != null
            ? new TimeSeriesCost(additionalOpexProfile)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var totalOpexCost = CostProfileMerger.MergeCostProfiles(
        [
            historicCost,
            wellInterventionProfile,
            offshoreFacilitiesProfile,
            onshoreRelatedOpex,
            additionalOpex
        ]);
        return totalOpexCost;
    }

    private static TimeSeriesCost CalculateCessationCost(Case caseItem)
    {
        TimeSeriesCost cessationWellsProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost),
            caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)
        );
        TimeSeriesCost cessationOffshoreFacilitiesProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost),
            caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)
        );

        var cessationOnshoreFacilitiesProfile = caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile);
        TimeSeriesCost cessationOnshoreFacilitiesCost = cessationOnshoreFacilitiesProfile != null
            ? new TimeSeriesCost(cessationOnshoreFacilitiesProfile)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var totalCessationCost = CostProfileMerger.MergeCostProfiles(
        [
            cessationWellsProfile,
            cessationOffshoreFacilitiesProfile,
            cessationOnshoreFacilitiesCost
        ]);
        return totalCessationCost;
    }

    private static TimeSeriesCost CalculateTotalOffshoreFacilityCost(Case caseItem)
    {
        TimeSeriesCost substructureProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride)
        );
        TimeSeriesCost surfProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride)
        );
        TimeSeriesCost topsideProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride)
        );
        TimeSeriesCost transportProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride)
        );
        TimeSeriesCost onshorePowerSupplyProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride)
        );

        var totalOffshoreFacilityCost = CostProfileMerger.MergeCostProfiles(
        [
            substructureProfile,
            surfProfile,
            topsideProfile,
            transportProfile,
            onshorePowerSupplyProfile
        ]);

        return totalOffshoreFacilityCost;
    }

    private static TimeSeriesCost CalculateTotalDevelopmentCost(Case caseItem)
    {
        TimeSeriesCost oilProducerProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride)
        );
        TimeSeriesCost gasProducerProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride)
        );
        TimeSeriesCost waterInjectorProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride)
        );
        TimeSeriesCost gasInjectorProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride)
        );

        var totalDevelopmentCost = CostProfileMerger.MergeCostProfiles(
        [
            oilProducerProfile,
            gasProducerProfile,
            waterInjectorProfile,
            gasInjectorProfile
        ]);

        return totalDevelopmentCost;
    }

    public static TimeSeriesCost CalculateTotalExplorationCost(Case caseItem)
    {
        TimeSeriesCost gAndGAdminCostProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost),
            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)
        );

        var seismicAcquisitionAndProcessingTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing);

        TimeSeriesCost seismicAcquisitionAndProcessingProfile = seismicAcquisitionAndProcessingTimeSeries != null
            ? new TimeSeriesCost(seismicAcquisitionAndProcessingTimeSeries)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var countryOfficeTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost);

        TimeSeriesCost countryOfficeCostProfile = countryOfficeTimeSeries != null
            ? new TimeSeriesCost(countryOfficeTimeSeries)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var explorationWellTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile);

        TimeSeriesCost explorationWellCostProfile = explorationWellTimeSeries != null
            ? new TimeSeriesCost(explorationWellTimeSeries)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var appraisalWellTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile);

        TimeSeriesCost appraisalWellCostProfile = appraisalWellTimeSeries != null
            ? new TimeSeriesCost(appraisalWellTimeSeries)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var sidetrackTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile);

        TimeSeriesCost sidetrackCostProfile = sidetrackTimeSeries != null
            ? new TimeSeriesCost(sidetrackTimeSeries)
            : new TimeSeriesCost { StartYear = 0, Values = [] };

        var totalExploration = CostProfileMerger.MergeCostProfiles(
        [
            gAndGAdminCostProfile,
            seismicAcquisitionAndProcessingProfile,
            countryOfficeCostProfile,
            explorationWellCostProfile,
            appraisalWellCostProfile,
            sidetrackCostProfile
        ]);

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
