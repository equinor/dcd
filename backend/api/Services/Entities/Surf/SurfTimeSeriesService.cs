using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SurfTimeSeriesService(
    ILogger<SurfService> logger,
    ISurfTimeSeriesRepository repository,
    ISurfRepository surfRepository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : ISurfTimeSeriesService
{
    public async Task<SurfCostProfileOverrideDto> CreateSurfCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        CreateSurfCostProfileOverrideDto dto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Surf>(projectId, surfId);

        var surf = await surfRepository.GetSurf(surfId)
            ?? throw new NotFoundInDBException($"Surf with id {surfId} not found.");

        var resourceHasProfile = await surfRepository.SurfHasCostProfileOverride(surfId);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Surf with id {surfId} already has a profile of type {nameof(SurfCostProfileOverride)}.");
        }

        SurfCostProfileOverride profile = new()
        {
            Surf = surf,
        };

        var newProfile = mapperService.MapToEntity(dto, profile, surfId);

        SurfCostProfileOverride createdProfile;
        try
        {
            createdProfile = repository.CreateSurfCostProfileOverride(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to create profile SurfCostProfileOverride for case id {caseId}.", caseId);
            throw;
        }

        var updatedDto = mapperService.MapToDto<SurfCostProfileOverride, SurfCostProfileOverrideDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }

    public async Task<SurfCostProfileDto> AddOrUpdateSurfCostProfile(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Surf>(projectId, surfId);

        var surf = await surfRepository.GetSurfWithCostProfile(surfId)
            ?? throw new NotFoundInDBException($"Surf with id {surfId} not found.");

        if (surf.CostProfile != null)
        {
            return await UpdateSurfCostProfile(projectId, caseId, surfId, surf.CostProfile.Id, dto);
        }

        return await CreateSurfCostProfile(caseId, surfId, dto, surf);
    }

    private async Task<SurfCostProfileDto> UpdateSurfCostProfile(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        Guid profileId,
        UpdateSurfCostProfileDto dto
    )
    {
        return await UpdateSurfTimeSeries<SurfCostProfile, SurfCostProfileDto, UpdateSurfCostProfileDto>(
            projectId,
            caseId,
            surfId,
            profileId,
            dto,
            repository.GetSurfCostProfile,
            repository.UpdateSurfCostProfile
        );
    }

    private async Task<SurfCostProfileDto> CreateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto,
        Surf surf
    )
    {
        SurfCostProfile surfCostProfile = new SurfCostProfile
        {
            Surf = surf
        };

        var newProfile = mapperService.MapToEntity(dto, surfCostProfile, surfId);

        if (newProfile.Surf.CostProfileOverride != null)
        {
            newProfile.Surf.CostProfileOverride.Override = false;
        }

        try
        {
            repository.CreateSurfCostProfile(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create cost profile for surf with id {surfId} for case id {caseId}.", surfId, caseId);
            throw;
        }

        var newDto = mapperService.MapToDto<SurfCostProfile, SurfCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    public async Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto
    )
    {
        return await UpdateSurfTimeSeries<SurfCostProfileOverride, SurfCostProfileOverrideDto, UpdateSurfCostProfileOverrideDto>(
            projectId,
            caseId,
            surfId,
            costProfileId,
            updatedSurfCostProfileOverrideDto,
            repository.GetSurfCostProfileOverride,
            repository.UpdateSurfCostProfileOverride
        );
    }

    private async Task<TDto> UpdateSurfTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, ISurfTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Surf>(projectId, existingProfile.Surf.Id);

        if (existingProfile.Surf.ProspVersion == null)
        {
            if (existingProfile.Surf.CostProfileOverride != null)
            {
                existingProfile.Surf.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, surfId);

        // TProfile updatedProfile;
        try
        {
            // updatedProfile = updateProfile(existingProfile);
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
}
