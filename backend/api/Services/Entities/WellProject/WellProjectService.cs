using api.Context;
using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectService : IWellProjectService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<WellProjectService> _logger;
    private readonly IMapper _mapper;
    private readonly IWellProjectRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;

    public WellProjectService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        IWellProjectRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<WellProjectService>();
        _mapper = mapper;
        _repository = repository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
    }

    public async Task<WellProject> CreateWellProject(Guid projectId, Guid sourceCaseId, CreateWellProjectDto wellProjectDto)
    {
        var wellProject = _mapper.Map<WellProject>(wellProjectDto);
        if (wellProject == null)
        {
            throw new ArgumentNullException(nameof(wellProject));
        }
        var project = await _projectService.GetProject(projectId);
        wellProject.Project = project;
        var createdWellProject = _context.WellProjects!.Add(wellProject);
        await _context.SaveChangesAsync();
        await SetCaseLink(wellProject, sourceCaseId, project);
        return createdWellProject.Entity;
    }

    private async Task SetCaseLink(WellProject wellProject, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.WellProjectLink = wellProject.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<WellProject> GetWellProject(Guid wellProjectId)
    {
        var wellProject = await _context.WellProjects!
            .Include(c => c.OilProducerCostProfile)
            .Include(c => c.OilProducerCostProfileOverride)
            .Include(c => c.GasProducerCostProfile)
            .Include(c => c.GasProducerCostProfileOverride)
            .Include(c => c.WaterInjectorCostProfile)
            .Include(c => c.WaterInjectorCostProfileOverride)
            .Include(c => c.GasInjectorCostProfile)
            .Include(c => c.GasInjectorCostProfileOverride)
            .FirstOrDefaultAsync(o => o.Id == wellProjectId);
        if (wellProject == null)
        {
            throw new ArgumentException(string.Format("Well project {0} not found.", wellProjectId));
        }
        return wellProject;
    }

    public async Task<WellProjectDto> UpdateWellProject(
        Guid caseId,
        Guid wellProjectId,
        UpdateWellProjectDto updatedWellProjectDto
    )
    {
        var existingWellProject = await _repository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDBException($"Well project with id {wellProjectId} not found.");

        _mapperService.MapToEntity(updatedWellProjectDto, existingWellProject, wellProjectId);

        // WellProject updatedWellProject;
        try
        {
            // updatedWellProject = _repository.UpdateWellProject(existingWellProject);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update well project with id {wellProjectId} for case id {caseId}.", wellProjectId, caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<WellProject, WellProjectDto>(existingWellProject, wellProjectId);
        return dto;
    }

    public async Task<DrillingScheduleDto> UpdateWellProjectWellDrillingSchedule(
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateDrillingScheduleDto updatedWellProjectWellDto
    )
    {
        var existingDrillingSchedule = await _repository.GetWellProjectWellDrillingSchedule(drillingScheduleId)
            ?? throw new NotFoundInDBException($"Drilling schedule with {drillingScheduleId} not found.");

        _mapperService.MapToEntity(updatedWellProjectWellDto, existingDrillingSchedule, drillingScheduleId);

        DrillingSchedule updatedDrillingSchedule;
        try
        {
            updatedDrillingSchedule = _repository.UpdateWellProjectWellDrillingSchedule(existingDrillingSchedule);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update drilling schedule with id {drillingScheduleId}", drillingScheduleId);
            throw;
        }

        var dto = _mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(updatedDrillingSchedule, drillingScheduleId);
        return dto;
    }

    public async Task<DrillingScheduleDto> CreateWellProjectWellDrillingSchedule(
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        CreateDrillingScheduleDto updatedWellProjectWellDto
    )
    {
        var existingWellProject = await _repository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDBException($"Well project with {wellProjectId} not found.");

        var existingWell = await _repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with {wellId} not found.");

        DrillingSchedule drillingSchedule = new();
        var newDrillingSchedule = _mapperService.MapToEntity(updatedWellProjectWellDto, drillingSchedule, wellProjectId);

        WellProjectWell newWellProjectWell = new()
        {
            Well = existingWell,
            WellProject = existingWellProject,
            DrillingSchedule = newDrillingSchedule
        };

        WellProjectWell createdWellProjectWell;
        try
        {
            createdWellProjectWell = _repository.CreateWellProjectWellDrillingSchedule(newWellProjectWell);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update drilling schedule with id {drillingScheduleId}", wellProjectId);
            throw;
        }

        if (createdWellProjectWell.DrillingSchedule == null)
        {
            // TODO: use a more specific exception
            throw new Exception(nameof(createdWellProjectWell.DrillingSchedule));
        }

        var dto = _mapperService.MapToDto<DrillingSchedule, DrillingScheduleDto>(createdWellProjectWell.DrillingSchedule, wellProjectId);
        return dto;
    }
}
