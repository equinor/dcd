using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellCostProfileService : IWellCostProfileService
{
    private readonly DcdDbContext _context;
    private readonly IMapper _mapper;

    public WellCostProfileService(
        DcdDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task UpdateCostProfilesForWellsFromDrillingSchedules(List<Guid> drillingScheduleIds)
    {
        if (drillingScheduleIds.Count == 0)
        {
            return;
        }
        var explorationWells = GetAllExplorationWells()
            .Where(ew => ew.DrillingScheduleId.HasValue && drillingScheduleIds.Contains(ew.DrillingScheduleId.Value));

        var wellProjectWells = GetAllWellProjectWells()
            .Where(wpw => wpw.DrillingScheduleId.HasValue && drillingScheduleIds.Contains(wpw.DrillingScheduleId.Value));

        var wellIds = explorationWells.Select(ew => ew.WellId).Union(wellProjectWells.Select(wpw => wpw.WellId)).Distinct();

        await UpdateCostProfilesForWells(wellIds.ToList());
    }

    public async Task UpdateCostProfilesForWells(List<Guid> wellIds)
    {
        var explorationWells = await GetAllExplorationWells().Where(ew => wellIds.Contains(ew.WellId)).ToListAsync();
        Console.WriteLine("ExplorationWells count " + explorationWells.Count);
        var wellProjectWells = GetAllWellProjectWells().Where(wpw => wellIds.Contains(wpw.WellId));

        await UpdateExplorationCostProfiles(explorationWells);
    }

    private async Task UpdateExplorationCostProfiles(List<ExplorationWell> explorationWells)
    {
        var explorations = explorationWells.Select(ew => ew.Exploration).Distinct();

        foreach (var exploration in explorations)
        {
            var connectedExplorationWells = await GetAllExplorationWellsForExploration(exploration.Id);

            var connectedExplorationCategoryWells = connectedExplorationWells
                .Where(ew => ew.Well.WellCategory == WellCategory.Exploration_Well);
            var explorationWellCostProfileValues = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(connectedExplorationCategoryWells);

            if (exploration.ExplorationWellCostProfile != null)
            {
                exploration.ExplorationWellCostProfile.Values = explorationWellCostProfileValues.Values;
                exploration.ExplorationWellCostProfile.StartYear = explorationWellCostProfileValues.StartYear;
            }
            else
            {
                var explorationWellCostProfile = new ExplorationWellCostProfile
                {
                    Exploration = exploration,
                    Values = explorationWellCostProfileValues.Values,
                    StartYear = explorationWellCostProfileValues.StartYear,
                };
                Console.WriteLine("Exploration well costs values: " + explorationWellCostProfile.Values);
                foreach (var value in explorationWellCostProfile.Values)
                {
                    Console.WriteLine(value);
                }
                exploration.ExplorationWellCostProfile = explorationWellCostProfile;
            }
        }
    }


    private static TimeSeries<double> GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(IEnumerable<ExplorationWell> explorationWells)
    {
        var costProfilesList = new List<TimeSeries<double>>();
        foreach (var explorationWell in explorationWells)
        {
            if (explorationWell?.DrillingSchedule?.Values?.Length > 0)
            {
                var well = explorationWell.Well;
                var values = explorationWell.DrillingSchedule.Values.Select(ds => ds * well.WellCost).ToArray();
                var costProfile = new TimeSeries<double>
                {
                    Values = values,
                    StartYear = explorationWell.DrillingSchedule.StartYear,
                };
                costProfilesList.Add(costProfile);
            }
        }

        var mergedCostProfile = TimeSeriesCost.MergeCostProfilesList(costProfilesList);
        return mergedCostProfile;
    }

    private async Task<List<ExplorationWell>> GetAllExplorationWellsForExploration(Guid explorationId)
    {
        return await _context.ExplorationWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Where(ew => ew.ExplorationId == explorationId).ToListAsync();
    }

    private IQueryable<ExplorationWell> GetAllExplorationWells()
    {
        return _context.ExplorationWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Include(ew => ew.Exploration)
                .ThenInclude(e => e.ExplorationWellCostProfile)
            .Include(ew => ew.Exploration)
                .ThenInclude(e => e.AppraisalWellCostProfile)
            .Include(ew => ew.Exploration)
                .ThenInclude(e => e.SidetrackCostProfile);

    }

    private IQueryable<WellProjectWell> GetAllWellProjectWells()
    {
        return _context.WellProjectWell
            .Include(wpw => wpw.DrillingSchedule)
            .Include(wpw => wpw.WellProject)
            .Include(wpw => wpw.Well);
    }
}
