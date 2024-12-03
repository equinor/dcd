using api.Features.Assets.CaseAssets.Explorations.Services;
using api.Features.Assets.CaseAssets.Substructures.Services;
using api.Features.Assets.CaseAssets.Surfs.Services;
using api.Features.Assets.CaseAssets.Topsides.Services;
using api.Features.Assets.CaseAssets.Transports.Services;
using api.Features.Assets.CaseAssets.WellProjects.Services;
using api.Models;

namespace api.Services.EconomicsServices;

public class CalculateTotalCostService(
    ICaseService caseService,
    ISubstructureService substructureService,
    ISurfService surfService,
    ITopsideService topsideService,
    ITransportService transportService,
    IWellProjectService wellProjectService,
    IExplorationService explorationService)
    : ICalculateTotalCostService
{
    public async Task CalculateTotalCost(Guid caseId)
    {
        var caseItem = await caseService.GetCaseWithIncludes(
            caseId,
            c => c.TotalFeasibilityAndConceptStudies!,
            c => c.TotalFeasibilityAndConceptStudiesOverride!,
            c => c.TotalFEEDStudies!,
            c => c.TotalFEEDStudiesOverride!,
            c => c.TotalOtherStudiesCostProfile!,
            c => c.HistoricCostCostProfile!,
            c => c.WellInterventionCostProfile!,
            c => c.WellInterventionCostProfileOverride!,
            c => c.OffshoreFacilitiesOperationsCostProfile!,
            c => c.OffshoreFacilitiesOperationsCostProfileOverride!,
            c => c.OnshoreRelatedOPEXCostProfile!,
            c => c.AdditionalOPEXCostProfile!,
            c => c.CessationWellsCost!,
            c => c.CessationWellsCostOverride!,
            c => c.CessationOffshoreFacilitiesCost!,
            c => c.CessationOffshoreFacilitiesCostOverride!,
            c => c.CessationOnshoreFacilitiesCostProfile!
        );

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

        var substructure = await substructureService.GetSubstructureWithIncludes(
            caseItem.SubstructureLink,
            s => s.CostProfileOverride!,
            s => s.CostProfile!
        );

        var surf = await surfService.GetSurfWithIncludes(
            caseItem.SurfLink,
            s => s.CostProfileOverride!,
            s => s.CostProfile!
        );

        var topside = await topsideService.GetTopsideWithIncludes(
            caseItem.TopsideLink,
            t => t.CostProfileOverride!,
            t => t.CostProfile!
        );

        var transport = await transportService.GetTransportWithIncludes(
            caseItem.TransportLink,
            t => t.CostProfileOverride!,
            t => t.CostProfile!
        );

        var totalOffshoreFacilityCost = CalculateTotalOffshoreFacilityCost(substructure, surf, topside, transport);
        var totalOffshoreFacilityProfile = new TimeSeries<double>
        {
            StartYear = totalOffshoreFacilityCost.StartYear,
            Values = totalOffshoreFacilityCost.Values
        };

        var wellProject = await wellProjectService.GetWellProjectWithIncludes(
            caseItem.WellProjectLink,
            w => w.OilProducerCostProfileOverride!,
            w => w.OilProducerCostProfile!,
            w => w.GasProducerCostProfileOverride!,
            w => w.GasProducerCostProfile!,
            w => w.WaterInjectorCostProfileOverride!,
            w => w.WaterInjectorCostProfile!,
            w => w.GasInjectorCostProfileOverride!,
            w => w.GasInjectorCostProfile!
        );

        var totalDevelopmentCost = CalculateTotalDevelopmentCost(wellProject);
        var developmentProfile = new TimeSeries<double>
        {
            StartYear = totalDevelopmentCost.StartYear,
            Values = totalDevelopmentCost.Values
        };

        var exploration = await explorationService.GetExplorationWithIncludes(
            caseItem.ExplorationLink,
            e => e.GAndGAdminCost!,
            e => e.CountryOfficeCost!,
            e => e.SeismicAcquisitionAndProcessing!,
            e => e.ExplorationWellCostProfile!,
            e => e.AppraisalWellCostProfile!,
            e => e.SidetrackCostProfile!);

        var explorationCost = CalculateTotalExplorationCost(exploration);
        var explorationProfile = new TimeSeries<double>
        {
            StartYear = explorationCost.StartYear,
            Values = explorationCost.Values
        };

        var totalCost = TimeSeriesCost.MergeCostProfilesList(
        [
            studiesProfile,
            opexProfile,
            cessationProfile,
            totalOffshoreFacilityProfile,
            developmentProfile,
            explorationProfile
        ]);

        if (caseItem.CalculatedTotalCostCostProfile != null)
        {
            caseItem.CalculatedTotalCostCostProfile.Values = totalCost.Values;
            caseItem.CalculatedTotalCostCostProfile.StartYear = totalCost.StartYear;
        }
        else
        {
            caseItem.CalculatedTotalCostCostProfile = new CalculatedTotalCostCostProfile
            {
                Values = totalCost.Values,
                StartYear = totalCost.StartYear
            };
        }

        return;
    }

    public TimeSeries<double> CalculateStudyCost(Case caseItem)
    {
        TimeSeries<double> feasibilityProfile = UseOverrideOrProfile(
            caseItem.TotalFeasibilityAndConceptStudies,
            caseItem.TotalFeasibilityAndConceptStudiesOverride
        );
        TimeSeries<double> feedProfile = UseOverrideOrProfile(
            caseItem.TotalFEEDStudies,
            caseItem.TotalFEEDStudiesOverride
        );

        TimeSeries<double> otherStudiesProfile = caseItem.TotalOtherStudiesCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        var totalStudyCost = TimeSeriesCost.MergeCostProfilesList(
        [
            feasibilityProfile,
            feedProfile,
            otherStudiesProfile
        ]);
        return totalStudyCost;
    }

    public TimeSeries<double> CalculateOpexCost(Case caseItem)
    {
        TimeSeries<double> wellInterventionProfile = UseOverrideOrProfile(
            caseItem.WellInterventionCostProfile,
            caseItem.WellInterventionCostProfileOverride
        );
        TimeSeries<double> offshoreFacilitiesProfile = UseOverrideOrProfile(
            caseItem.OffshoreFacilitiesOperationsCostProfile,
            caseItem.OffshoreFacilitiesOperationsCostProfileOverride
        );

        TimeSeries<double> historicCostProfile = caseItem.HistoricCostCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };
        TimeSeries<double> onshoreRelatedOpexProfile = caseItem.OnshoreRelatedOPEXCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };
        TimeSeries<double> additionalOpexProfile = caseItem.AdditionalOPEXCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        var totalOpexCost = TimeSeriesCost.MergeCostProfilesList(
        [
            historicCostProfile,
            wellInterventionProfile,
            offshoreFacilitiesProfile,
            onshoreRelatedOpexProfile,
            additionalOpexProfile
        ]);
        return totalOpexCost;
    }

    public TimeSeries<double> CalculateCessationCost(Case caseItem)
    {
        TimeSeries<double> cessationWellsProfile = UseOverrideOrProfile(
            caseItem.CessationWellsCost,
            caseItem.CessationWellsCostOverride
        );
        TimeSeries<double> cessationOffshoreFacilitiesProfile = UseOverrideOrProfile(
            caseItem.CessationOffshoreFacilitiesCost,
            caseItem.CessationOffshoreFacilitiesCostOverride
        );
        TimeSeries<double> cessationOnshoreFacilitiesProfile = caseItem.CessationOnshoreFacilitiesCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        var totalCessationCost = TimeSeriesCost.MergeCostProfilesList(
        [
            cessationWellsProfile,
            cessationOffshoreFacilitiesProfile,
            cessationOnshoreFacilitiesProfile
        ]);
        return totalCessationCost;
    }

    public static TimeSeries<double> CalculateTotalOffshoreFacilityCost(
        Substructure? substructure,
        Surf? surf,
        Topside? topside,
        Transport? transport)
    {
        TimeSeries<double> substructureProfile = UseOverrideOrProfile(
            substructure?.CostProfile,
            substructure?.CostProfileOverride
        );
        TimeSeries<double> surfProfile = UseOverrideOrProfile(
            surf?.CostProfile,
            surf?.CostProfileOverride
        );
        TimeSeries<double> topsideProfile = UseOverrideOrProfile(
            topside?.CostProfile,
            topside?.CostProfileOverride
        );
        TimeSeries<double> transportProfile = UseOverrideOrProfile(
            transport?.CostProfile,
            transport?.CostProfileOverride
        );

        var totalOffshoreFacilityCost = TimeSeriesCost.MergeCostProfilesList(
        [
            substructureProfile,
            surfProfile,
            topsideProfile,
            transportProfile
        ]);

        return totalOffshoreFacilityCost;
    }


    public static TimeSeries<double> CalculateTotalDevelopmentCost(WellProject wellProject)
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

        var totalDevelopmentCost = TimeSeriesCost.MergeCostProfilesList(
        [
            oilProducerProfile,
            gasProducerProfile,
            waterInjectorProfile,
            gasInjectorProfile
        ]);

        return totalDevelopmentCost;
    }

    public static TimeSeries<double> CalculateTotalExplorationCost(Exploration exploration)
    {
        TimeSeries<double> gAndGAdminCostProfile = UseOverrideOrProfile(
            exploration.GAndGAdminCost,
            exploration.GAndGAdminCostOverride
        );
        TimeSeries<double> seismicAcquisitionAndProcessingProfile = exploration?.SeismicAcquisitionAndProcessing
             ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        TimeSeries<double> countryOfficeCostProfile = exploration?.CountryOfficeCost
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        TimeSeries<double> explorationWellCostProfile = exploration?.ExplorationWellCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        TimeSeries<double> appraisalWellCostProfile = exploration?.AppraisalWellCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        TimeSeries<double> sidetrackCostProfile = exploration?.SidetrackCostProfile
            ?? new TimeSeries<double> { StartYear = 0, Values = [] };

        var totalexploration = TimeSeriesCost.MergeCostProfilesList(
        [
            gAndGAdminCostProfile,
            seismicAcquisitionAndProcessingProfile,
            countryOfficeCostProfile,
            explorationWellCostProfile,
            appraisalWellCostProfile,
            sidetrackCostProfile
        ]);

        return totalexploration;
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
        else if (profile != null)
        {
            return new TimeSeries<double>
            {
                StartYear = profile.StartYear,
                Values = profile.Values ?? []
            };
        }
        return new TimeSeries<double> { StartYear = 0, Values = [] };
    }
}

