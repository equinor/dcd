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

        await context.WellProjectWell
            .Include(x => x.DrillingSchedule)
            .Include(x => x.Well)
            .Where(x => wellProjectIds.Contains(x.WellProjectId))
            .LoadAsync();

        var explorationIds = caseItems.Select(x => x.ExplorationId).ToList();

        await context.ExplorationWell
            .Include(x => x.DrillingSchedule)
            .Include(x => x.Well)
            .Where(x => explorationIds.Contains(x.ExplorationId))
            .LoadAsync();

        var drillingSchedulesForWellProjectWell = await (
                from caseItem in context.Cases
                join wp in context.WellProjects on caseItem.WellProjectId equals wp.Id
                join wpw in context.WellProjectWell on wp.Id equals wpw.WellProjectId
                where wpw.DrillingSchedule != null
                select new
                {
                    CaseId = caseItem.Id,
                    DrillingSchedule = wpw.DrillingSchedule!
                })
            .GroupBy(x => x.CaseId, x => x.DrillingSchedule)
            .ToDictionaryAsync(x => x.Key, x => x.ToList());

        var drillingSchedulesForExplorationWell = await (
                from caseItem in context.Cases
                join ew in context.ExplorationWell on caseItem.ExplorationId equals ew.ExplorationId
                where ew.DrillingSchedule != null
                select new
                {
                    CaseId = caseItem.Id,
                    DrillingSchedule = ew.DrillingSchedule!
                })
            .GroupBy(x => x.CaseId, x => x.DrillingSchedule)
            .ToDictionaryAsync(x => x.Key, x => x.ToList());

        return caseItems.Select(caseItem => new CaseWithDrillingSchedules
        {
            CaseItem = caseItem,
            DrillingSchedulesForExplorationWell = drillingSchedulesForExplorationWell.TryGetValue(caseItem.Id, out var expSchedules) ? expSchedules : [],
            DrillingSchedulesForWellProjectWell = drillingSchedulesForWellProjectWell.TryGetValue(caseItem.Id, out var wpwSchedules) ? wpwSchedules : []
        }).ToList();
    }
}

public class CaseWithDrillingSchedules
{
    public required Case CaseItem { get; set; }
    public required List<DrillingSchedule> DrillingSchedulesForWellProjectWell { get; set; }
    public required List<DrillingSchedule> DrillingSchedulesForExplorationWell { get; set; }
}
