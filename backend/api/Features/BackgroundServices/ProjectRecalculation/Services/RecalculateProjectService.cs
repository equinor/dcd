using System.Text.Json;

using api.Context;
using api.Features.Cases.Recalculation;
using api.Models.Infrastructure.ProjectRecalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.BackgroundServices.ProjectRecalculation.Services;

public class RecalculateProjectService(IDbContextFactory<DcdDbContext> contextFactory, IRecalculationService recalculationService)
{
    public async Task RecalculateProjects()
    {
        while (true)
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            context.ChangeTracker.LazyLoadingEnabled = false;

            var pendingProject = await context.PendingRecalculations.OrderBy(x => x.Id).FirstOrDefaultAsync();

            if (pendingProject == null)
            {
                return;
            }

            var start = DateTime.UtcNow;

            var caseIds = await context.Cases
                .Where(x => x.ProjectId == pendingProject.ProjectId)
                .Select(x => x.Id)
                .ToListAsync();

            Dictionary<Guid, Dictionary<string, long>> debugLog = [];

            foreach (var caseId in caseIds)
            {
                var timingForCase = await recalculationService.RunAllRecalculations(caseId);

                debugLog.Add(caseId, timingForCase);
            }

            context.PendingRecalculations.Remove(pendingProject);

            var end = DateTime.UtcNow;

            context.CompletedRecalculations.Add(new CompletedRecalculation
            {
                ProjectId = pendingProject.ProjectId,
                StartUtc = start,
                EndUtc = end,
                CalculationLengthInMilliseconds = (int)end.Subtract(start).TotalMilliseconds,
                DebugLog = JsonSerializer.Serialize(debugLog)
            });

            await context.SaveChangesAsync();
        }
    }
}
