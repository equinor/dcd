using api.Context;
using api.Models;
using api.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation;

public class RecalculationRepository(DcdDbContext context)
{
    public async Task<CaseWithDrillingSchedules> LoadCaseData(Guid caseId)
    {
        var cases = await LoadCaseData([caseId]);

        return cases.Single();
    }

    public async Task<List<CaseWithDrillingSchedules>> LoadCaseData(List<Guid> caseIds)
    {
        var caseItems = await context.Cases
            .Include(x => x.Project).ThenInclude(x => x.DevelopmentOperationalWellCosts)
            .Include(x => x.Surf)
            .Include(x => x.Topside)
            .Include(x => x.DrainageStrategy)
            .Where(x => caseIds.Contains(x.Id))
            .ToListAsync();

        await context.TimeSeriesProfiles
            .Where(x => caseIds.Contains(x.CaseId))
            .LoadAsync();

        await context.Campaigns
            .Include(x => x.CampaignWells).ThenInclude(x => x.Well)
            .Where(x => caseIds.Contains(x.CaseId))
            .LoadAsync();

        var drillingSchedulesForDevelopmentWell = await (
                from campaign in context.Campaigns
                join dw in context.CampaignWells on campaign.Id equals dw.CampaignId
                where caseIds.Contains(campaign.CaseId)
                where campaign.CampaignType == CampaignType.DevelopmentCampaign
                select new
                {
                    campaign.CaseId,
                    DevelopmentWell = dw
                })
            .GroupBy(x => x.CaseId, x => x.DevelopmentWell)
            .ToDictionaryAsync(x => x.Key, x => x.ToList());

        var drillingSchedulesForExplorationWell = await (
                from campaign in context.Campaigns
                join ew in context.CampaignWells on campaign.Id equals ew.CampaignId
                where caseIds.Contains(campaign.CaseId)
                where campaign.CampaignType == CampaignType.ExplorationCampaign
                select new
                {
                    campaign.CaseId,
                    ExplorationWell = ew
                })
            .GroupBy(x => x.CaseId, x => x.ExplorationWell)
            .ToDictionaryAsync(x => x.Key, x => x.ToList());

        return caseItems.Select(caseItem => new CaseWithDrillingSchedules
        {
            CaseItem = caseItem,
            ExplorationWells = drillingSchedulesForExplorationWell.TryGetValue(caseItem.Id, out var expSchedules) ? expSchedules : [],
            DevelopmentWells = drillingSchedulesForDevelopmentWell.TryGetValue(caseItem.Id, out var wpwSchedules) ? wpwSchedules : []
        }).ToList();
    }
}

public class CaseWithDrillingSchedules
{
    public required Case CaseItem { get; set; }
    public required List<CampaignWell> DevelopmentWells { get; set; }
    public required List<CampaignWell> ExplorationWells { get; set; }
}
