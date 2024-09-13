using api.Context;
using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationService : IExplorationService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly IProjectAccessService _projectAccessService;
    private readonly ILogger<ExplorationService> _logger;
    private readonly IMapper _mapper;
    private readonly ICaseRepository _caseRepository;
    private readonly IExplorationRepository _repository;
    private readonly IMapperService _mapperService;

    public ExplorationService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ICaseRepository caseRepository,
        IExplorationRepository repository,
        IMapperService mapperService,
        IProjectAccessService projectAccessService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<ExplorationService>();
        _mapper = mapper;
        _caseRepository = caseRepository;
        _repository = repository;
        _mapperService = mapperService;
        _projectAccessService = projectAccessService;
    }

    public async Task<Exploration> CreateExploration(Guid projectId, Guid sourceCaseId, CreateExplorationDto explorationDto)
    {
        var exploration = _mapper.Map<Exploration>(explorationDto);
        if (exploration == null)
        {
            throw new ArgumentNullException(nameof(exploration));
        }
        var project = await _projectService.GetProject(projectId);
        exploration.Project = project;
        var createdExploration = _context.Explorations!.Add(exploration);
        SetCaseLink(exploration, sourceCaseId, project);
        await _context.SaveChangesAsync();
        return createdExploration.Entity;
    }

    private static void SetCaseLink(Exploration exploration, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.ExplorationLink = exploration.Id;
    }

    public async Task<Exploration> GetExploration(Guid explorationId)
    {
        var exploration = await _context.Explorations!
            .Include(c => c.ExplorationWellCostProfile)
            .Include(c => c.AppraisalWellCostProfile)
            .Include(c => c.SidetrackCostProfile)
            .Include(c => c.GAndGAdminCost)
            .Include(c => c.GAndGAdminCostOverride)
            .Include(c => c.SeismicAcquisitionAndProcessing)
            .Include(c => c.CountryOfficeCost)
            .FirstOrDefaultAsync(o => o.Id == explorationId);
        if (exploration == null)
        {
            throw new ArgumentException(string.Format("Exploration {0} not found.", explorationId));
        }
        return exploration;
    }

    public async Task<ExplorationDto> UpdateExploration(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the exploration
        await _projectAccessService.ProjectExists<Exploration>(projectId, explorationId);

        var existingExploration = await _repository.GetExploration(explorationId)
            ?? throw new NotFoundInDBException($"Exploration with id {explorationId} not found.");

        _mapperService.MapToEntity(updatedExplorationDto, existingExploration, explorationId);

        try
        {
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update exploration with id {explorationId} for case id {caseId}.", explorationId, caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<Exploration, ExplorationDto>(existingExploration, explorationId);
        return dto;
    }

    public async Task<DrillingScheduleDto> UpdateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateDrillingScheduleDto updatedExplorationWellDto
    )
    {
        var existingExploration = await _repository.GetExplorationWithDrillingSchedule(drillingScheduleId)
            ?? throw new NotFoundInDBException($"No exploration connected to {drillingScheduleId} found.");

        // Need to verify that the project from the URL is the same as the project of the exploration
        await _projectAccessService.ProjectExists<Exploration>(projectId, existingExploration.Id);

        var existingDrillingSchedule = existingExploration.ExplorationWells?.FirstOrDefault(w => w.WellId == wellId)?.DrillingSchedule
            ?? throw new NotFoundInDBException($"Drilling schedule with id {drillingScheduleId} not found.");

        _mapperService.MapToEntity(updatedExplorationWellDto, existingDrillingSchedule, drillingScheduleId);

        try
        {
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update drilling schedule with id {drillingScheduleId}", drillingScheduleId);
            throw;
        }

        var dto = _mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(existingDrillingSchedule, drillingScheduleId);
        return dto;
    }

    public async Task<DrillingScheduleDto> CreateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        CreateDrillingScheduleDto updatedExplorationWellDto
    )
    {
        var existingExploration = await _repository.GetExploration(explorationId)
            ?? throw new NotFoundInDBException($"Well project with {explorationId} not found.");

        var existingWell = await _repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with {wellId} not found.");

        DrillingSchedule drillingSchedule = new();
        var newDrillingSchedule = _mapperService.MapToEntity(updatedExplorationWellDto, drillingSchedule, explorationId);

        ExplorationWell newExplorationWell = new()
        {
            Well = existingWell,
            Exploration = existingExploration,
            DrillingSchedule = newDrillingSchedule
        };

        ExplorationWell createdExplorationWell;
        try
        {
            createdExplorationWell = _repository.CreateExplorationWellDrillingSchedule(newExplorationWell);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update drilling schedule with id {drillingScheduleId}", explorationId);
            throw;
        }

        if (createdExplorationWell.DrillingSchedule == null)
        {
            // TODO: use a more specific exception
            throw new Exception(nameof(createdExplorationWell.DrillingSchedule));
        }

        var dto = _mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(createdExplorationWell.DrillingSchedule, explorationId);
        return dto;
    }
}
