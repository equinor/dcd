using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Create;

public class CreateRevisionRepository(DcdDbContext context)
{
    public async Task<Project> GetDetachedProjectGraph(Guid projectPk)
    {
        var project = await context.Projects
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == projectPk);

        var caseItems = await context.Cases
            .Include(x => x.DrainageStrategy)
            .Include(x => x.Transport)
            .Include(x => x.Topside)
            .Include(x => x.Surf)
            .Include(x => x.Substructure)
            .Include(x => x.OnshorePowerSupply)
            .Include(x => x.DrainageStrategy)
            .Include(x => x.DrainageStrategy)
            .Where(x => x.ProjectId == projectPk)
            .ToListAsync();

        var caseIds = caseItems.Select(x => x.Id).ToList();

        await context.TimeSeriesProfiles
            .Where(x => caseIds.Contains(x.Id))
            .LoadAsync();

        await context.Explorations
            .Include(c => c.ExplorationWells).ThenInclude(c => c.Well)
            .Include(c => c.ExplorationWells).ThenInclude(c => c.DrillingSchedule)
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();

        await context.WellProjects
            .Include(c => c.WellProjectWells).ThenInclude(c => c.Well)
            .Include(c => c.WellProjectWells).ThenInclude(c => c.DrillingSchedule)
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();

        DetachEntriesToEnablePrimaryKeyEdits();

        return project;
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
