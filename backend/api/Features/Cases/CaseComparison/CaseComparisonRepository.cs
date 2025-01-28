using api.Context;
using api.Context.Extensions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.CaseComparison;

public class CaseComparisonRepository(DcdDbContext context)
{
    public async Task<Guid> GetPrimaryKeyForProjectIdOrRevisionId(Guid projectId)
    {
        return await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);
    }

    public async Task<Project> LoadProject(Guid projectPk)
    {
        var project = await context.Projects
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == projectPk);

        var caseIds = project.Cases.Where(x => !x.Archived).Select(c => c.Id).ToList();
        await context.Cases
            .Include(c => c.TimeSeriesProfiles)
            .Where(x => caseIds.Contains(x.Id))
            .LoadAsync();

        var drainageStrategyLinks = project.Cases.Select(x => x.DrainageStrategyLink).ToList();
        await context.DrainageStrategies
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .Include(c => c.Co2Intensity)
            .Include(d => d.ProductionProfileWater)
            .Include(d => d.ProductionProfileWaterInjection)
            .Include(d => d.ProductionProfileNgl)
            .Include(d => d.DeferredOilProduction)
            .Include(d => d.DeferredGasProduction)
            .Where(x => drainageStrategyLinks.Contains(x.Id))
            .LoadAsync();

        var explorationLinks = project.Cases.Select(x => x.ExplorationLink).ToList();
        await context.Explorations
            .Where(x => explorationLinks.Contains(x.Id))
            .LoadAsync();

        var wellProjectLinks = project.Cases.Select(x => x.WellProjectLink).ToList();
        await context.WellProjects
            .Where(x => wellProjectLinks.Contains(x.Id))
            .LoadAsync();

        var substructureLinks = project.Cases.Select(x => x.SubstructureLink).ToList();
        await context.Substructures
            .Where(x => substructureLinks.Contains(x.Id))
            .LoadAsync();

        var surfLinkIds = project.Cases.Select(x => x.SurfLink).ToList();
        await context.Surfs
            .Where(x => surfLinkIds.Contains(x.Id))
            .LoadAsync();

        var topsideLinks = project.Cases.Select(x => x.TopsideLink).ToList();
        await context.Topsides
            .Where(x => topsideLinks.Contains(x.Id))
            .LoadAsync();

        var transportLinks = project.Cases.Select(x => x.TransportLink).ToList();
        await context.Transports
            .Where(x => transportLinks.Contains(x.Id))
            .LoadAsync();

        var onshorePowerSupplyLinks = project.Cases.Select(x => x.OnshorePowerSupplyLink).ToList();
        await context.OnshorePowerSupplies
            .Where(x => onshorePowerSupplyLinks.Contains(x.Id))
            .LoadAsync();

        return project;
    }
}
