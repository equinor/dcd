using api.Context;
using api.Exceptions;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Update;

public class UpdateExplorationWellCostProfilesService(DcdDbContext context)
{
    public async Task HandleExplorationWell(List<Guid> wellIds)
    {
        var uniqueExplorationIds = await context.ExplorationWell
            .Where(x => wellIds.Contains(x.WellId))
            .Select(x => x.ExplorationId)
            .Distinct()
            .ToListAsync();

        var explorationLinksUsedByCases = await context.Cases
            .Where(c => uniqueExplorationIds.Contains(c.ExplorationLink))
            .Select(c => c.ExplorationLink)
            .Distinct()
            .ToListAsync();

        foreach (var explorationLinks in explorationLinksUsedByCases)
        {
            await UpdateExplorationCostProfiles(explorationLinks);
        }
    }

    public async Task UpdateExplorationCostProfiles(Guid explorationId)
    {
        var exploration = await context.Explorations.SingleAsync(x => x.Id == explorationId)
                          ?? throw new NotFoundInDbException($"Exploration {explorationId} not found in database.");

        var wellIds = await context.ExplorationWell
            .Where(ew => ew.ExplorationId == exploration.Id)
            .Select(ew => ew.WellId)
            .ToListAsync();

        var (explorationCategoryWells, explorationWellExplorationCategoryWells) = await GetWellData(wellIds, explorationId, WellCategory.Exploration_Well);
        var explorationCategoryTimeSeries = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(explorationCategoryWells, explorationWellExplorationCategoryWells);
        var explorationCategoryCostProfile = new ExplorationWellCostProfile
        {
            Values = explorationCategoryTimeSeries.Values,
            StartYear = explorationCategoryTimeSeries.StartYear
        };

        var (appraisalWells, explorationWellAppraisal) = await GetWellData(wellIds, explorationId, WellCategory.Appraisal_Well);
        var appraisalTimeSeries = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(appraisalWells, explorationWellAppraisal);
        var appraisalCostProfile = new AppraisalWellCostProfile
        {
            Values = appraisalTimeSeries.Values,
            StartYear = appraisalTimeSeries.StartYear
        };

        var (sidetrackWells, explorationWellSidetrack) = await GetWellData(wellIds, explorationId, WellCategory.Sidetrack);
        var sidetrackTimeSeries = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(sidetrackWells, explorationWellSidetrack);
        var sidetrackCostProfile = new SidetrackCostProfile
        {
            Values = sidetrackTimeSeries.Values,
            StartYear = sidetrackTimeSeries.StartYear
        };

        exploration.ExplorationWellCostProfile = explorationCategoryCostProfile;
        exploration.AppraisalWellCostProfile = appraisalCostProfile;
        exploration.SidetrackCostProfile = sidetrackCostProfile;
    }

    private async Task<(List<Well> wells, List<ExplorationWell> explorationWells)> GetWellData(List<Guid> wellIds, Guid explorationId, WellCategory wellCategory)
    {
        var wells = await context.Wells
            .Where(w => wellIds.Contains(w.Id))
            .Where(w => w.WellCategory == wellCategory)
            .ToListAsync();

        var wellWellIds = wells.Select(w => w.Id).ToList();

        var explorationWells = await context.ExplorationWell
            .Include(ew => ew.DrillingSchedule)
            .Where(ew => ew.ExplorationId == explorationId)
            .Where(ew => wellWellIds.Contains(ew.WellId))
            .ToListAsync();

        return (wells, explorationWells);
    }

    private static TimeSeries<double> GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(List<Well> wells, List<ExplorationWell> explorationWells)
    {
        var costProfilesList = new List<TimeSeries<double>?>();

        foreach (var explorationWell in explorationWells)
        {
            if (explorationWell.DrillingSchedule?.Values.Length > 0)
            {
                var well = wells.Single(w => w.Id == explorationWell.WellId);
                var values = explorationWell.DrillingSchedule.Values.Select(ds => ds * well.WellCost).ToArray();

                costProfilesList.Add(new TimeSeries<double>
                {
                    Values = values,
                    StartYear = explorationWell.DrillingSchedule.StartYear
                });
            }
        }

        return CostProfileMerger.MergeCostProfiles(costProfilesList);
    }
}
