using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Campaigns.Update.UpdateCampaign;

public class UpdateCampaignService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateCampaign(Guid projectId, Guid caseId, Guid campaignId, UpdateCampaignDto updateCampaignDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingCampaign = await context.Campaigns.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId && x.Id == campaignId);

        switch (updateCampaignDto.CampaignCostType)
        {
            case CampaignCostType.RigUpgrading:
                existingCampaign.RigUpgradingCostStartYear = updateCampaignDto.StartYear;
                existingCampaign.RigUpgradingCostValues = updateCampaignDto.Values;

                break;
            case CampaignCostType.RigMobDemob:
                existingCampaign.RigMobDemobCostStartYear = updateCampaignDto.StartYear;
                existingCampaign.RigMobDemobCostValues = updateCampaignDto.Values;

                break;
        }

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
