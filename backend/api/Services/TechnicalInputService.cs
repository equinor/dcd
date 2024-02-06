using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

namespace api.Services;

public class TechnicalInputService : ITechnicalInputService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ICaseService _caseService;
    private readonly IWellProjectService _wellProjectService;
    private readonly IExplorationService _explorationService;
    private readonly IExplorationOperationalWellCostsService _explorationOperationalWellCostsService;
    private readonly IDevelopmentOperationalWellCostsService _developmentOperationalWellCostsService;
    private readonly IWellService _wellService;
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly ILogger<TechnicalInputService> _logger;

    public TechnicalInputService(DcdDbContext context, IProjectService projectService, ICaseService caseService,
    IWellProjectService wellProjectService, IExplorationService explorationService,
    IExplorationOperationalWellCostsService explorationOperationalWellCostsService,
    IDevelopmentOperationalWellCostsService developmentOperationalWellCostsService,
    IWellService wellService, ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
    ILoggerFactory loggerFactory)
    {
        _context = context;

        _projectService = projectService;
        _caseService = caseService;

        _explorationService = explorationService;
        _wellProjectService = wellProjectService;

        _explorationOperationalWellCostsService = explorationOperationalWellCostsService;
        _developmentOperationalWellCostsService = developmentOperationalWellCostsService;

        _wellService = wellService;

        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;

        _logger = loggerFactory.CreateLogger<TechnicalInputService>();
    }

    public async Task<TechnicalInputDto> UpdateTehnicalInput(TechnicalInputDto technicalInputDto)
    {
        await UpdateProject(technicalInputDto.ProjectDto);

        await UpdateExplorationOperationalWellCosts(technicalInputDto.ExplorationOperationalWellCostsDto);
        await UpdateDevelopmentOperationalWellCosts(technicalInputDto.DevelopmentOperationalWellCostsDto);

        if (technicalInputDto.WellDtos?.Length > 0)
        {
            var wellResult = await CreateAndUpdateWells(technicalInputDto.WellDtos, technicalInputDto.CaseId);
            if (wellResult != null)
            {
                technicalInputDto.ExplorationDto = wellResult.Value.explorationDto;
                technicalInputDto.WellProjectDto = wellResult.Value.wellProjectDto;
            }
            var project = await _projectService.GetProject(technicalInputDto.ProjectDto.ProjectId);
            technicalInputDto.ProjectDto = ProjectDtoAdapter.Convert(project);
        }

        await _context.SaveChangesAsync();
        return technicalInputDto;
    }

    public async Task<(ExplorationDto explorationDto, WellProjectDto wellProjectDto)?> CreateAndUpdateWells(
        WellDto[] wellDtos, Guid? caseId)
    {
        var runCostProfileCalculation = false;
        var runSaveChanges = false;
        var updatedWells = new List<Guid>();
        foreach (var wellDto in wellDtos)
        {
            if (wellDto.Id == Guid.Empty)
            {
                var well = WellAdapter.Convert(wellDto);
                var updatedWell = _context.Wells!.Add(well);
                wellDto.Id = updatedWell.Entity.Id;

                runSaveChanges = true;
            }
            else
            {
                if (wellDto.HasChanges)
                {
                    var existing = await _wellService.GetWell(wellDto.Id);
                    if (wellDto.WellCost != existing.WellCost || wellDto.WellCategory != existing.WellCategory)
                    {
                        runCostProfileCalculation = true;
                        updatedWells.Add(wellDto.Id);
                    }
                    WellAdapter.ConvertExisting(existing, wellDto);

                    _context.Wells!.Update(existing);
                }
            }
        }

        if (runSaveChanges)
        {
            await _context.SaveChangesAsync();
        }
        if (runCostProfileCalculation)
        {
            await _costProfileFromDrillingScheduleHelper.UpdateCostProfilesForWells(updatedWells);
            if (caseId != null)
            {
                var caseItem = await _caseService.GetCase((Guid)caseId);
                var exploration = await _explorationService.GetExploration(caseItem.ExplorationLink);
                var wellProject = await _wellProjectService.GetWellProject(caseItem.WellProjectLink);

                return (ExplorationDtoAdapter.Convert(exploration), WellProjectDtoAdapter.Convert(wellProject));
            }
        }
        return null;
    }
    public async Task<ProjectDto> UpdateProject(ProjectDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = await _projectService.GetProject(updatedDto.ProjectId);
        ProjectAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Projects!.Update(item);
        await _context.SaveChangesAsync();
        return ProjectDtoAdapter.Convert(updatedItem.Entity);
    }

    public async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts(ExplorationOperationalWellCostsDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = await _explorationOperationalWellCostsService.GetOperationalWellCosts(updatedDto.Id) ?? new ExplorationOperationalWellCosts();
        ExplorationOperationalWellCostsAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.ExplorationOperationalWellCosts!.Update(item);
        await _context.SaveChangesAsync();
        return ExplorationOperationalWellCostsDtoAdapter.Convert(updatedItem.Entity);
    }

    public async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts(DevelopmentOperationalWellCostsDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = await _developmentOperationalWellCostsService.GetOperationalWellCosts(updatedDto.Id) ?? new DevelopmentOperationalWellCosts();
        DevelopmentOperationalWellCostsAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.DevelopmentOperationalWellCosts!.Update(item);
        await _context.SaveChangesAsync();
        return DevelopmentOperationalWellCostsDtoAdapter.Convert(updatedItem.Entity);
    }
}
