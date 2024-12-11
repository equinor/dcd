using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation.Types;

public static class TotalIncomeService
{
    public static bool ShouldCalculateTotalIncome(DcdDbContext context)
    {
        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(ProductionProfileOil.InternalData)).IsModified ||
                          e.Property(nameof(ProductionProfileOil.StartYear)).IsModified
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
                      (
                          e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                          e.Property(nameof(ProductionProfileGas.StartYear)).IsModified
                      ));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        return productionProfileGasChanges
               || productionProfileGasAdded
               || additionalProductionProfileGasChanges
               || additionalProductionProfileGasAdded
               || productionProfileOilChanges
               || productionProfileOilAdded
               || additionalProductionProfileOilChanges
               || additionalProductionProfileOilAdded;
    }
}
