using api.Context;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.GetWithAssets;

public class CaseWithAssetsRepository(DcdDbContext context)
{
    public async Task<Case> GetCaseWithAssets(Guid projectPk, Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(x => x.DrainageStrategy)
            .Include(x => x.OnshorePowerSupply)
            .Include(x => x.Substructure)
            .Include(x => x.Surf)
            .Include(x => x.Topside)
            .Include(x => x.Transport)
            .SingleOrDefaultAsync(c => c.ProjectId == projectPk && c.Id == caseId);

        if (caseItem == null)
        {
            throw new NotFoundInDbException($"Case with id {caseId} not found.");
        }

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        await context.Campaigns
            .Include(x => x.CampaignWells).ThenInclude(x => x.Well)
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        return caseItem;
    }
}
