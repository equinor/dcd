using api.Context;
using api.Context.Extensions;
using api.Features.Recalculation;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Campaigns.Update.UpdateCampaignWells;

public class UpdateCampaignWellsService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task UpdateCampaignWells(Guid projectId, Guid caseId, Guid campaignId, List<SaveCampaignWellDto> campaignWellDtos)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var existingCampaign = await context.Campaigns.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId && x.Id == campaignId);

        var wellIds = campaignWellDtos.Select(x => x.WellId).ToList();

        var existingCampaignWells = await context.CampaignWells
            .Where(x => x.CampaignId == campaignId)
            .Where(x => wellIds.Contains(x.WellId))
            .ToListAsync();

        foreach (var campaignWellDto in campaignWellDtos)
        {
            var existingCampaignWell = existingCampaignWells.SingleOrDefault(x => x.WellId == campaignWellDto.WellId);

            if (existingCampaignWell == null)
            {
                existingCampaign.CampaignWells.Add(new CampaignWell
                {
                    WellId = campaignWellDto.WellId,
                    CampaignId = campaignId,
                    StartYear = campaignWellDto.StartYear,
                    Values = campaignWellDto.Values
                });

                continue;
            }

            existingCampaignWell.StartYear = campaignWellDto.StartYear;
            existingCampaignWell.Values = campaignWellDto.Values;
        }

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
