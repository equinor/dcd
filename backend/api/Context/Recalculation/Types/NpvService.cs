using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation.Types;

public static class NpvService
{
    public static bool ShouldCalculateNpv(DcdDbContext context)
    {
        var projectChanges = context.ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Project.DiscountRate)).IsModified ||
                       e.Property(nameof(Project.ExchangeRateUSDToNOK)).IsModified ||
                       e.Property(nameof(Project.OilPriceUSD)).IsModified ||
                       e.Property(nameof(Project.GasPriceNOK)).IsModified));

        var caseChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.DG4Date)).IsModified);

        return projectChanges || caseChanges;
    }
}
