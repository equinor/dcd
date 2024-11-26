using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CostProfileFromDrillingScheduleHelper(
    DcdDbContext context,
    ICaseService caseService,
    IMapper mapper) : ICostProfileFromDrillingScheduleHelper
{
    public async Task UpdateCostProfilesForWells(List<Guid> wellIds)
    {
        var explorationWells = GetAllExplorationWells().Where(ew => wellIds.Contains(ew.WellId));

        var wellProjectWells = GetAllWellProjectWells().Where(wpw => wellIds.Contains(wpw.WellId));

        var uniqueExplorationIds = explorationWells.Select(ew => ew.ExplorationId).Distinct();
        var uniqueWellProjectIds = wellProjectWells.Select(wpw => wpw.WellProjectId).Distinct();

        var explorationCases = (await caseService.GetAll()).Where(c => uniqueExplorationIds.Contains(c.ExplorationLink));
        var wellProjectCases = (await caseService.GetAll()).Where(c => uniqueWellProjectIds.Contains(c.WellProjectLink));

        var explorationCaseIds = explorationCases.Select(c => c.Id).Distinct();
        var wellProjectCaseIds = wellProjectCases.Select(c => c.Id).Distinct();

        var updatedExplorationDtoList = new List<Exploration>();
        foreach (var caseId in explorationCaseIds)
        {
            var explorationDto = await UpdateExplorationCostProfilesForCase(caseId);
            updatedExplorationDtoList.Add(explorationDto);
        }

        var updatedWellProjectDtoList = new List<WellProject>();
        foreach (var caseId in wellProjectCaseIds)
        {
            var wellProjectDto = await UpdateWellProjectCostProfilesForCase(caseId);
            updatedWellProjectDtoList.Add(wellProjectDto);
        }

        UpdateExplorations(updatedExplorationDtoList.ToArray());

        UpdateWellProjects(updatedWellProjectDtoList.ToArray());

        await context.SaveChangesAsync();
    }

    private WellProjectWithProfilesDto[] UpdateWellProjects(WellProject[] updatedWellProjects)
    {
        var updatedWellProjectDtoList = new List<WellProjectWithProfilesDto>();
        foreach (var updatedWellProject in updatedWellProjects)
        {
            var wellProject = context.WellProjects.Update(updatedWellProject);
            var wellProjectDto = mapper.Map<WellProjectWithProfilesDto>(wellProject.Entity);
            if (wellProjectDto == null)
            {
                throw new ArgumentNullException(nameof(wellProjectDto));
            }
            updatedWellProjectDtoList.Add(wellProjectDto);
        }

        return updatedWellProjectDtoList.ToArray();
    }

    private ExplorationWithProfilesDto[] UpdateExplorations(Exploration[] updatedExplorations)
    {
        var updatedExplorationDtoList = new List<ExplorationWithProfilesDto>();
        foreach (var updatedExploration in updatedExplorations)
        {
            var exploration = context.Explorations.Update(updatedExploration);
            var explorationDto = mapper.Map<ExplorationWithProfilesDto>(exploration.Entity);
            if (explorationDto == null)
            {
                throw new ArgumentNullException(nameof(explorationDto));
            }
            updatedExplorationDtoList.Add(explorationDto);
        }

        return updatedExplorationDtoList.ToArray();
    }

    private async Task<Exploration> GetExploration(Guid explorationId)
    {
        var exploration = await context.Explorations.FindAsync(explorationId)
            ?? throw new NotFoundInDBException($"Exploration {explorationId} not found in database.");
        return exploration;
    }

    private async Task<WellProject> GetWellProject(Guid wellProjectId)
    {
        var wellProject = await context.WellProjects.FindAsync(wellProjectId)
            ?? throw new NotFoundInDBException($"WellProject {wellProjectId} not found in database.");
        return wellProject;
    }

    private async Task<Exploration> UpdateExplorationCostProfilesForCase(Guid caseId)
    {
        var caseItem = await caseService.GetCase(caseId);

        return await UpdateExplorationCostProfiles(caseItem.ExplorationLink);
    }

    public async Task<Exploration> UpdateExplorationCostProfiles(Guid explorationId)
    {
        var exploration = await GetExploration(explorationId);

        var explorationWells = GetAllExplorationWells().Where(ew => ew.ExplorationId == exploration.Id);

        return UpdateExplorationCostProfilesForCase(exploration, explorationWells);
    }

    private Exploration UpdateExplorationCostProfilesForCase(Exploration exploration, IEnumerable<ExplorationWell> explorationWells)
    {
        var wellIds = explorationWells.Select(ew => ew.WellId);
        var wells = GetAllWells().Where(w => wellIds.Contains(w.Id));

        var explorationCategoryWells = wells.Where(w => w.WellCategory == WellCategory.Exploration_Well).ToList();
        var explorationWellExplorationCategoryWells = explorationWells.Where(ew => explorationCategoryWells.Exists(w => w.Id == ew.WellId)).ToList();

        var appraisalWells = wells.Where(w => w.WellCategory == WellCategory.Appraisal_Well).ToList();
        var explorationWellAppraisal = explorationWells.Where(ew => appraisalWells.Exists(w => w.Id == ew.WellId)).ToList();

        var sidetrackWells = wells.Where(w => w.WellCategory == WellCategory.Sidetrack).ToList();
        var explorationWellSidetrack = explorationWells.Where(ew => sidetrackWells.Exists(w => w.Id == ew.WellId)).ToList();

        var explorationCategoryTimeSeries = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(explorationCategoryWells, explorationWellExplorationCategoryWells);
        var explorationCategoryCostProfile = new ExplorationWellCostProfile
        {
            Values = explorationCategoryTimeSeries.Values,
            StartYear = explorationCategoryTimeSeries.StartYear,
        };

        var appraisalTimeSeries = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(appraisalWells, explorationWellAppraisal);
        var appraisalCostProfile = new AppraisalWellCostProfile
        {
            Values = appraisalTimeSeries.Values,
            StartYear = appraisalTimeSeries.StartYear,
        };

        var sidetrackTimeSeries = GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(sidetrackWells, explorationWellSidetrack);
        var sidetrackCostProfile = new SidetrackCostProfile
        {
            Values = sidetrackTimeSeries.Values,
            StartYear = sidetrackTimeSeries.StartYear,
        };

        exploration.ExplorationWellCostProfile = explorationCategoryCostProfile;
        exploration.AppraisalWellCostProfile = appraisalCostProfile;
        exploration.SidetrackCostProfile = sidetrackCostProfile;

        return exploration;
    }

    private static TimeSeries<double> GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(List<Well> wells, List<ExplorationWell> explorationWells)
    {
        var costProfilesList = new List<TimeSeries<double>?>();
        foreach (var explorationWell in explorationWells)
        {
            if (explorationWell?.DrillingSchedule?.Values?.Length > 0)
            {
                var well = wells.Single(w => w.Id == explorationWell.WellId);
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

    private async Task<WellProject> UpdateWellProjectCostProfilesForCase(Guid caseId)
    {
        var caseItem = await caseService.GetCase(caseId);

        return await UpdateWellProjectCostProfiles(caseItem.WellProjectLink);
    }

    public async Task<WellProject> UpdateWellProjectCostProfiles(Guid wellProjectId)
    {
        var wellProject = await GetWellProject(wellProjectId);
        var wellProjectWells = GetAllWellProjectWells().Where(ew => ew.WellProjectId == wellProject.Id);

        return UpdateWellProjectCostProfilesForCase(wellProject, wellProjectWells);
    }

    private WellProject UpdateWellProjectCostProfilesForCase(WellProject wellProject, IEnumerable<WellProjectWell> wellProjectWells)
    {
        var wellIds = wellProjectWells.Select(ew => ew.WellId);
        var wells = GetAllWells().Where(w => wellIds.Contains(w.Id));

        var oilProducerWells = wells.Where(w => w.WellCategory == WellCategory.Oil_Producer).ToList();
        var wellProjectWellOilProducer = wellProjectWells.Where(ew => oilProducerWells.Exists(w => w.Id == ew.WellId)).ToList();

        var gasProducerWells = wells.Where(w => w.WellCategory == WellCategory.Gas_Producer).ToList();
        var wellProjectWellGasProducer = wellProjectWells.Where(ew => gasProducerWells.Exists(w => w.Id == ew.WellId)).ToList();

        var waterInjectorWells = wells.Where(w => w.WellCategory == WellCategory.Water_Injector).ToList();
        var wellProjectWellWaterInjector = wellProjectWells.Where(ew => waterInjectorWells.Exists(w => w.Id == ew.WellId)).ToList();

        var gasInjectorWells = wells.Where(w => w.WellCategory == WellCategory.Gas_Injector).ToList();
        var wellProjectWellGasInjector = wellProjectWells.Where(ew => gasInjectorWells.Exists(w => w.Id == ew.WellId)).ToList();

        var oilProducerTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(oilProducerWells, wellProjectWellOilProducer);
        var oilProducerCostProfile = new OilProducerCostProfile
        {
            Values = oilProducerTimeSeries.Values,
            StartYear = oilProducerTimeSeries.StartYear,
        };

        var gasProducerTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(gasProducerWells, wellProjectWellGasProducer);
        var gasProducerCostProfile = new GasProducerCostProfile
        {
            Values = gasProducerTimeSeries.Values,
            StartYear = gasProducerTimeSeries.StartYear,
        };

        var waterInjectorTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(waterInjectorWells, wellProjectWellWaterInjector);
        var waterInjectorCostProfile = new WaterInjectorCostProfile
        {
            Values = waterInjectorTimeSeries.Values,
            StartYear = waterInjectorTimeSeries.StartYear,
        };

        var gasInjectorTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(gasInjectorWells, wellProjectWellGasInjector);
        var gasInjectorCostProfile = new GasInjectorCostProfile
        {
            Values = gasInjectorTimeSeries.Values,
            StartYear = gasInjectorTimeSeries.StartYear,
        };

        wellProject.OilProducerCostProfile = oilProducerCostProfile;
        wellProject.GasProducerCostProfile = gasProducerCostProfile;
        wellProject.WaterInjectorCostProfile = waterInjectorCostProfile;
        wellProject.GasInjectorCostProfile = gasInjectorCostProfile;

        return wellProject;
    }

    private static TimeSeries<double> GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(List<Well> wells, List<WellProjectWell> wellProjectWells)
    {
        var costProfilesList = new List<TimeSeries<double>?>();
        foreach (var wellProjectWell in wellProjectWells)
        {
            if (wellProjectWell?.DrillingSchedule?.Values?.Length > 0)
            {
                var well = wells.Single(w => w.Id == wellProjectWell.WellId);
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

    private IQueryable<Well> GetAllWells()
    {
        if (context.Wells != null)
        {
            return context.Wells;
        }
        else
        {
            return Enumerable.Empty<Well>().AsQueryable();
        }
    }

    private IQueryable<ExplorationWell> GetAllExplorationWells()
    {
        return context.ExplorationWell.Include(ew => ew.DrillingSchedule);
    }

    private IQueryable<WellProjectWell> GetAllWellProjectWells()
    {
        return context.WellProjectWell.Include(wpw => wpw.DrillingSchedule);
    }
}
