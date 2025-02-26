using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Create;

public class CreateRevisionRepository(DcdDbContext context)
{
    public async Task<Project> GetFullProjectGraph(Guid projectPk)
    {
        var project = await context.Projects
            .Include(p => p.DevelopmentOperationalWellCosts)
            .Include(p => p.ExplorationOperationalWellCosts)
            .SingleAsync(p => p.Id == projectPk);

        await context.ProjectMembers
            .Where(p => p.ProjectId == projectPk)
            .LoadAsync();

        var caseItems = await context.Cases
            .Include(x => x.DrainageStrategy)
            .Include(x => x.OnshorePowerSupply)
            .Include(x => x.Substructure)
            .Include(x => x.Surf)
            .Include(x => x.Topside)
            .Include(x => x.Transport)
            .Where(x => x.ProjectId == projectPk)
            .ToListAsync();

        var caseIds = caseItems.Select(x => x.Id).ToList();

        await context.TimeSeriesProfiles
            .Where(x => caseIds.Contains(x.Id))
            .LoadAsync();

        await context.Campaigns
            .Include(c => c.CampaignWells).ThenInclude(c => c.Well)
            .Where(x => x.Case.ProjectId == projectPk)
            .LoadAsync();

        return project;
    }
}
