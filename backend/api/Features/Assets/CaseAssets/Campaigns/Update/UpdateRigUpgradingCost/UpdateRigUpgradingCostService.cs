using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateRigUpgradingCost;

public class UpdateRigUpgradingCostService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateRigUpgradingCost(Guid projectId, Guid caseId, Guid campaignId, UpdateRigUpgradingCostDto updateRigUpgradingCostDto)
    {
        var existingCampaign = await context.Campaigns.SingleAsync(x => 
            x.Case.ProjectId == projectId && x.CaseId == caseId && x.Id == campaignId);

        existingCampaign.RigUpgradingCost = updateRigUpgradingCostDto.Cost;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
} 