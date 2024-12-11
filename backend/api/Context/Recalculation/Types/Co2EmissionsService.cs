using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation.Types;

public static class Co2EmissionsService
{
    public static bool ShouldCalculateCo2Emissions(DcdDbContext context)
    {
        var caseItemChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = context.ChangeTracker.Entries<Topside>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Topside.FuelConsumption)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareOilProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxOilProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareGasProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxGasProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareWaterInjectionProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxWaterInjectionProfile)).IsModified ||
                       e.Property(nameof(Topside.OilCapacity)).IsModified ||
                       e.Property(nameof(Topside.GasCapacity)).IsModified ||
                       e.Property(nameof(Topside.WaterInjectionCapacity)).IsModified));

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileOil.StartYear)).IsModified));

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

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileWaterInjection.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Added);

        var wellChanges = context.ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(DrillingSchedule.InternalData)).IsModified ||
                       e.Property(nameof(DrillingSchedule.StartYear)).IsModified));

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        return caseItemChanges
               || topsideChanges
               || productionProfileOilChanges
               || productionProfileOilAdded
               || additionalProductionProfileOilChanges
               || additionalProductionProfileOilAdded
               || productionProfileGasChanges
               || productionProfileGasAdded
               || additionalProductionProfileGasChanges
               || additionalProductionProfileGasAdded
               || productionProfileWaterInjectionChanges
               || productionProfileWaterInjectionAdded
               || wellChanges
               || drillingScheduleChanges
               || drillingScheduleAdded;
    }
}
