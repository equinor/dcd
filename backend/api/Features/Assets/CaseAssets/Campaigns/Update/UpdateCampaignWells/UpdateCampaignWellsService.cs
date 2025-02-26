using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Models;
using api.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateCampaignWells;

public class UpdateCampaignWellsService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateCampaignWells(Guid projectId, Guid caseId, Guid campaignId, List<SaveCampaignWellDto> campaignWellDtos)
    {
        var existingCampaign = await context.Campaigns.SingleAsync(x => x.Case.ProjectId == projectId && x.CaseId == caseId && x.Id == campaignId);

        switch (existingCampaign.CampaignType)
        {
            case CampaignType.ExplorationCampaign:
                await SaveHandleExplorationCampaignWell(existingCampaign, campaignId, campaignWellDtos);
                break;
            default:
                await SaveHandleDevelopmentCampaignWell(existingCampaign, campaignId, campaignWellDtos);
                break;
        }

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }

    private async Task SaveHandleExplorationCampaignWell(Campaign existingCampaign, Guid campaignId, List<SaveCampaignWellDto> campaignWellDtos)
    {
        var wellIds = campaignWellDtos.Select(x => x.WellId).ToList();

        var existingExplorationWells = await context.ExplorationWell
            .Where(x => x.CampaignId == campaignId)
            .Where(x => wellIds.Contains(x.WellId))
            .ToListAsync();

        foreach (var campaignWellDto in campaignWellDtos)
        {
            var existingExplorationWell = existingExplorationWells.SingleOrDefault(x => x.WellId == campaignWellDto.WellId);

            if (existingExplorationWell == null)
            {
                existingCampaign.ExplorationWells.Add(new ExplorationWell
                {
                    WellId = campaignWellDto.WellId,
                    CampaignId = campaignId,
                    StartYear = campaignWellDto.StartYear,
                    Values = campaignWellDto.Values
                });

                continue;
            }

            existingExplorationWell.StartYear = campaignWellDto.StartYear;
            existingExplorationWell.Values = campaignWellDto.Values;
        }
        //todo deal with deletes
    }

    private async Task SaveHandleDevelopmentCampaignWell(Campaign existingCampaign, Guid campaignId, List<SaveCampaignWellDto> campaignWellDtos)
    {
        var wellIds = campaignWellDtos.Select(x => x.WellId).ToList();

        var existingDevelopmentWells = await context.DevelopmentWells
            .Where(x => x.CampaignId == campaignId)
            .Where(x => wellIds.Contains(x.WellId))
            .ToListAsync();

        foreach (var campaignWellDto in campaignWellDtos)
        {
            var existingDevelopmentWell = existingDevelopmentWells.SingleOrDefault(x => x.WellId == campaignWellDto.WellId);

            if (existingDevelopmentWell == null)
            {
                existingCampaign.DevelopmentWells.Add(new DevelopmentWell
                {
                    WellId = campaignWellDto.WellId,
                    CampaignId = campaignId,
                    StartYear = campaignWellDto.StartYear,
                    Values = campaignWellDto.Values
                });

                return;
            }

            existingDevelopmentWell.StartYear = campaignWellDto.StartYear;
            existingDevelopmentWell.Values = campaignWellDto.Values;
        }
        //todo deal with deletes
    }
}
