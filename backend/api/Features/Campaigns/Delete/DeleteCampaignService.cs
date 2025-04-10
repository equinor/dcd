using api.Context;
using api.Context.Extensions;
using api.Features.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Campaigns.Delete;

public class DeleteCampaignService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task DeleteCampaign(Guid projectId, Guid caseId, Guid campaignId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var campaign = await context.Campaigns
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Id == campaignId)
            .SingleAsync();

        await context.CampaignWells
            .Where(x => x.CampaignId == campaignId)
            .LoadAsync();

        context.CampaignWells.RemoveRange(campaign.CampaignWells);
        context.Campaigns.RemoveRange(campaign);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
