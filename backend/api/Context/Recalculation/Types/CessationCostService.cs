using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation.Types;

public static class CessationCostService
{
    public static bool ShouldCalculateCessationCostProfile(DcdDbContext context)
    {
        var caseCessationWellsCostOverrideChanges = context.ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(CessationWellsCostOverride.InternalData)).IsModified
                          || e.Property(nameof(CessationWellsCostOverride.Override)).IsModified
                      ));

        var cessationWellsCostAdded = context.ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State == EntityState.Added);

        var caseCessationOffshoreFacilitiesCostOverrideChanges = context.ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(CessationOffshoreFacilitiesCostOverride.InternalData)).IsModified
                          || e.Property(nameof(CessationOffshoreFacilitiesCostOverride.Override)).IsModified
                      ));

        var cessationOffshoreFacilitiesCostAdded = context.ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(ProductionProfileOil.InternalData)).IsModified
                          || e.Property(nameof(ProductionProfileOil.StartYear)).IsModified
                      ));

        var productionProfileOilAdded = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var surfChanges = context.ChangeTracker.Entries<Surf>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(Surf.CessationCost)).IsModified
                      ));

        var developmentOperationalWellCostsChanges = context.ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(DevelopmentOperationalWellCosts.PluggingAndAbandonment)).IsModified
                      ));
        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(DrillingSchedule.InternalData)).IsModified
                      ));

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        return caseCessationWellsCostOverrideChanges
               || cessationWellsCostAdded
               || caseCessationOffshoreFacilitiesCostOverrideChanges
               || cessationOffshoreFacilitiesCostAdded
               || productionProfileOilChanges
               || productionProfileOilAdded
               || additionalProductionProfileOilChanges
               || additionalProductionProfileOilAdded
               || productionProfileGasChanges
               || productionProfileGasAdded
               || additionalProductionProfileGasChanges
               || additionalProductionProfileGasAdded
               || surfChanges
               || developmentOperationalWellCostsChanges
               || drillingScheduleChanges
               || drillingScheduleAdded;
    }
}
