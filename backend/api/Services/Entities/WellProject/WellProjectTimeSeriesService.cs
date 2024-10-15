using api.Context;
using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectTimeSeriesService : IWellProjectTimeSeriesService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly IProjectAccessService _projectAccessService;
    private readonly ILogger<WellProjectService> _logger;
    private readonly IMapper _mapper;
    private readonly IWellProjectTimeSeriesRepository _repository;
    private readonly IWellProjectRepository _wellProjectRepository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;

    public WellProjectTimeSeriesService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        IWellProjectTimeSeriesRepository repository,
        IWellProjectRepository wellProjectRepository,
        ICaseRepository caseRepository,
        IMapperService mapperService,
        IProjectAccessService projectAccessService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<WellProjectService>();
        _mapper = mapper;
        _repository = repository;
        _wellProjectRepository = wellProjectRepository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
        _projectAccessService = projectAccessService;
    }

    public async Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateOilProducerCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto, UpdateOilProducerCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetOilProducerCostProfileOverride,
            _repository.UpdateOilProducerCostProfileOverride
        );
    }

    public async Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasProducerCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto, UpdateGasProducerCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetGasProducerCostProfileOverride,
            _repository.UpdateGasProducerCostProfileOverride
        );
    }

    public async Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateWaterInjectorCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto, UpdateWaterInjectorCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetWaterInjectorCostProfileOverride,
            _repository.UpdateWaterInjectorCostProfileOverride
        );
    }

    public async Task<GasInjectorCostProfileOverrideDto> UpdateGasInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasInjectorCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<GasInjectorCostProfileOverride, GasInjectorCostProfileOverrideDto, UpdateGasInjectorCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetGasInjectorCostProfileOverride,
            _repository.UpdateGasInjectorCostProfileOverride
        );
    }

    public async Task<OilProducerCostProfileOverrideDto> CreateOilProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateOilProducerCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateWellProjectProfile<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto, CreateOilProducerCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            createProfileDto,
            _repository.CreateOilProducerCostProfileOverride,
            WellProjectProfileNames.OilProducerCostProfileOverride
        );
    }

    public async Task<GasProducerCostProfileOverrideDto> CreateGasProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateGasProducerCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateWellProjectProfile<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto, CreateGasProducerCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            createProfileDto,
            _repository.CreateGasProducerCostProfileOverride,
            WellProjectProfileNames.GasProducerCostProfileOverride
        );
    }

    public async Task<WaterInjectorCostProfileOverrideDto> CreateWaterInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateWaterInjectorCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateWellProjectProfile<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto, CreateWaterInjectorCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            createProfileDto,
            _repository.CreateWaterInjectorCostProfileOverride,
            WellProjectProfileNames.WaterInjectorCostProfileOverride
        );
    }

    public async Task<GasInjectorCostProfileOverrideDto> CreateGasInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateGasInjectorCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateWellProjectProfile<GasInjectorCostProfileOverride, GasInjectorCostProfileOverrideDto, CreateGasInjectorCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            createProfileDto,
            _repository.CreateGasInjectorCostProfileOverride,
            WellProjectProfileNames.GasInjectorCostProfileOverride
        );
    }

    private async Task<TDto> UpdateWellProjectCostProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
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

        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<WellProject>(projectId, existingProfile.WellProject.Id);

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, wellProjectId);

        try
        {
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            _logger.LogError(ex, "Failed to update profile {profileName} with id {profileId} for case id {caseId}.", profileName, profileId, caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
        return updatedDto;
    }

    private async Task<TDto> CreateWellProjectProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
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
        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<WellProject>(projectId, wellProjectId);

        var wellProject = await _wellProjectRepository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDBException($"Well project with id {wellProjectId} not found.");

        var resourceHasProfile = await _wellProjectRepository.WellProjectHasProfile(wellProjectId, profileName);

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
            await _repository.SaveChangesAndRecalculateAsync(caseId);
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
