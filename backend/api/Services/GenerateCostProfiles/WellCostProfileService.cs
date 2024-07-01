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
        var wellProjectWells = await GetAllWellProjectWells().Where(wpw => wellIds.Contains(wpw.WellId)).ToListAsync();

        await UpdateExplorationCostProfiles(explorationWells);
        await UpdateWellProjectCostProfiles(wellProjectWells);
    }

    private async Task UpdateWellProjectCostProfiles(List<WellProjectWell> wellProjectWells)
    {
        var wellProjects = wellProjectWells.Select(wpw => wpw.WellProject).Distinct();

        foreach (var wellProject in wellProjects)
        {
            var connectedWellProjectWells = await GetAllWellProjectWellsForWellProject(wellProject.Id);

            var connectedWellProjectCategoryWells = connectedWellProjectWells
                .Where(wpw => wpw.Well.WellCategory == WellCategory.Oil_Producer);
            var wellProjectWellCostProfileValues = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(connectedWellProjectCategoryWells);

            wellProject.OilProducerCostProfile ??= new OilProducerCostProfile
            {
                WellProject = wellProject
            };

            wellProject.OilProducerCostProfile.Values = wellProjectWellCostProfileValues.Values;
            wellProject.OilProducerCostProfile.StartYear = wellProjectWellCostProfileValues.StartYear;

            var connectedGasProducerWells = connectedWellProjectWells
                .Where(wpw => wpw.Well.WellCategory == WellCategory.Gas_Producer);
            var gasProducerCostProfileValues = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(connectedGasProducerWells);

            wellProject.GasProducerCostProfile ??= new GasProducerCostProfile
            {
                WellProject = wellProject
            };

            wellProject.GasProducerCostProfile.Values = gasProducerCostProfileValues.Values;
            wellProject.GasProducerCostProfile.StartYear = gasProducerCostProfileValues.StartYear;

            var connectedWaterInjectorWells = connectedWellProjectWells
                .Where(wpw => wpw.Well.WellCategory == WellCategory.Water_Injector);
            var waterInjectorCostProfileValues = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(connectedWaterInjectorWells);

            wellProject.WaterInjectorCostProfile ??= new WaterInjectorCostProfile
            {
                WellProject = wellProject
            };

            wellProject.WaterInjectorCostProfile.Values = waterInjectorCostProfileValues.Values;
            wellProject.WaterInjectorCostProfile.StartYear = waterInjectorCostProfileValues.StartYear;

            var connectedGasInjectorWells = connectedWellProjectWells
                .Where(wpw => wpw.Well.WellCategory == WellCategory.Gas_Injector);
            var gasInjectorCostProfileValues = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(connectedGasInjectorWells);

            wellProject.GasInjectorCostProfile ??= new GasInjectorCostProfile
            {
                WellProject = wellProject
            };

            wellProject.GasInjectorCostProfile.Values = gasInjectorCostProfileValues.Values;
            wellProject.GasInjectorCostProfile.StartYear = gasInjectorCostProfileValues.StartYear;
        }
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

            exploration.ExplorationWellCostProfile ??= new ExplorationWellCostProfile
            {
                Exploration = exploration
            };

            exploration.ExplorationWellCostProfile.Values = explorationWellCostProfileValues.Values;
            exploration.ExplorationWellCostProfile.StartYear = explorationWellCostProfileValues.StartYear;

            var connectedAppraisalWells = connectedExplorationWells
                .Where(ew => ew.Well.WellCategory == WellCategory.Appraisal_Well);
            var appraisalWellCostProfileValues = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(connectedAppraisalWells);

            exploration.AppraisalWellCostProfile ??= new AppraisalWellCostProfile
            {
                Exploration = exploration
            };

            exploration.AppraisalWellCostProfile.Values = appraisalWellCostProfileValues.Values;
            exploration.AppraisalWellCostProfile.StartYear = appraisalWellCostProfileValues.StartYear;

            var connectedSidetrackWells = connectedExplorationWells
                .Where(ew => ew.Well.WellCategory == WellCategory.Sidetrack);
            var sidetrackCostProfileValues = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(connectedSidetrackWells);

            exploration.SidetrackCostProfile ??= new SidetrackCostProfile
            {
                Exploration = exploration
            };

            exploration.SidetrackCostProfile.Values = sidetrackCostProfileValues.Values;
            exploration.SidetrackCostProfile.StartYear = sidetrackCostProfileValues.StartYear;
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

    private static TimeSeries<double> GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(IEnumerable<WellProjectWell> wellProjectWells)
    {
        var costProfilesList = new List<TimeSeries<double>>();
        foreach (var wellProjectWell in wellProjectWells)
        {
            if (wellProjectWell?.DrillingSchedule?.Values?.Length > 0)
            {
                var well = wellProjectWell.Well;
                var values = wellProjectWell.DrillingSchedule.Values.Select(ds => ds * well.WellCost).ToArray();
                var costProfile = new TimeSeries<double>
                {
                    Values = values,
                    StartYear = wellProjectWell.DrillingSchedule.StartYear,
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

    private async Task<List<WellProjectWell>> GetAllWellProjectWellsForWellProject(Guid wellProjectId)
    {
        return await _context.WellProjectWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Where(ew => ew.WellProjectId == wellProjectId).ToListAsync();
    }

    private IQueryable<WellProjectWell> GetAllWellProjectWells()
    {
        return _context.WellProjectWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Include(ew => ew.WellProject)
                .ThenInclude(e => e.OilProducerCostProfile)
            .Include(ew => ew.WellProject)
                .ThenInclude(e => e.GasProducerCostProfile)
            .Include(ew => ew.WellProject)
                .ThenInclude(e => e.WaterInjectorCostProfile)
            .Include(ew => ew.WellProject)
                .ThenInclude(e => e.GasInjectorCostProfile);
    }
}
