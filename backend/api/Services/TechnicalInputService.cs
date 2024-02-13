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
            var project = await _projectService.GetProject(technicalInputDto.ProjectDto.Id);
            var projectDto = _mapper.Map<ProjectDto>(project);
            if (projectDto == null)
            {
                _logger.LogError("Failed to map project to dto");
                throw new Exception("Failed to map project to dto");
            }
            technicalInputDto.ProjectDto = projectDto;
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
                var well = _mapper.Map<Well>(wellDto);
                if (well == null)
                {
                    throw new ArgumentNullException(nameof(well));
                }
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
                    _mapper.Map(wellDto, existing);

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

                var explorationDto = _mapper.Map<ExplorationDto>(exploration);
                var wellProjectDto = _mapper.Map<WellProjectDto>(wellProject);
                if (explorationDto == null || wellProjectDto == null)
                {
                    _logger.LogError("Failed to map exploration or well project to dto");
                    throw new Exception("Failed to map exploration or well project to dto");
                }

                return (explorationDto, wellProjectDto);
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
        var item = await _projectService.GetProject(updatedDto.Id);
        _mapper.Map(updatedDto, item);
        var updatedItem = _context.Projects!.Update(item);
        await _context.SaveChangesAsync();

        var projectDto = _mapper.Map<ProjectDto>(updatedItem.Entity);
        if (projectDto == null)
        {
            _logger.LogError("Failed to map project to dto");
            throw new Exception("Failed to map project to dto");
        }

        return projectDto;
    }

    public async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts(ExplorationOperationalWellCostsDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = await _explorationOperationalWellCostsService.GetOperationalWellCosts(updatedDto.Id) ?? new ExplorationOperationalWellCosts();
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

    public async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts(DevelopmentOperationalWellCostsDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = await _developmentOperationalWellCostsService.GetOperationalWellCosts(updatedDto.Id) ?? new DevelopmentOperationalWellCosts();
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
