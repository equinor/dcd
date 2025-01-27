using api.Context;
using api.Features.Profiles;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.WellCostProfile;

public class WellCostProfileService(DcdDbContext context)
{
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

        await UpdateWellProjectCostProfiles(wellProjectWells.ToList());
        await UpdateExplorationCostProfiles(explorationWells.ToList());
    }

    public async Task UpdateCostProfilesForWells(Guid caseId)
    {
        var wellIds = new List<Guid>();

        wellIds.AddRange(await context.Cases
            .Where(w => w.Id == caseId)
            .SelectMany(x => x.WellProject!.WellProjectWells)
            .Select(x => x.WellId)
            .ToListAsync());

        wellIds.AddRange(await context.Cases
            .Where(w => w.Id == caseId)
            .SelectMany(x => x.Exploration!.ExplorationWells)
            .Select(x => x.WellId)
            .ToListAsync());

        var explorationWells = await context.ExplorationWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Include(ew => ew.Exploration)
            .Where(x => wellIds.Contains(x.WellId))
            .ToListAsync();

        var wellProjectWells = await context.WellProjectWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Include(ew => ew.WellProject).ThenInclude(e => e.WaterInjectorCostProfile)
            .Include(ew => ew.WellProject).ThenInclude(e => e.GasInjectorCostProfile)
            .Where(x => wellIds.Contains(x.WellId))
            .ToListAsync();

        await UpdateExplorationCostProfiles(explorationWells);
        await UpdateWellProjectCostProfiles(wellProjectWells);
    }

    public async Task UpdateCostProfilesForWells(List<Well> wells)
    {
        if (wells.Count == 0)
        {
            return;
        }

        var explorationWells = await GetAllExplorationWells().Where(ew => wells.Contains(ew.Well)).ToListAsync();
        var wellProjectWells = await GetAllWellProjectWells().Where(wpw => wells.Contains(wpw.Well)).ToListAsync();

        await UpdateExplorationCostProfiles(explorationWells);
        await UpdateWellProjectCostProfiles(wellProjectWells);
    }

    private async Task UpdateWellProjectCostProfiles(List<WellProjectWell> wellProjectWells)
    {
        var wellProjects = wellProjectWells.Select(wpw => wpw.WellProject).Distinct();

        foreach (var wellProject in wellProjects)
        {
            var profileTypes = new List<string>
            {
                ProfileTypes.OilProducerCostProfile,
                ProfileTypes.GasProducerCostProfile
            };

            var caseItem = await context.Cases
                .Include(x => x.TimeSeriesProfiles.Where(y => profileTypes.Contains(y.ProfileType)))
                .SingleAsync(x => x.WellProjectLink == wellProject.Id);

            var connectedWellProjectWells = await GetAllWellProjectWellsForWellProject(wellProject.Id);

            var connectedWellProjectCategoryWells = connectedWellProjectWells
                .Where(wpw => wpw.Well.WellCategory == WellCategory.Oil_Producer);
            var wellProjectWellCostProfileValues = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(connectedWellProjectCategoryWells);

            var oilProducerCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.OilProducerCostProfile);

            oilProducerCostProfile.Values = wellProjectWellCostProfileValues.Values;
            oilProducerCostProfile.StartYear = wellProjectWellCostProfileValues.StartYear;

            var connectedGasProducerWells = connectedWellProjectWells
                .Where(wpw => wpw.Well.WellCategory == WellCategory.Gas_Producer);
            var gasProducerCostProfileValues = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(connectedGasProducerWells);

            var gasProducerCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.GasProducerCostProfile);

            gasProducerCostProfile.Values = gasProducerCostProfileValues.Values;
            gasProducerCostProfile.StartYear = gasProducerCostProfileValues.StartYear;

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

            var profileTypes = new List<string>
            {
                ProfileTypes.ExplorationWellCostProfile,
                ProfileTypes.AppraisalWellCostProfile,
                ProfileTypes.SidetrackCostProfile
            };

            var caseItem = await context.Cases
                .Include(x => x.TimeSeriesProfiles.Where(y => profileTypes.Contains(y.ProfileType)))
                .Where(x => x.ExplorationLink == exploration.Id)
                .SingleAsync();

            var explorationWellCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.ExplorationWellCostProfile);

            explorationWellCostProfile.Values = explorationWellCostProfileValues.Values;
            explorationWellCostProfile.StartYear = explorationWellCostProfileValues.StartYear;

            var connectedAppraisalWells = connectedExplorationWells
                .Where(ew => ew.Well.WellCategory == WellCategory.Appraisal_Well);

            var appraisalWellCostProfileValues = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(connectedAppraisalWells);

            var appraisalWellCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.AppraisalWellCostProfile);

            appraisalWellCostProfile.Values = appraisalWellCostProfileValues.Values;
            appraisalWellCostProfile.StartYear = appraisalWellCostProfileValues.StartYear;

            var connectedSidetrackWells = connectedExplorationWells
                .Where(ew => ew.Well.WellCategory == WellCategory.Sidetrack);
            var sidetrackCostProfileValues = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(connectedSidetrackWells);

            var sidetrackCostProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SidetrackCostProfile);

            sidetrackCostProfile.Values = sidetrackCostProfileValues.Values;
            sidetrackCostProfile.StartYear = sidetrackCostProfileValues.StartYear;
        }
    }

    private static TimeSeries<double> GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(IEnumerable<ExplorationWell> explorationWells)
    {
        var costProfilesList = new List<TimeSeries<double>?>();
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

        var mergedCostProfile = CostProfileMerger.MergeCostProfiles(costProfilesList);
        return mergedCostProfile;
    }

    private static TimeSeries<double> GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(IEnumerable<WellProjectWell> wellProjectWells)
    {
        var costProfilesList = new List<TimeSeries<double>?>();
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

        var mergedCostProfile = CostProfileMerger.MergeCostProfiles(costProfilesList);
        return mergedCostProfile;
    }

    private async Task<List<ExplorationWell>> GetAllExplorationWellsForExploration(Guid explorationId)
    {
        return await context.ExplorationWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Where(ew => ew.ExplorationId == explorationId).ToListAsync();
    }

    private IQueryable<ExplorationWell> GetAllExplorationWells()
    {
        return context.ExplorationWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Include(ew => ew.Exploration);
    }

    private async Task<List<WellProjectWell>> GetAllWellProjectWellsForWellProject(Guid wellProjectId)
    {
        return await context.WellProjectWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Where(ew => ew.WellProjectId == wellProjectId).ToListAsync();
    }

    private IQueryable<WellProjectWell> GetAllWellProjectWells()
    {
        return context.WellProjectWell
            .Include(ew => ew.DrillingSchedule)
            .Include(ew => ew.Well)
            .Include(ew => ew.WellProject)
                .ThenInclude(e => e.WaterInjectorCostProfile)
            .Include(ew => ew.WellProject)
                .ThenInclude(e => e.GasInjectorCostProfile);
    }
}
