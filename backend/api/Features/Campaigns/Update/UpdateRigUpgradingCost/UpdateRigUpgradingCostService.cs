using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Campaigns.Update.UpdateRigUpgradingCost;

public class UpdateRigUpgradingCostService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateRigUpgradingCost(Guid projectId, Guid caseId, Guid campaignId, UpdateRigUpgradingCostDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingCampaign = await context.Campaigns.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId && x.Id == campaignId);

        existingCampaign.RigUpgradingCost = dto.Cost;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
