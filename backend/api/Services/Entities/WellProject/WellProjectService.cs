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

        WellProject updatedWellProject;
        try
        {
            updatedWellProject = _repository.UpdateWellProject(existingWellProject);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update well project with id {wellProjectId} for case id {caseId}.", wellProjectId, caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<WellProject, WellProjectDto>(updatedWellProject, wellProjectId);
        return dto;
    }

    public async Task<WellProjectWellDto> UpdateWellProjectWell(
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        UpdateWellProjectWellDto updatedWellProjectWellDto
    )
    {
        var existingWellProject = await _repository.GetWellProjectWell(wellProjectId, wellId)
            ?? throw new NotFoundInDBException($"Well project well with id {wellProjectId} and ${wellId} not found.");

        _mapperService.MapToEntity(updatedWellProjectWellDto, existingWellProject, wellProjectId);

        WellProjectWell updatedWellProject;
        try
        {
            updatedWellProject = _repository.UpdateWellProjectWell(existingWellProject);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update well project well with id {wellProjectId} and well id {wellId}.", wellProjectId, wellId);
            throw;
        }

        var dto = _mapperService.MapToDto<WellProjectWell, WellProjectWellDto>(updatedWellProject, wellProjectId);
        return dto;
    }

    public async Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateOilProducerCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto, UpdateOilProducerCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetOilProducerCostProfileOverride,
            _repository.UpdateOilProducerCostProfileOverride
        );
    }

    public async Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasProducerCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto, UpdateGasProducerCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetGasProducerCostProfileOverride,
            _repository.UpdateGasProducerCostProfileOverride
        );
    }

    public async Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateWaterInjectorCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto, UpdateWaterInjectorCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetWaterInjectorCostProfileOverride,
            _repository.UpdateWaterInjectorCostProfileOverride
        );
    }

    public async Task<GasInjectorCostProfileOverrideDto> UpdateGasInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasInjectorCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<GasInjectorCostProfileOverride, GasInjectorCostProfileOverrideDto, UpdateGasInjectorCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetGasInjectorCostProfileOverride,
            _repository.UpdateGasInjectorCostProfileOverride
        );
    }

    public async Task<OilProducerCostProfileOverrideDto> CreateOilProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        CreateOilProducerCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateWellProjectProfile<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto, CreateOilProducerCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            createProfileDto,
            _repository.CreateOilProducerCostProfileOverride,
            WellProjectProfileNames.OilProducerCostProfileOverride
        );
    }

    public async Task<GasProducerCostProfileOverrideDto> CreateGasProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        CreateGasProducerCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateWellProjectProfile<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto, CreateGasProducerCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            createProfileDto,
            _repository.CreateGasProducerCostProfileOverride,
            WellProjectProfileNames.GasProducerCostProfileOverride
        );
    }

    public async Task<WaterInjectorCostProfileOverrideDto> CreateWaterInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        CreateWaterInjectorCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateWellProjectProfile<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto, CreateWaterInjectorCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            createProfileDto,
            _repository.CreateWaterInjectorCostProfileOverride,
            WellProjectProfileNames.WaterInjectorCostProfileOverride
        );
    }

    public async Task<GasInjectorCostProfileOverrideDto> CreateGasInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        CreateGasInjectorCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateWellProjectProfile<GasInjectorCostProfileOverride, GasInjectorCostProfileOverrideDto, CreateGasInjectorCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            createProfileDto,
            _repository.CreateGasInjectorCostProfileOverride,
            WellProjectProfileNames.GasInjectorCostProfileOverride
        );
    }

    private async Task<TDto> UpdateWellProjectCostProfile<TProfile, TDto, TUpdateDto>(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, IWellProjectTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, wellProjectId);

        TProfile updatedProfile;
        try
        {
            updatedProfile = updateProfile(existingProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            _logger.LogError(ex, "Failed to update profile {profileName} with id {profileId} for case id {caseId}.", profileName, profileId, caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(updatedProfile, profileId);
        return updatedDto;
    }

    private async Task<TDto> CreateWellProjectProfile<TProfile, TDto, TCreateDto>(
        Guid caseId,
        Guid wellProjectId,
        TCreateDto createWellProjectProfileDto,
        Func<TProfile, TProfile> createProfile,
        WellProjectProfileNames profileName
    )
        where TProfile : class, IWellProjectTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        var wellProject = await _repository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDBException($"Well project with id {wellProjectId} not found.");

        var resourceHasProfile = await _repository.WellProjectHasProfile(wellProjectId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Well project with id {wellProjectId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            WellProject = wellProject,
        };

        var newProfile = _mapperService.MapToEntity(createWellProjectProfileDto, profile, wellProjectId);

        TProfile createdProfile;
        try
        {
            createdProfile = createProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create profile {profileName} for case id {caseId}.", profileName, caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }
}