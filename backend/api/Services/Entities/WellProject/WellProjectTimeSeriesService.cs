using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Features.TechnicalInput.Dtos;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectTimeSeriesService(
    ILogger<WellProjectService> logger,
    IWellProjectTimeSeriesRepository repository,
    IWellProjectRepository wellProjectRepository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : IWellProjectTimeSeriesService
{
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
            repository.GetOilProducerCostProfileOverride,
            repository.UpdateOilProducerCostProfileOverride
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
            repository.GetGasProducerCostProfileOverride,
            repository.UpdateGasProducerCostProfileOverride
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
            repository.GetWaterInjectorCostProfileOverride,
            repository.UpdateWaterInjectorCostProfileOverride
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
            repository.GetGasInjectorCostProfileOverride,
            repository.UpdateGasInjectorCostProfileOverride
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
            repository.CreateOilProducerCostProfileOverride,
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
            repository.CreateGasProducerCostProfileOverride,
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
            repository.CreateWaterInjectorCostProfileOverride,
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
            repository.CreateGasInjectorCostProfileOverride,
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
        await projectAccessService.ProjectExists<WellProject>(projectId, existingProfile.WellProject.Id);

        mapperService.MapToEntity(updatedProfileDto, existingProfile, wellProjectId);

        try
        {
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            logger.LogError(ex, "Failed to update profile {profileName} with id {profileId} for case id {caseId}.", profileName, profileId, caseId);
            throw;
        }

        var updatedDto = mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
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
        await projectAccessService.ProjectExists<WellProject>(projectId, wellProjectId);

        var wellProject = await wellProjectRepository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDBException($"Well project with id {wellProjectId} not found.");

        var resourceHasProfile = await wellProjectRepository.WellProjectHasProfile(wellProjectId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Well project with id {wellProjectId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            WellProject = wellProject,
        };

        var newProfile = mapperService.MapToEntity(createWellProjectProfileDto, profile, wellProjectId);

        TProfile createdProfile;
        try
        {
            createdProfile = createProfile(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to create profile {profileName} for case id {caseId}.", profileName, caseId);
            throw;
        }

        var updatedDto = mapperService.MapToDto<TProfile, TDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }
}
