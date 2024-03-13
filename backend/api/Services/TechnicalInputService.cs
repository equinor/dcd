using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

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
    private readonly IMapper _mapper;

    public TechnicalInputService(
        DcdDbContext context,
        IProjectService projectService,
        ICaseService caseService,
        IWellProjectService wellProjectService,
        IExplorationService explorationService,
        IExplorationOperationalWellCostsService explorationOperationalWellCostsService,
        IDevelopmentOperationalWellCostsService developmentOperationalWellCostsService,
        IWellService wellService,
        ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
        ILoggerFactory loggerFactory,
        IMapper mapper
    )
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
        _mapper = mapper;
    }

    public async Task<TechnicalInputDto> UpdateTehnicalInput(Guid projectId, UpdateTechnicalInputDto technicalInputDto)
    {
        var project = await _projectService.GetProject(projectId);

        await UpdateProject(project, technicalInputDto.ProjectDto);

        await UpdateExplorationOperationalWellCosts(project, technicalInputDto.ExplorationOperationalWellCostsDto);
        await UpdateDevelopmentOperationalWellCosts(project, technicalInputDto.DevelopmentOperationalWellCostsDto);

        var returnDto = new TechnicalInputDto();

        if (technicalInputDto.UpdateWellDtos?.Length > 0 || technicalInputDto.CreateWellDtos?.Length > 0)
        {
            var wellResult = await CreateAndUpdateWells(projectId, technicalInputDto.CreateWellDtos, technicalInputDto.UpdateWellDtos);
            if (wellResult != null)
            {
                returnDto.ExplorationDto = wellResult.Value.explorationDto;
                returnDto.WellProjectDto = wellResult.Value.wellProjectDto;
            }
        }

        if (technicalInputDto.DeleteWellDtos?.Length > 0)
        {
            await DeleteWells(technicalInputDto.DeleteWellDtos);
        }

        await _context.SaveChangesAsync();

        var returnProject = await _projectService.GetProject(projectId);
        var returnProjectDto = _mapper.Map<ProjectDto>(returnProject);

        if (returnProjectDto == null)
        {
            _logger.LogError("Failed to map project to dto");
            throw new Exception("Failed to map project to dto");
        }

        returnDto.ProjectDto = returnProjectDto;

        return returnDto;
    }

    private async Task DeleteWells(DeleteWellDto[] deleteWellDtos)
    {
        foreach (var wellDto in deleteWellDtos)
        {
            var well = await _context.Wells!.FindAsync(wellDto.Id);
            if (well != null)
            {
                _context.Wells.Remove(well);
            }
        }
    }

    private async Task<(ExplorationDto explorationDto, WellProjectDto wellProjectDto)?> CreateAndUpdateWells(
            Guid projectId,
            CreateWellDto[]? createWellDtos,
            UpdateWellDto[]? updateWellDtos
            )
    {
        var updatedWells = new List<Guid>();

        if (createWellDtos != null)
        {
            foreach (var wellDto in createWellDtos)
            {
                var well = _mapper.Map<Well>(wellDto);
                if (well == null)
                {
                    throw new ArgumentNullException(nameof(well));
                }
                well.ProjectId = projectId;
                _context.Wells!.Add(well);
            }
        }

        if (updateWellDtos != null)
        {
            foreach (var wellDto in updateWellDtos)
            {
                var existing = await _wellService.GetWell(wellDto.Id);
                if (wellDto.WellCost != existing.WellCost || wellDto.WellCategory != existing.WellCategory)
                {
                    updatedWells.Add(wellDto.Id);
                }
                _mapper.Map(wellDto, existing);
                _context.Wells!.Update(existing);
            }
        }

        if (createWellDtos?.Any() == true || updateWellDtos?.Any() == true)
        {
            await _context.SaveChangesAsync();
        }
        if (updatedWells.Any())
        {
            await _costProfileFromDrillingScheduleHelper.UpdateCostProfilesForWells(updatedWells);
        }
        return null;
    }
    private async Task<ProjectDto> UpdateProject(Project project, UpdateProjectDto updatedDto)
    {
        _mapper.Map(updatedDto, project);
        var updatedItem = _context.Projects!.Update(project);
        await _context.SaveChangesAsync();

        var projectDto = _mapper.Map<ProjectDto>(updatedItem.Entity);
        if (projectDto == null)
        {
            _logger.LogError("Failed to map project to dto");
            throw new Exception("Failed to map project to dto");
        }

        return projectDto;
    }

    private async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts(Project project, UpdateExplorationOperationalWellCostsDto updatedDto)
    {
        if (project.ExplorationOperationalWellCosts == null)
        {
            _logger.LogError("Exploration operational well costs not found");
            throw new Exception("Exploration operational well costs not found");
        }
        var item = await _explorationOperationalWellCostsService.GetOperationalWellCosts(project.ExplorationOperationalWellCosts.Id) ?? new ExplorationOperationalWellCosts();
        _mapper.Map(updatedDto, item);
        var updatedItem = _context.ExplorationOperationalWellCosts!.Update(item);
        await _context.SaveChangesAsync();
        var explorationOperationalWellCostsDto = _mapper.Map<ExplorationOperationalWellCostsDto>(updatedItem.Entity);
        if (explorationOperationalWellCostsDto == null)
        {
            _logger.LogError("Failed to map exploration operational well costs to dto");
            throw new Exception("Failed to map exploration operational well costs to dto");
        }
        return explorationOperationalWellCostsDto;
    }

    private async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts(Project project, UpdateDevelopmentOperationalWellCostsDto updatedDto)
    {
        if (project.DevelopmentOperationalWellCosts == null)
        {
            _logger.LogError("Development operational well costs not found");
            throw new Exception("Development operational well costs not found");
        }
        var item = await _developmentOperationalWellCostsService.GetOperationalWellCosts(project.DevelopmentOperationalWellCosts.Id) ?? new DevelopmentOperationalWellCosts();
        _mapper.Map(updatedDto, item);
        var updatedItem = _context.DevelopmentOperationalWellCosts!.Update(item);
        await _context.SaveChangesAsync();
        var developmentOperationalWellCostsDto = _mapper.Map<DevelopmentOperationalWellCostsDto>(updatedItem.Entity);
        if (developmentOperationalWellCostsDto == null)
        {
            _logger.LogError("Failed to map development operational well costs to dto");
            throw new Exception("Failed to map development operational well costs to dto");
        }
        return developmentOperationalWellCostsDto;
    }
}
