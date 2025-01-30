using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Duplicate;

public class DuplicateCaseRepository(DcdDbContext context)
{
    public async Task<Case> GetDetachedCaseGraph(Guid projectId, Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(c => c.DrainageStrategy)
            .Include(c => c.Transport)
            .Include(c => c.Topside)
            .Include(c => c.Surf)
            .Include(c => c.Substructure)
            .Include(c => c.OnshorePowerSupply)
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        await context.Explorations
            .Include(c => c.ExplorationWells).ThenInclude(c => c.Well)
            .Include(c => c.ExplorationWells).ThenInclude(c => c.DrillingSchedule)
            .Where(x => x.Id == caseItem.ExplorationLink)
            .LoadAsync();

        await context.WellProjects
            .Include(c => c.WellProjectWells).ThenInclude(c => c.Well)
            .Include(c => c.WellProjectWells).ThenInclude(c => c.DrillingSchedule)
            .Where(x => x.Id == caseItem.WellProjectLink)
            .LoadAsync();

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        DetachEntriesToEnablePrimaryKeyEdits();

        return caseItem;
    }

    private void DetachEntriesToEnablePrimaryKeyEdits()
    {
        var entries = context.ChangeTracker
            .Entries()
            .Where(e => e.State != EntityState.Detached)
            .ToList();

        foreach (var entry in entries)
        {
            entry.State = EntityState.Detached;
        }
    }
}
