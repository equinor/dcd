using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation.Types;

public static class ExplorationAndWellProjectCostService
{
    public static (List<Well> wells, List<Guid> drillingScheduleIds) GetExplorationAndWellProjectCostToRecalculate(DcdDbContext context)
    {
        var modifiedWellsWithCostChange = context.ChangeTracker.Entries<Well>()
            .Where(e => (e.State == EntityState.Modified)
                        && (e.Property(nameof(Well.WellCost)).IsModified || e.Property(nameof(Well.WellCategory)).IsModified));

        var modifiedWellIds = modifiedWellsWithCostChange.Select(e => e.Entity).ToList();

        var modifiedDrillingSchedules = context.ChangeTracker.Entries<DrillingSchedule>()
            .Where(e => (e.State == EntityState.Modified)
                        && (e.Property(nameof(DrillingSchedule.InternalData)).IsModified
                            || e.Property(nameof(DrillingSchedule.StartYear)).IsModified));

        var addedDrillingSchedules = context.ChangeTracker.Entries<DrillingSchedule>()
            .Where(e => e.State == EntityState.Added);

        var modifiedDrillingScheduleIds = modifiedDrillingSchedules.Select(e => e.Entity.Id)
            .Union(addedDrillingSchedules.Select(e => e.Entity.Id)).ToList();

        return (modifiedWellIds, modifiedDrillingScheduleIds);
    }
}
