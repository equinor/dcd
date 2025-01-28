using api.Context;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Duplicate;

public class DuplicateCaseRepository(DcdDbContext context)
{
    public async Task<Case> GetDetachedCaseGraph(Guid projectId, Guid caseId)
    {
        var caseItem = await LoadCase(projectId, caseId);

        await LoadDrainageStrategies(caseItem.DrainageStrategyLink);
        await LoadExplorations(caseItem.ExplorationLink);
        await LoadWellProjects(caseItem.WellProjectLink);
        await LoadTransports(caseItem.TransportLink);
        await LoadTopsides(caseItem.TopsideLink);
        await LoadSurfs(caseItem.SurfLink);
        await LoadSubstructures(caseItem.SubstructureLink);
        await LoadOnshorePowerSupplies(caseItem.OnshorePowerSupplyLink);

        DetachEntriesToEnablePrimaryKeyEdits();

        return caseItem;
    }

    private async Task<Case> LoadCase(Guid projectId, Guid caseId)
    {
        return await context.Cases
            .Include(c => c.TimeSeriesProfiles)
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == caseId)
            .SingleOrDefaultAsync() ?? throw new NotFoundInDbException($"Case {caseId} not found.");
    }

    private async Task LoadDrainageStrategies(Guid drainageStrategyLink)
    {
        await context.DrainageStrategies
            .Include(c => c.ProductionProfileWaterInjection)
            .Include(c => c.Co2Intensity)
            .Include(c => c.ProductionProfileNgl)
            .Include(c => c.DeferredOilProduction)
            .Include(c => c.DeferredGasProduction)
            .Where(x => x.Id == drainageStrategyLink)
            .LoadAsync();
    }

    private async Task LoadExplorations(Guid explorationLink)
    {
        await context.Explorations
            .Include(c => c.ExplorationWells).ThenInclude(c => c.DrillingSchedule)
            .Include(c => c.ExplorationWells).ThenInclude(c => c.Well)
            .Where(x => x.Id == explorationLink)
            .LoadAsync();
    }

    private async Task LoadWellProjects(Guid wellProjectLink)
    {
        await context.WellProjects
            .Include(c => c.WellProjectWells).ThenInclude(c => c.Well)
            .Include(c => c.WellProjectWells).ThenInclude(c => c.DrillingSchedule)
            .Where(x => x.Id == wellProjectLink)
            .LoadAsync();
    }

    private async Task LoadTransports(Guid transportLink)
    {
        await context.Transports
            .Where(x => x.Id == transportLink)
            .LoadAsync();
    }

    private async Task LoadTopsides(Guid topsideLink)
    {
        await context.Topsides
            .Where(x => x.Id == topsideLink)
            .LoadAsync();
    }

    private async Task LoadSurfs(Guid surfLink)
    {
        await context.Surfs
            .Where(x => x.Id == surfLink)
            .LoadAsync();
    }

    private async Task LoadSubstructures(Guid substructureLink)
    {
        await context.Substructures
            .Where(x => x.Id == substructureLink)
            .LoadAsync();
    }

    private async Task LoadOnshorePowerSupplies(Guid onshorePowerSupplyLink)
    {
        await context.OnshorePowerSupplies
            .Where(x => x.Id == onshorePowerSupplyLink)
            .LoadAsync();
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
