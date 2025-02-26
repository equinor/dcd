using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Duplicate;

public class DuplicateCaseRepository(DcdDbContext context)
{
    public async Task<Case> GetFullCaseGraph(Guid projectId, Guid caseId)
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

        await context.Campaigns
            .Include(c => c.ExplorationWells)
            .Where(x => x.CaseId == caseItem.Id)
            .LoadAsync();

        await context.Campaigns
            .Include(c => c.DevelopmentWells)
            .Where(x => x.CaseId == caseItem.Id)
            .LoadAsync();

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        return caseItem;
    }
}
