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

    public ExplorationDto UpdateExplorationCostProfilesFromDrillingSchedule(Guid caseId)
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

        var explorationCategoryTimeSeries = GenerateCostProfileFromDrillingSchedulesAndWellCost(explorationCategoryWells, explorationWellExplorationCategoryWells);
        var explorationCategoryCostProfile = new ExplorationWellCostProfile
        {
            Values = explorationCategoryTimeSeries.Values,
            StartYear = explorationCategoryTimeSeries.StartYear,
        };

        var appraisalTimeSeries = GenerateCostProfileFromDrillingSchedulesAndWellCost(appraisalWells, explorationWellAppraisal);
        var appraisalCostProfile = new AppraisalWellCostProfile
        {
            Values = appraisalTimeSeries.Values,
            StartYear = appraisalTimeSeries.StartYear,
        };

        var sidetrackTimeSeries = GenerateCostProfileFromDrillingSchedulesAndWellCost(sidetrackWells, explorationWellSidetrack);
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

    private TimeSeries<double> GenerateCostProfileFromDrillingSchedulesAndWellCost(List<Well> wells, List<ExplorationWell> explorationWells)
    {
        var costProfilesList = new List<TimeSeries<double>>();
        foreach (var explorationWell in explorationWells)
        {
            if (explorationWell != null && explorationWell.DrillingSchedule != null && explorationWell.DrillingSchedule.Values != null && explorationWell.DrillingSchedule.Values.Length > 0)
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
}
