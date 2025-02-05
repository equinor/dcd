using api.Context;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Campaigns.GetCampaignWell;

public class GetCampaignWellService(DcdDbContext context)
{
    public async Task<CampaignWellDto> GetCampaignWell(Guid projectId, Guid caseId, Guid campaignId, Guid wellId)
    {
        var developmentWell = await context.DevelopmentWells
            .Include(x => x.Well)
            .Where(x => x.CampaignId == campaignId)
            .Where(x => x.WellId == wellId)
            .Where(x => x.Campaign.CaseId == caseId)
            .Where(x => x.Campaign.Case.ProjectId == projectId)
            .SingleOrDefaultAsync();

        if (developmentWell != null)
        {
            return MapDevelopmentWell(developmentWell);
        }

        var explorationWell = await context.ExplorationWell
            .Include(x => x.Well)
            .Where(x => x.CampaignId == campaignId)
            .Where(x => x.WellId == wellId)
            .Where(x => x.Campaign.CaseId == caseId)
            .Where(x => x.Campaign.Case.ProjectId == projectId)
            .SingleAsync();

        return MapExplorationWell(explorationWell);
    }

    private static CampaignWellDto MapExplorationWell(ExplorationWell explorationWell)
    {
        return new CampaignWellDto
        {
            WellId = explorationWell.WellId,
            WellName = explorationWell.Well.Name ?? "",
            WellCategory = explorationWell.Well.WellCategory,
            StartYear = explorationWell.StartYear,
            Values = explorationWell.Values
        };
    }

    private static CampaignWellDto MapDevelopmentWell(DevelopmentWell developmentWell)
    {
        return new CampaignWellDto
        {
            WellId = developmentWell.WellId,
            WellName = developmentWell.Well.Name ?? "",
            WellCategory = developmentWell.Well.WellCategory,
            StartYear = developmentWell.StartYear,
            Values = developmentWell.Values
        };
    }
}
