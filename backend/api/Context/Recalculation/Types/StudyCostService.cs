using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation.Types;

public static class StudyCostService
{
    public static bool ShouldCalculateStudyCost(DcdDbContext context)
    {
        var caseChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(Case.CapexFactorFeasibilityStudies)).IsModified
                          || e.Property(nameof(Case.CapexFactorFEEDStudies)).IsModified
                          || e.Property(nameof(Case.DG0Date)).IsModified
                          || e.Property(nameof(Case.DG1Date)).IsModified
                          || e.Property(nameof(Case.DG2Date)).IsModified
                          || e.Property(nameof(Case.DG3Date)).IsModified
                          || e.Property(nameof(Case.DG4Date)).IsModified
                      ));

        var substructureChanges = context.ChangeTracker.Entries<SubstructureCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(SubstructureCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(SubstructureCostProfileOverride.InternalData)).IsModified
                      ));

        var substructureCostProfileAdded = context.ChangeTracker.Entries<SubstructureCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var surfChanges = context.ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(SurfCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(SurfCostProfileOverride.InternalData)).IsModified
                      ));

        var surfCostProfileAdded = context.ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var topsideChanges = context.ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TopsideCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(TopsideCostProfileOverride.InternalData)).IsModified
                      ));

        var topsideCostProfileAdded = context.ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var transportChanges = context.ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TransportCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(TransportCostProfileOverride.InternalData)).IsModified
                      ));

        var transportCostProfileAdded = context.ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var onshorePowerSupplyChanges = context.ChangeTracker.Entries<OnshorePowerSupplyCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(OnshorePowerSupplyCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(OnshorePowerSupplyCostProfileOverride.InternalData)).IsModified
                      ));

        var onshorePowerSupplyAdded = context.ChangeTracker.Entries<OnshorePowerSupplyCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectOilProducerChanges = context.ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(OilProducerCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectOilProducerAdded = context.ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectOilProducerOverrideChanges = context.ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(OilProducerCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(OilProducerCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectOilProducerOverrideAdded = context.ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasProducerChanges = context.ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(GasProducerCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectGasProducerAdded = context.ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasProducerOverrideChanges = context.ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(GasProducerCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(GasProducerCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectGasProducerOverrideAdded = context.ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectWaterInjectorChanges = context.ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(WaterInjectorCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectWaterInjectorAdded = context.ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectWaterInjectorOverrideChanges = context.ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(WaterInjectorCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(WaterInjectorCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectWaterInjectorOverrideAdded = context.ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasInjectorChanges = context.ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(GasInjectorCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectGasInjectorAdded = context.ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasInjectorOverrideChanges = context.ChangeTracker.Entries<GasInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(GasInjectorCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(GasInjectorCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectGasInjectorOverrideAdded = context.ChangeTracker.Entries<GasInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        return caseChanges
               || substructureChanges
               || substructureCostProfileAdded
               || surfChanges
               || surfCostProfileAdded
               || topsideChanges
               || topsideCostProfileAdded
               || transportChanges
               || transportCostProfileAdded
               || onshorePowerSupplyChanges
               || onshorePowerSupplyAdded
               || wellProjectOilProducerChanges
               || wellProjectOilProducerAdded
               || wellProjectOilProducerOverrideChanges
               || wellProjectOilProducerOverrideAdded
               || wellProjectGasProducerChanges
               || wellProjectGasProducerAdded
               || wellProjectGasProducerOverrideChanges
               || wellProjectGasProducerOverrideAdded
               || wellProjectWaterInjectorChanges
               || wellProjectWaterInjectorAdded
               || wellProjectWaterInjectorOverrideChanges
               || wellProjectWaterInjectorOverrideAdded
               || wellProjectGasInjectorChanges
               || wellProjectGasInjectorAdded
               || wellProjectGasInjectorOverrideChanges
               || wellProjectGasInjectorOverrideAdded;
    }
}
