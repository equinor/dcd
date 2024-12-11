using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation.Types;

public static class OpexService
{
    public static bool ShouldCalculateOpex(DcdDbContext context)
    {
        var historicCostChanges = context.ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(HistoricCostCostProfile.StartYear)).IsModified
                      || e.Property(nameof(HistoricCostCostProfile.InternalData)).IsModified);

        var historicCostAdded = context.ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var onshoreOpexChanges = context.ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(OnshoreRelatedOPEXCostProfile.StartYear)).IsModified
                      || e.Property(nameof(OnshoreRelatedOPEXCostProfile.InternalData)).IsModified);

        var onshoreOpexAdded = context.ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var additionalOpexChanges = context.ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(AdditionalOPEXCostProfile.StartYear)).IsModified
                      || e.Property(nameof(AdditionalOPEXCostProfile.InternalData)).IsModified);

        var additionalOpexAdded = context.ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var developmentOperationalWellCostsChanges = context.ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell)).IsModified);

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileOil.StartYear)).IsModified ||
                       e.Property(nameof(ProductionProfileOil.InternalData)).IsModified));

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

        var wellsChanges = context.ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(DrillingSchedule.InternalData)).IsModified ||
                       e.Property(nameof(DrillingSchedule.StartYear)).IsModified));

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        var topsideOpexChanges = context.ChangeTracker.Entries<Topside>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Topside.FacilityOpex)).IsModified);

        return historicCostChanges
               || historicCostAdded
               || onshoreOpexChanges
               || onshoreOpexAdded
               || additionalOpexChanges
               || additionalOpexAdded
               || developmentOperationalWellCostsChanges
               || productionProfileOilChanges
               || additionalProductionProfileOilChanges
               || productionProfileOilAdded
               || additionalProductionProfileOilAdded
               || productionProfileGasChanges
               || productionProfileGasAdded
               || additionalProductionProfileGasChanges
               || additionalProductionProfileGasAdded
               || wellsChanges
               || drillingScheduleChanges
               || drillingScheduleAdded
               || topsideOpexChanges;
    }
}
