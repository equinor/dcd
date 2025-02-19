using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.CampaignWells.Save;
using api.Features.Cases.Recalculation;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Campaigns.Update;

public class UpdateCampaignService(DcdDbContext context,
    SaveCampaignWellService saveCampaignWellService,
    RecalculationService recalculationService)
{
    public async Task UpdateCampaign(
        Guid projectId,
        Guid caseId,
        Guid campaignId,
        UpdateCampaignDto updateCampaignDto)
    {
        var existingCampaign = await context.Campaigns.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId && x.Id == campaignId);

        existingCampaign.RigUpgradingCost = updateCampaignDto.RigUpgradingCost;
        existingCampaign.RigUpgradingCostStartYear = updateCampaignDto.RigUpgradingCostStartYear;
        existingCampaign.RigUpgradingCostValues = updateCampaignDto.RigUpgradingCostValues;

        existingCampaign.RigMobDemobCost = updateCampaignDto.RigMobDemobCost;
        existingCampaign.RigMobDemobCostStartYear = updateCampaignDto.RigMobDemobCostStartYear;
        existingCampaign.RigMobDemobCostValues = updateCampaignDto.RigMobDemobCostValues;

        foreach (var well in updateCampaignDto.CampaignWells)
        {
            await saveCampaignWellService.SaveCampaignWell(projectId, caseId, campaignId, well.WellId, well);
        }

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
