using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation.Types;

public static class GAndGAdminCostService
{
    public static bool ShouldCalculateGAndGAdminCost(DcdDbContext context)
    {
        var projectChanges = context.ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Project.Country)).IsModified);

        var caseChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Case.DG4Date)).IsModified ||
                       e.Property(nameof(Case.DG1Date)).IsModified));

        var wellsChanges = context.ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DrillingSchedule.InternalData)).IsModified
                      || e.Property(nameof(DrillingSchedule.StartYear)).IsModified);

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        return projectChanges || caseChanges || wellsChanges || drillingScheduleChanges || drillingScheduleAdded;
    }
}
