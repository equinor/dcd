using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Campaigns.Update.UpdateRigMobDemobCost;

public class UpdateRigMobDemobCostService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateRigMobDemobCost(Guid projectId, Guid caseId, Guid campaignId, UpdateRigMobDemobCostDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingCampaign = await context.Campaigns.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId && x.Id == campaignId);

        existingCampaign.RigMobDemobCost = dto.Cost;

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
