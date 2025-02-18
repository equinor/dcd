using api.Context;
using api.Models;

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
            .Include(x => x.WellProject)
            .Include(x => x.Exploration)
            .Where(x => caseIds.Contains(x.Id))
            .ToListAsync();

        await context.TimeSeriesProfiles
            .Where(x => caseIds.Contains(x.CaseId))
            .LoadAsync();

        var wellProjectIds = caseItems.Select(x => x.WellProjectId).ToList();

        await context.DevelopmentWells
            .Include(x => x.Well)
            .Where(x => wellProjectIds.Contains(x.WellProjectId))
            .LoadAsync();

        var explorationIds = caseItems.Select(x => x.ExplorationId).ToList();

        await context.ExplorationWell
            .Include(x => x.Well)
            .Where(x => explorationIds.Contains(x.ExplorationId))
            .LoadAsync();

        var drillingSchedulesForDevelopmentWell = await (
                from caseItem in context.Cases
                join dw in context.DevelopmentWells on caseItem.WellProjectId equals dw.WellProjectId
                select new
                {
                    CaseId = caseItem.Id,
                    DevelopmentWell = dw
                })
            .GroupBy(x => x.CaseId, x => x.DevelopmentWell)
            .ToDictionaryAsync(x => x.Key, x => x.ToList());

        var drillingSchedulesForExplorationWell = await (
                from caseItem in context.Cases
                join ew in context.ExplorationWell on caseItem.ExplorationId equals ew.ExplorationId
                select new
                {
                    CaseId = caseItem.Id,
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
    public required List<DevelopmentWell> DevelopmentWells { get; set; }
    public required List<ExplorationWell> ExplorationWells { get; set; }
}
