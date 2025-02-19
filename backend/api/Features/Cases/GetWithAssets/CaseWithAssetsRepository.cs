using api.Context;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.GetWithAssets;

public class CaseWithAssetsRepository(DcdDbContext context)
{
    public async Task<Case> GetCaseWithAssets(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(x => x.DrainageStrategy)
            .Include(x => x.Topside)
            .Include(x => x.Substructure)
            .Include(x => x.Surf)
            .Include(x => x.Transport)
            .Include(x => x.OnshorePowerSupply)
            .SingleOrDefaultAsync(c => c.Id == caseId);

        if (caseItem == null)
        {
            throw new NotFoundInDbException($"Case with id {caseId} not found.");
        }

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        await context.Explorations
            .Include(c => c.ExplorationWells)
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        await context.WellProjects
            .Include(c => c.DevelopmentWells)
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        await context.Campaigns
            .Include(x => x.DevelopmentWells).ThenInclude(x => x.Well)
            .Include(x => x.DevelopmentWells).ThenInclude(x => x.WellProject)
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        await context.Campaigns
            .Include(x => x.ExplorationWells).ThenInclude(x => x.Well)
            .Include(x => x.ExplorationWells).ThenInclude(x => x.Exploration)
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        return caseItem;
    }
}
