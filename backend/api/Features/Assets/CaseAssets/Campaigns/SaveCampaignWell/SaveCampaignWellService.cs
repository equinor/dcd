using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Campaigns.SaveCampaignWell;

public class SaveCampaignWellService(DcdDbContext context)
{
    public async Task SaveCampaignWell(Guid projectId, Guid caseId, Guid campaignId, Guid wellId, SaveCampaignWellDto dto)
    {
        var existingCampaign = await context.Campaigns.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId && x.Id == campaignId);

        var caseItem = await context.Cases.SingleAsync(x => x.Id == caseId);

        switch (existingCampaign.CampaignType)
        {
            case CampaignTypes.ExplorationCampaign:
                await SaveHandleExplorationCampaignWell(existingCampaign, campaignId, caseItem.ExplorationId, wellId, dto);
                break;
            default:
                await SaveHandleDevelopmentCampaignWell(existingCampaign, campaignId, caseItem.WellProjectId, wellId, dto);
                break;
        }

        await context.SaveChangesAsync();
    }

    private async Task SaveHandleExplorationCampaignWell(Campaign existingCampaign,
        Guid campaignId,
        Guid explorationId,
        Guid wellId,
        SaveCampaignWellDto dto)
    {
        var existingExplorationWell = await context.ExplorationWell
            .Where(x => x.CampaignId == campaignId)
            .Where(x => x.WellId == wellId)
            .SingleOrDefaultAsync();

        if (existingExplorationWell == null)
        {
            existingCampaign.ExplorationWells.Add(new ExplorationWell
            {
                WellId = wellId,
                ExplorationId = explorationId,
                CampaignId = campaignId,
                StartYear = dto.StartYear,
                Values = dto.Values
            });

            return;
        }

        existingExplorationWell.StartYear = dto.StartYear;
        existingExplorationWell.Values = dto.Values;
    }

    private async Task SaveHandleDevelopmentCampaignWell(Campaign existingCampaign,
        Guid campaignId,
        Guid wellProjectId,
        Guid wellId,
        SaveCampaignWellDto dto)
    {
        var existingDevelopmentWell = await context.DevelopmentWells
            .Where(x => x.CampaignId == campaignId)
            .Where(x => x.WellId == wellId)
            .SingleOrDefaultAsync();

        if (existingDevelopmentWell == null)
        {
            existingCampaign.DevelopmentWells.Add(new DevelopmentWell
            {
                WellId = wellId,
                WellProjectId = wellProjectId,
                CampaignId = campaignId,
                StartYear = dto.StartYear,
                Values = dto.Values
            });

            return;
        }

        existingDevelopmentWell.StartYear = dto.StartYear;
        existingDevelopmentWell.Values = dto.Values;
    }
}
