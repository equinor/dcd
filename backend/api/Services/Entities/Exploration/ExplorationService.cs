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
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<ExplorationService>();
        _mapper = mapper;
        _caseRepository = caseRepository;
        _repository = repository;
        _mapperService = mapperService;
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
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto
    )
    {
        var existingExploration = await _repository.GetExploration(explorationId)
            ?? throw new NotFoundInDBException($"Exploration with id {explorationId} not found.");

        _mapperService.MapToEntity(updatedExplorationDto, existingExploration, explorationId);

        Exploration updatedExploration;
        try
        {
            updatedExploration = _repository.UpdateExploration(existingExploration);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update exploration with id {explorationId} for case id {caseId}.", explorationId, caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<Exploration, ExplorationDto>(updatedExploration, explorationId);
        return dto;
    }

    public async Task<DrillingScheduleDto> UpdateExplorationWellDrillingSchedule(
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateDrillingScheduleDto updatedExplorationWellDto
    )
    {
        var existingDrillingSchedule = await _repository.GetExplorationWellDrillingSchedule(drillingScheduleId)
            ?? throw new NotFoundInDBException($"Drilling schedule with {drillingScheduleId} not found.");

        _mapperService.MapToEntity(updatedExplorationWellDto, existingDrillingSchedule, drillingScheduleId);

        DrillingSchedule updatedDrillingSchedule;
        try
        {
            updatedDrillingSchedule = _repository.UpdateExplorationWellDrillingSchedule(existingDrillingSchedule);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update drilling schedule with id {drillingScheduleId}", drillingScheduleId);
            throw;
        }

        var dto = _mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(updatedDrillingSchedule, drillingScheduleId);
        return dto;
    }

    public async Task<DrillingScheduleDto> CreateExplorationWellDrillingSchedule(
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
            await _repository.SaveChangesAsync();
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
