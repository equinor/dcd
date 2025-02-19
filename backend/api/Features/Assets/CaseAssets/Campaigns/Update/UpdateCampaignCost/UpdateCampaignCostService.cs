using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateCampaignCost;

public class UpdateCampaignCostService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateCampaignCost(Guid projectId, Guid caseId, Guid campaignId, UpdateCampaignCostDto updateCampaignCostDto)
    {
        var existingCampaign = await context.Campaigns.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId && x.Id == campaignId);

        existingCampaign.RigUpgradingCost = updateCampaignCostDto.RigUpgradingCost;
        existingCampaign.RigMobDemobCost = updateCampaignCostDto.RigMobDemobCost;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
