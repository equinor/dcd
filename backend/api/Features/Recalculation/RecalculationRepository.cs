using api.Context;
using api.Models;
using api.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Recalculation;

public class RecalculationRepository(DcdDbContext context)
{
    public async Task<CaseWithCampaignWells> LoadCaseData(Guid caseId)
    {
        var cases = await LoadCaseData([caseId]);

        return cases.Single();
    }

    public async Task<List<CaseWithCampaignWells>> LoadCaseData(List<Guid> caseIds)
    {
        var caseItems = await context.Cases
            .Include(x => x.Project)
            .Include(x => x.DrainageStrategy)
            .Include(x => x.Surf)
            .Include(x => x.Topside)
            .Where(x => caseIds.Contains(x.Id))
            .ToListAsync();

        await context.TimeSeriesProfiles
            .Where(x => caseIds.Contains(x.CaseId))
            .LoadAsync();

        await context.Campaigns
            .Include(x => x.CampaignWells).ThenInclude(x => x.Well)
            .Where(x => caseIds.Contains(x.CaseId))
            .LoadAsync();

        var result = new List<CaseWithCampaignWells>();

        foreach (var caseItem in caseItems)
        {
            var developmentWells = new List<CampaignWell>();
            var explorationWells = new List<CampaignWell>();

            foreach (var campaign in caseItem.Campaigns)
            {
                foreach (var campaignWell in campaign.CampaignWells)
                {
                    if (campaign.CampaignType == CampaignType.DevelopmentCampaign)
                    {
                        developmentWells.Add(campaignWell);
                    }

                    if (campaign.CampaignType == CampaignType.ExplorationCampaign)
                    {
                        explorationWells.Add(campaignWell);
                    }
                }
            }

            result.Add(new CaseWithCampaignWells
            {
                CaseItem = caseItem,
                ExplorationWells = explorationWells,
                DevelopmentWells = developmentWells
            });
        }

        return result;
    }
}

public class CaseWithCampaignWells
{
    public required Case CaseItem { get; set; }
    public required List<CampaignWell> DevelopmentWells { get; set; }
    public required List<CampaignWell> ExplorationWells { get; set; }
}
