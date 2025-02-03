using api.Context;
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
            .SingleAsync(c => c.Id == caseId);

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        await context.Explorations
            .Include(c => c.ExplorationWells).ThenInclude(c => c.DrillingSchedule)
            .Where(x => x.Id == caseItem.ExplorationId)
            .LoadAsync();

        await context.WellProjects
            .Include(c => c.DevelopmentWells).ThenInclude(c => c.DrillingSchedule)
            .Where(x => x.Id == caseItem.WellProjectId)
            .LoadAsync();

        return caseItem;
    }
}
