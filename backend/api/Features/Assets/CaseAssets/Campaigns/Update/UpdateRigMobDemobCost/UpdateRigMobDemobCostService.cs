using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateRigMobDemobCost;

public class UpdateRigMobDemobCostService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateRigMobDemobCost(Guid projectId, Guid caseId, Guid campaignId, UpdateRigMobDemobCostDto updateRigMobDemobCostDto)
    {
        var existingCampaign = await context.Campaigns.SingleAsync(x => 
            x.Case.ProjectId == projectId && x.CaseId == caseId && x.Id == campaignId);

        existingCampaign.RigMobDemobCost = updateRigMobDemobCostDto.Cost;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
} 