using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation.Types;

public static class TotalCostService
{
    public static bool ShouldCalculateTotalCost(DcdDbContext context)
    {

        var totalFeasibilityAdded = context.ChangeTracker.Entries<TotalFeasibilityAndConceptStudies>()
            .Any(e => e.State == EntityState.Added);

        var totalFeasibilityOverrideChanges = context.ChangeTracker.Entries<TotalFeasibilityAndConceptStudiesOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TotalFeasibilityAndConceptStudiesOverride.Override)).IsModified
                          || e.Property(nameof(TotalFeasibilityAndConceptStudiesOverride.InternalData)).IsModified
                      ));


        var totalFEEDChanges = context.ChangeTracker.Entries<TotalFEEDStudies>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var totalFEEDOverrideChanges = context.ChangeTracker.Entries<TotalFEEDStudiesOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var totalOtherStudiesChanges = context.ChangeTracker.Entries<TotalOtherStudiesCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var historicCostChanges = context.ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var wellInterventionChanges = context.ChangeTracker.Entries<WellInterventionCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var wellInterventionOverrideChanges = context.ChangeTracker.Entries<WellInterventionCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var offshoreFacilitiesOperationsChanges = context.ChangeTracker.Entries<OffshoreFacilitiesOperationsCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var offshoreFacilitiesOperationsOverrideChanges = context.ChangeTracker.Entries<OffshoreFacilitiesOperationsCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshoreRelatedOPEXChanges = context.ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var additionalOPEXChanges = context.ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationWellsChanges = context.ChangeTracker.Entries<CessationWellsCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationWellsOverrideChanges = context.ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOffshoreFacilitiesChanges = context.ChangeTracker.Entries<CessationOffshoreFacilitiesCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOffshoreFacilitiesOverrideChanges = context.ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOnshoreFacilitiesChanges = context.ChangeTracker.Entries<CessationOnshoreFacilitiesCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var surfChanges = context.ChangeTracker.Entries<SurfCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var surfOverrideChanges = context.ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var substructureChanges = context.ChangeTracker.Entries<SubstructureCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var substructureOverrideChanges = context.ChangeTracker.Entries<SubstructureCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var topsideChanges = context.ChangeTracker.Entries<TopsideCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var topsideOverrideChanges = context.ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var transportChanges = context.ChangeTracker.Entries<TransportCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var transportOverrideChanges = context.ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshorePowerSupplyChanges = context.ChangeTracker.Entries<OnshorePowerSupplyCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshorePowerSupplyOverrideChanges = context.ChangeTracker.Entries<OnshorePowerSupplyCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var oilProducerChanges = context.ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var oilProducerOverrideChanges = context.ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasProducerChanges = context.ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasProducerOverrideChanges = context.ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var waterInjectorChanges = context.ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var waterInjectorOverrideChanges = context.ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasInjectorChanges = context.ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasInjectorOverrideChanges = context.ChangeTracker.Entries<GasInjectorCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gAndGAdminChanges = context.ChangeTracker.Entries<GAndGAdminCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gAndGAdminOverrideChanges = context.ChangeTracker.Entries<GAndGAdminCostOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var seismicChanges = context.ChangeTracker.Entries<SeismicAcquisitionAndProcessing>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var countryOfficeChanges = context.ChangeTracker.Entries<CountryOfficeCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var explorationWellChanges = context.ChangeTracker.Entries<ExplorationWellCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var appraisalWellChanges = context.ChangeTracker.Entries<AppraisalWellCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var sidetrackChanges = context.ChangeTracker.Entries<SidetrackCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        return
            totalFeasibilityAdded
            || totalFeasibilityOverrideChanges
            || totalFEEDChanges
            || totalFEEDOverrideChanges
            || totalOtherStudiesChanges
            || historicCostChanges
            || wellInterventionChanges
            || wellInterventionOverrideChanges
            || offshoreFacilitiesOperationsChanges
            || offshoreFacilitiesOperationsOverrideChanges
            || onshoreRelatedOPEXChanges
            || additionalOPEXChanges
            || cessationWellsChanges
            || cessationWellsOverrideChanges
            || cessationOffshoreFacilitiesChanges
            || cessationOffshoreFacilitiesOverrideChanges
            || cessationOnshoreFacilitiesChanges
            || surfChanges
            || surfOverrideChanges
            || substructureChanges
            || substructureOverrideChanges
            || topsideChanges
            || topsideOverrideChanges
            || transportChanges
            || transportOverrideChanges
            || onshorePowerSupplyChanges
            || onshorePowerSupplyOverrideChanges
            || oilProducerChanges
            || oilProducerOverrideChanges
            || gasProducerChanges
            || gasProducerOverrideChanges
            || waterInjectorChanges
            || waterInjectorOverrideChanges
            || gasInjectorChanges
            || gasInjectorOverrideChanges
            || gAndGAdminChanges
            || gAndGAdminOverrideChanges
            || seismicChanges
            || countryOfficeChanges
            || explorationWellChanges
            || appraisalWellChanges
            || sidetrackChanges;
    }
}
