using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

namespace api.Services;

public class TechnicalInputService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly ExplorationOperationalWellCostsService _explorationOperationalWellCostsService;
    private readonly DevelopmentOperationalWellCostsService _developmentOperationalWellCostsService;
    private readonly WellService _wellService;
    private readonly CostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly ILogger<CaseService> _logger;

    public TechnicalInputService(DcdDbContext context, ProjectService projectService,
    ExplorationOperationalWellCostsService explorationOperationalWellCostsService,
    DevelopmentOperationalWellCostsService developmentOperationalWellCostsService,
    WellService wellService, CostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
    ILoggerFactory loggerFactory)
    {
        _context = context;

        _projectService = projectService;

        _explorationOperationalWellCostsService = explorationOperationalWellCostsService;
        _developmentOperationalWellCostsService = developmentOperationalWellCostsService;

        _wellService = wellService;

        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;

        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public TechnicalInputDto UpdateTehnicalInput(TechnicalInputDto technicalInputDto)
    {
        UpdateProject(technicalInputDto.ProjectDto);

        UpdateExplorationOperationalWellCosts(technicalInputDto.ExplorationOperationalWellCostsDto);
        UpdateDevelopmentOperationalWellCosts(technicalInputDto.DevelopmentOperationalWellCostsDto);

        if (technicalInputDto.WellDtos?.Length > 0)
        {
            CreateAndUpdateWells(technicalInputDto.WellDtos);
        }

        _context.SaveChanges();
        return technicalInputDto;
    }

    public void CreateAndUpdateWells(WellDto[] wellDtos)
    {
        var changes = false;
        var runCostProfileCalculation = false;
        var runSaveChanges = false;
        var updatedWells = new List<Guid>();
        foreach (var wellDto in wellDtos)
        {
            if (wellDto.Id == Guid.Empty)
            {
                var well = WellAdapter.Convert(wellDto);
                var updatedWell = _context.Wells!.Add(well);

                changes = true;
                runSaveChanges = true;
            }
            else
            {
                if (wellDto.HasChanges)
                {
                    var existing = _wellService.GetWell(wellDto.Id);
                    if (wellDto.WellCost != existing.WellCost || wellDto.WellCategory != existing.WellCategory)
                    {
                        runCostProfileCalculation = true;
                        updatedWells.Add(wellDto.Id);
                    }
                    WellAdapter.ConvertExisting(existing, wellDto);

                    var well = _context.Wells!.Update(existing);

                    changes = true;
                }
            }
        }
        if (changes)
        {
            if (runSaveChanges)
            {
                _context.SaveChanges();
            }
            if (runCostProfileCalculation)
            {
                _costProfileFromDrillingScheduleHelper.UpdateCostProfilesForWells(updatedWells);
            }
        }
    }

    public ProjectDto UpdateProject(ProjectDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _projectService.GetProjectWithoutAssets(updatedDto.ProjectId);
        ProjectAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Projects!.Update(item);
        return ProjectDtoAdapter.Convert(updatedItem.Entity);
    }

    public ExplorationOperationalWellCostsDto UpdateExplorationOperationalWellCosts(ExplorationOperationalWellCostsDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _explorationOperationalWellCostsService.GetOperationalWellCosts(updatedDto.Id) ?? new ExplorationOperationalWellCosts();
        ExplorationOperationalWellCostsAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.ExplorationOperationalWellCosts!.Update(item);
        return ExplorationOperationalWellCostsDtoAdapter.Convert(updatedItem.Entity);
    }

    public DevelopmentOperationalWellCostsDto UpdateDevelopmentOperationalWellCosts(DevelopmentOperationalWellCostsDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _developmentOperationalWellCostsService.GetOperationalWellCosts(updatedDto.Id) ?? new DevelopmentOperationalWellCosts();
        DevelopmentOperationalWellCostsAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.DevelopmentOperationalWellCosts!.Update(item);
        return DevelopmentOperationalWellCostsDtoAdapter.Convert(updatedItem.Entity);
    }
}
