using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TechnicalInputService : ITechnicalInputService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly IExplorationOperationalWellCostsService _explorationOperationalWellCostsService;
    private readonly IDevelopmentOperationalWellCostsService _developmentOperationalWellCostsService;
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly ILogger<TechnicalInputService> _logger;
    private readonly IMapper _mapper;
    private readonly IProjectRepository _repository;


    public TechnicalInputService(
        DcdDbContext context,
        IProjectService projectService,
        IExplorationOperationalWellCostsService explorationOperationalWellCostsService,
        IDevelopmentOperationalWellCostsService developmentOperationalWellCostsService,
        ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        IProjectRepository repository
    )
    {
        _context = context;

        _projectService = projectService;

        _explorationOperationalWellCostsService = explorationOperationalWellCostsService;
        _developmentOperationalWellCostsService = developmentOperationalWellCostsService;

        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;

        _logger = loggerFactory.CreateLogger<TechnicalInputService>();
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<TechnicalInputDto> UpdateTehnicalInput(Guid projectId, UpdateTechnicalInputDto technicalInputDto)
    {
        var project = await _projectService.GetProject(projectId);

        await UpdateProject(project, technicalInputDto.ProjectDto);

        await UpdateExplorationOperationalWellCosts(project, technicalInputDto.ExplorationOperationalWellCostsDto);
        await UpdateDevelopmentOperationalWellCosts(project, technicalInputDto.DevelopmentOperationalWellCostsDto);

        var returnDto = new TechnicalInputDto();

        if (technicalInputDto.DeleteWellDtos?.Length > 0)
        {
            await DeleteWells(technicalInputDto.DeleteWellDtos);
        }

        if (technicalInputDto.UpdateWellDtos?.Length > 0 || technicalInputDto.CreateWellDtos?.Length > 0)
        {
            var wellResult = await CreateAndUpdateWells(projectId, technicalInputDto.CreateWellDtos, technicalInputDto.UpdateWellDtos);
            if (wellResult != null)
            {
                returnDto.ExplorationDto = wellResult.Value.explorationDto;
                returnDto.WellProjectDto = wellResult.Value.wellProjectDto;
            }
        }

        await _context.SaveChangesAsync();

        var returnProject = await _projectService.GetProject(projectId);
        var returnProjectDto = _mapper.Map<ProjectDto>(returnProject, opts => opts.Items["ConversionUnit"] = returnProject.PhysicalUnit.ToString());

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
        var affectedAssets = new Dictionary<string, List<Guid>>() {
            { nameof(Exploration), new List<Guid>() },
            { nameof(WellProject), new List<Guid>() }
        };

        foreach (var wellDto in deleteWellDtos)
        {
            var well = await _context.Wells!.FindAsync(wellDto.Id);
            if (well != null)
            {
                var explorationWells = _context.ExplorationWell!.Where(ew => ew.WellId == well.Id);
                foreach (var explorationWell in explorationWells)
                {
                    _context.ExplorationWell!.Remove(explorationWell);
                    affectedAssets[nameof(Exploration)].Add(explorationWell.ExplorationId);
                }
                var wellProjectWells = _context.WellProjectWell!.Where(ew => ew.WellId == well.Id);
                foreach (var wellProjectWell in wellProjectWells)
                {
                    _context.WellProjectWell!.Remove(wellProjectWell);
                    affectedAssets[nameof(WellProject)].Add(wellProjectWell.WellProjectId);
                }
                _context.Wells.Remove(well);
            }
        }
        await _context.SaveChangesAsync();
        foreach (var explorationId in affectedAssets[nameof(Exploration)])
        {
            await _costProfileFromDrillingScheduleHelper.UpdateExplorationCostProfiles(explorationId);
        }
        foreach (var wellProjectId in affectedAssets[nameof(WellProject)])
        {
            await _costProfileFromDrillingScheduleHelper.UpdateWellProjectCostProfiles(wellProjectId);
        }
    }

    private async Task<(ExplorationWithProfilesDto explorationDto, WellProjectWithProfilesDto wellProjectDto)?> CreateAndUpdateWells(
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
                var existing = await GetWell(wellDto.Id);
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
        if (updatedWells.Count != 0)
        {
            await _costProfileFromDrillingScheduleHelper.UpdateCostProfilesForWells(updatedWells);
        }
        return null;
    }

    private async Task<Well> GetWell(Guid wellId)
    {
        var well = await _context.Wells!
            .Include(e => e.WellProjectWells)
            .Include(e => e.ExplorationWells)
            .FirstOrDefaultAsync(w => w.Id == wellId);
        if (well == null)
        {
            throw new ArgumentException(string.Format("Well {0} not found.", wellId));
        }
        return well;
    }
    private async Task<ProjectDto> UpdateProject(Project project, UpdateProjectDto updatedDto)
    {
        _mapper.Map(updatedDto, project);
        var updatedItem = _context.Projects!.Update(project);
        await _context.SaveChangesAsync();

        var projectDto = _mapper.Map<ProjectDto>(updatedItem.Entity, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());
        if (projectDto == null)
        {
            _logger.LogError("Failed to map project to dto");
            throw new Exception("Failed to map project to dto");
        }
        await _repository.UpdateModifyTime(project.Id);
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
