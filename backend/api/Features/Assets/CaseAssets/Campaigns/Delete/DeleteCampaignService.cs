using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Campaigns.Delete;

public class DeleteCampaignService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task DeleteCampaign(Guid projectId, Guid caseId, Guid campaignId)
    {
        var campaign = await context.Campaigns
            .Where(x => x.Case.ProjectId == projectId)
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Id == campaignId)
            .SingleAsync();

        await context.DevelopmentWells
            .Where(x => x.CampaignId == campaignId)
            .LoadAsync();

        await context.ExplorationWell
            .Where(x => x.CampaignId == campaignId)
            .LoadAsync();

        context.ExplorationWell.RemoveRange(campaign.ExplorationWells);
        context.DevelopmentWells.RemoveRange(campaign.DevelopmentWells);
        context.Campaigns.RemoveRange(campaign);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
