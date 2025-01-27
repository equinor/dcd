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

        var wellProject = await context.WellProjects
            .Include(x => x.OilProducerCostProfileOverride)
            .Include(x => x.OilProducerCostProfile)
            .Include(x => x.GasProducerCostProfileOverride)
            .Include(x => x.GasProducerCostProfile)
            .Include(x => x.WaterInjectorCostProfileOverride)
            .Include(x => x.WaterInjectorCostProfile)
            .Include(x => x.GasInjectorCostProfileOverride)
            .Include(x => x.GasInjectorCostProfile)
            .SingleAsync(x => x.Id == caseItem.WellProjectLink);

        var exploration = await context.Explorations
            .Include(e => e.CountryOfficeCost)
            .Include(e => e.SeismicAcquisitionAndProcessing)
            .Include(e => e.AppraisalWellCostProfile)
            .Include(e => e.SidetrackCostProfile)
            .SingleAsync(x => x.Id == caseItem.ExplorationLink);

        CalculateTotalCost(caseItem, wellProject, exploration);
    }

    public static void CalculateTotalCost(Case caseItem, WellProject wellProject, Exploration exploration)
    {
        var totalStudyCost = CalculateStudyCost(caseItem);

        var studiesProfile = new TimeSeries<double>
        {
            StartYear = totalStudyCost.StartYear,
            Values = totalStudyCost.Values ?? []
        };

        var totalOpexCost = CalculateOpexCost(caseItem);
        var opexProfile = new TimeSeries<double>
        {
            StartYear = totalOpexCost.StartYear,
            Values = totalOpexCost.Values ?? []
        };

        var totalCessationCost = CalculateCessationCost(caseItem);
        var cessationProfile = new TimeSeries<double>
        {
            StartYear = totalCessationCost.StartYear,
            Values = totalCessationCost.Values ?? []
        };

        var totalOffshoreFacilityCost = CalculateTotalOffshoreFacilityCost(caseItem);
        var totalOffshoreFacilityProfile = new TimeSeries<double>
        {
            StartYear = totalOffshoreFacilityCost.StartYear,
            Values = totalOffshoreFacilityCost.Values
        };

        var totalDevelopmentCost = CalculateTotalDevelopmentCost(wellProject);
        var developmentProfile = new TimeSeries<double>
        {
            StartYear = totalDevelopmentCost.StartYear,
            Values = totalDevelopmentCost.Values
        };

        var explorationCost = CalculateTotalExplorationCost(caseItem, exploration);
        var explorationProfile = new TimeSeries<double>
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

    private static TimeSeries<double> CalculateStudyCost(Case caseItem)
    {
        TimeSeries<double> feasibilityProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies),
            caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)
        );
        TimeSeries<double> feedProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies),
            caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)
        );

        var totalOtherStudiesCostProfile = caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile);

        TimeSeries<double> otherStudies = totalOtherStudiesCostProfile != null
            ? new TimeSeriesCost(totalOtherStudiesCostProfile)
            : new TimeSeries<double> { StartYear = 0, Values = [] };

        var totalStudyCost = CostProfileMerger.MergeCostProfiles(
        [
            feasibilityProfile,
            feedProfile,
            otherStudies
        ]);
        return totalStudyCost;
    }

    private static TimeSeries<double> CalculateOpexCost(Case caseItem)
    {
        TimeSeries<double> wellInterventionProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)
        );
        TimeSeries<double> offshoreFacilitiesProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)
        );

        var historicCostProfile = caseItem.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile);
        TimeSeries<double> historicCost = historicCostProfile != null
            ? new TimeSeriesCost(historicCostProfile)
            : new TimeSeries<double> { StartYear = 0, Values = [] };

        var onshoreRelatedOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.OnshoreRelatedOPEXCostProfile);
        TimeSeries<double> onshoreRelatedOpex = onshoreRelatedOpexProfile != null
            ? new TimeSeriesCost(onshoreRelatedOpexProfile)
            : new TimeSeries<double> { StartYear = 0, Values = [] };

        var additionalOpexProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalOPEXCostProfile);
        TimeSeries<double> additionalOpex = additionalOpexProfile != null
            ? new TimeSeriesCost(additionalOpexProfile)
            : new TimeSeries<double> { StartYear = 0, Values = [] };

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

    private static TimeSeries<double> CalculateCessationCost(Case caseItem)
    {
        TimeSeries<double> cessationWellsProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost),
            caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)
        );
        TimeSeries<double> cessationOffshoreFacilitiesProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost),
            caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)
        );

        var cessationOnshoreFacilitiesProfile = caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile);
        TimeSeries<double> cessationOnshoreFacilitiesCost = cessationOnshoreFacilitiesProfile != null
            ? new TimeSeriesCost(cessationOnshoreFacilitiesProfile)
            : new TimeSeries<double> { StartYear = 0, Values = [] };

        var totalCessationCost = CostProfileMerger.MergeCostProfiles(
        [
            cessationWellsProfile,
            cessationOffshoreFacilitiesProfile,
            cessationOnshoreFacilitiesCost
        ]);
        return totalCessationCost;
    }

    private static TimeSeries<double> CalculateTotalOffshoreFacilityCost(Case caseItem)
    {
        TimeSeries<double> substructureProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride)
        );
        TimeSeries<double> surfProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride)
        );
        TimeSeries<double> topsideProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride)
        );
        TimeSeries<double> transportProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile),
            caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride)
        );
        TimeSeries<double> onshorePowerSupplyProfile = UseOverrideOrProfile(
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

    private static TimeSeries<double> CalculateTotalDevelopmentCost(WellProject wellProject)
    {
        TimeSeries<double> oilProducerProfile = UseOverrideOrProfile(
            wellProject.OilProducerCostProfile,
            wellProject.OilProducerCostProfileOverride
        );
        TimeSeries<double> gasProducerProfile = UseOverrideOrProfile(
            wellProject.GasProducerCostProfile,
            wellProject.GasProducerCostProfileOverride
        );
        TimeSeries<double> waterInjectorProfile = UseOverrideOrProfile(
            wellProject.WaterInjectorCostProfile,
            wellProject.WaterInjectorCostProfileOverride
        );
        TimeSeries<double> gasInjectorProfile = UseOverrideOrProfile(
            wellProject.GasInjectorCostProfile,
            wellProject.GasInjectorCostProfileOverride
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

    public static TimeSeries<double> CalculateTotalExplorationCost(Case caseItem, Exploration exploration)
    {
        TimeSeries<double> gAndGAdminCostProfile = UseOverrideOrProfile(
            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost),
            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)
        );
        TimeSeries<double> seismicAcquisitionAndProcessingProfile = exploration?.SeismicAcquisitionAndProcessing
             ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        TimeSeries<double> countryOfficeCostProfile = exploration?.CountryOfficeCost
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        var explorationWellTimeSeries = caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile);

        TimeSeries<double> explorationWellCostProfile = explorationWellTimeSeries != null
                                                        ? new TimeSeriesCost(explorationWellTimeSeries)
                                                        : new TimeSeries<double> { StartYear = 0, Values = [] };

        TimeSeries<double> appraisalWellCostProfile = exploration?.AppraisalWellCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        TimeSeries<double> sidetrackCostProfile = exploration?.SidetrackCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

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

    private static TimeSeries<double> UseOverrideOrProfile<T>(TimeSeries<double>? profile, T? profileOverride)
        where T : TimeSeries<double>, ITimeSeriesOverride
    {
        if (profileOverride?.Override == true)
        {
            return new TimeSeries<double>
            {
                StartYear = profileOverride.StartYear,
                Values = profileOverride.Values ?? []
            };
        }

        if (profile != null)
        {
            return new TimeSeries<double>
            {
                StartYear = profile.StartYear,
                Values = profile.Values ?? []
            };
        }

        return new TimeSeries<double> { StartYear = 0, Values = [] };
    }

    private static TimeSeries<double> UseOverrideOrProfile(TimeSeriesProfile? profile, TimeSeriesProfile? profileOverride)
    {
        if (profileOverride?.Override == true)
        {
            return new TimeSeries<double>
            {
                StartYear = profileOverride.StartYear,
                Values = profileOverride.Values
            };
        }

        if (profile != null)
        {
            return new TimeSeries<double>
            {
                StartYear = profile.StartYear,
                Values = profile.Values
            };
        }

        return new TimeSeries<double> { StartYear = 0, Values = [] };
    }
}
