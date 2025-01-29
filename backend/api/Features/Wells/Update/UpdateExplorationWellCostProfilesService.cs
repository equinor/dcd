using api.Context;
using api.Features.Profiles;
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
        var exploration = await context.Explorations
            .SingleAsync(x => x.Id == explorationId);

        var wellIds = await context.ExplorationWell
            .Where(ew => ew.ExplorationId == exploration.Id)
            .Select(ew => ew.WellId)
            .ToListAsync();

        await HandleExplorationWellCostProfile(exploration, wellIds);
        await HandleAppraisalWellCostProfile(exploration, wellIds);
        await HandleSidetrackCostProfile(exploration, wellIds);
    }

    private async Task HandleExplorationWellCostProfile(Exploration exploration, List<Guid> wellIds)
    {
        var (explorationCategoryWells, explorationWellExplorationCategoryWells) = await GetWellData(wellIds, exploration.Id, WellCategory.Exploration_Well);
        var explorationCategoryTimeSeries = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(explorationCategoryWells, explorationWellExplorationCategoryWells);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.ExplorationWellCostProfile))
            .Where(x => x.ExplorationLink == exploration.Id)
            .SingleAsync();

        var explorationWellCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.ExplorationWellCostProfile);

        explorationWellCostProfile.StartYear = explorationCategoryTimeSeries.StartYear;
        explorationWellCostProfile.Values = explorationCategoryTimeSeries.Values;
    }

    private async Task HandleAppraisalWellCostProfile(Exploration exploration, List<Guid> wellIds)
    {
        var (appraisalWells, explorationWellAppraisal) = await GetWellData(wellIds, exploration.Id, WellCategory.Appraisal_Well);
        var appraisalTimeSeries = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(appraisalWells, explorationWellAppraisal);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.AppraisalWellCostProfile))
            .Where(x => x.ExplorationLink == exploration.Id)
            .SingleAsync();

        var appraisalWellCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.AppraisalWellCostProfile);

        appraisalWellCostProfile.StartYear = appraisalTimeSeries.StartYear;
        appraisalWellCostProfile.Values = appraisalTimeSeries.Values;
    }

    private async Task HandleSidetrackCostProfile(Exploration exploration, List<Guid> wellIds)
    {
        var (sidetrackWells, explorationWellSidetrack) = await GetWellData(wellIds, exploration.Id, WellCategory.Sidetrack);
        var sidetrackTimeSeries = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(sidetrackWells, explorationWellSidetrack);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.SidetrackCostProfile))
            .Where(x => x.ExplorationLink == exploration.Id)
            .SingleAsync();

        var sidetrackCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SidetrackCostProfile);

        sidetrackCostProfile.StartYear = sidetrackTimeSeries.StartYear;
        sidetrackCostProfile.Values = sidetrackTimeSeries.Values;
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

    private static TimeSeriesCost GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(List<Well> wells, List<ExplorationWell> explorationWells)
    {
        var costProfilesList = new List<TimeSeriesCost>();

        foreach (var explorationWell in explorationWells)
        {
            if (explorationWell.DrillingSchedule?.Values.Length > 0)
            {
                var well = wells.Single(w => w.Id == explorationWell.WellId);
                var values = explorationWell.DrillingSchedule.Values.Select(ds => ds * well.WellCost).ToArray();

                costProfilesList.Add(new TimeSeriesCost
                {
                    Values = values,
                    StartYear = explorationWell.DrillingSchedule.StartYear
                });
            }
        }

        return CostProfileMerger.MergeCostProfiles(costProfilesList);
    }
}
