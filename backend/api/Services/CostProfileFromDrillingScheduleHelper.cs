using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CostProfileFromDrillingScheduleHelper
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExplorationService> _logger;

    public CostProfileFromDrillingScheduleHelper(DcdDbContext context, ProjectService
        projectService, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _context = context;
        _projectService = projectService;
        _serviceProvider = serviceProvider;
        _logger = loggerFactory.CreateLogger<ExplorationService>();
    }

    public void UpdateCostProfilesForWells(List<Guid> wellIds)
    {
        var explorationWellService = _serviceProvider.GetRequiredService<ExplorationWellService>();
        var explorationWells = explorationWellService.GetAll().Where(ew => wellIds.Contains(ew.WellId));

        var wellProjectWellService = _serviceProvider.GetRequiredService<WellProjectWellService>();
        var wellProjectWells = wellProjectWellService.GetAll().Where(wpw => wellIds.Contains(wpw.WellId));

        var uniqueExplorationIds = explorationWells.Select(ew => ew.ExplorationId).Distinct();
        var uniqueWellProjectIds = wellProjectWells.Select(wpw => wpw.WellProjectId).Distinct();

        var caseService = _serviceProvider.GetRequiredService<CaseService>();
        var explorationCases = caseService.GetAll().Where(c => uniqueExplorationIds.Contains(c.ExplorationLink));
        var wellProjectCases = caseService.GetAll().Where(c => uniqueWellProjectIds.Contains(c.WellProjectLink));

        var explorationCaseIds = explorationCases.Select(c => c.Id).Distinct();
        var wellProjectCaseIds = wellProjectCases.Select(c => c.Id).Distinct();

        foreach (var caseId in explorationCaseIds)
        {
            UpdateExplorationCostProfilesForCase(caseId);
        }

        foreach (var caseId in wellProjectCaseIds)
        {
            UpdateWellProjectCostProfilesForCase(caseId);
        }
    }

    public ExplorationDto UpdateExplorationCostProfilesForCase(Guid caseId)
    {
        var caseService = _serviceProvider.GetRequiredService<CaseService>();
        var caseItem = caseService.GetCase(caseId);

        var explorationService = _serviceProvider.GetRequiredService<ExplorationService>();
        var exploration = explorationService.GetExploration(caseItem.ExplorationLink);

        var explorationWellService = _serviceProvider.GetRequiredService<ExplorationWellService>();
        var explorationWells = explorationWellService.GetAll().Where(ew => ew.ExplorationId == exploration.Id);

        var wellService = _serviceProvider.GetRequiredService<WellService>();
        var wellIds = explorationWells.Select(ew => ew.WellId);
        var wells = wellService.GetAll().Where(w => wellIds.Contains(w.Id));

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

        var explorationDto = ExplorationDtoAdapter.Convert(exploration);
        var dto = explorationService.NewUpdateExploration(explorationDto);
        return dto;
    }

    private static TimeSeries<double> GenerateExplorationCostProfileFromDrillingSchedulesAndWellCost(List<Well> wells, List<ExplorationWell> explorationWells)
    {
        var costProfilesList = new List<TimeSeries<double>>();
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

    public WellProjectDto UpdateWellProjectCostProfilesForCase(Guid caseId)
    {
        var caseService = _serviceProvider.GetRequiredService<CaseService>();
        var caseItem = caseService.GetCase(caseId);

        var wellProjectService = _serviceProvider.GetRequiredService<WellProjectService>();
        var wellProject = wellProjectService.GetWellProject(caseItem.WellProjectLink);

        var wellProjectWellService = _serviceProvider.GetRequiredService<WellProjectWellService>();
        var wellProjectWells = wellProjectWellService.GetAll().Where(ew => ew.WellProjectId == wellProject.Id);

        var wellService = _serviceProvider.GetRequiredService<WellService>();
        var wellIds = wellProjectWells.Select(ew => ew.WellId);
        var wells = wellService.GetAll().Where(w => wellIds.Contains(w.Id));

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

        var wellProjectDto = WellProjectDtoAdapter.Convert(wellProject);
        var dto = wellProjectService.NewUpdateWellProject(wellProjectDto);
        return dto;
    }

    private static TimeSeries<double> GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(List<Well> wells, List<WellProjectWell> wellProjectWells)
    {
        var costProfilesList = new List<TimeSeries<double>>();
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

}
