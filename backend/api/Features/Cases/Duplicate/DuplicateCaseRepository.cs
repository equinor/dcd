using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Duplicate;

public class DuplicateCaseRepository(DcdDbContext context)
{
    public async Task<Case> GetFullCaseGraph(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseItem = await context.Cases
            .Include(c => c.DrainageStrategy)
            .Include(c => c.Transport)
            .Include(c => c.Topside)
            .Include(c => c.Surf)
            .Include(c => c.Substructure)
            .Include(c => c.OnshorePowerSupply)
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleOrDefaultAsync();

        if (caseItem == null)
        {
            throw new NotFoundInDbException($"Case with id {caseId} not found.");
        }

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .LoadAsync();

        await context.Campaigns
            .Include(c => c.CampaignWells)
            .Where(x => x.CaseId == caseItem.Id)
            .LoadAsync();

        return caseItem;
    }
}
