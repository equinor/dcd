using api.Context;
using api.Features.Cases.Recalculation;
using api.Models.Infrastructure.ProjectRecalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.BackgroundServices.ProjectRecalculation.Services;

public class RecalculateProjectService(IDbContextFactory<DcdDbContext> contextFactory, RecalculationService recalculationService)
{
    public async Task RecalculateProjects()
    {
        while (true)
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var pendingProject = await context.PendingRecalculations.OrderBy(x => x.Id).FirstOrDefaultAsync();

            if (pendingProject == null)
            {
                return;
            }

            var start = DateTime.UtcNow;

            await recalculationService.SaveChangesAndRecalculateProject(pendingProject.ProjectId);

            var pendingRecalculationsWithSameProjectId = await context.PendingRecalculations
                .Where(x => x.ProjectId == pendingProject.ProjectId)
                .ToListAsync();
            
            context.PendingRecalculations.RemoveRange(pendingRecalculationsWithSameProjectId);

            var end = DateTime.UtcNow;

            context.CompletedRecalculations.Add(new CompletedRecalculation
            {
                ProjectId = pendingProject.ProjectId,
                StartUtc = start,
                EndUtc = end,
                CalculationLengthInMilliseconds = (int)end.Subtract(start).TotalMilliseconds
            });

            await context.SaveChangesAsync();
        }
    }
}
