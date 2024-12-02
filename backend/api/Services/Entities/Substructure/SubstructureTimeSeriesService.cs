using api.Dtos;
using api.Exceptions;
using api.Features.ProjectAccess;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SubstructureTimeSeriesService(
    ILogger<SubstructureService> logger,
    ISubstructureRepository substructureRepository,
    ISubstructureTimeSeriesRepository repository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : ISubstructureTimeSeriesService
{
    public async Task<SubstructureCostProfileDto> AddOrUpdateSubstructureCostProfile(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Substructure>(projectId, substructureId);

        var substructure = await substructureRepository.GetSubstructureWithCostProfile(substructureId)
            ?? throw new NotFoundInDBException($"Substructure with id {substructureId} not found.");

        if (substructure.CostProfile != null)
        {
            return await UpdateSubstructureCostProfile(projectId, caseId, substructureId, substructure.CostProfile.Id, dto);
        }

        return await CreateSubstructureCostProfile(caseId, substructureId, dto, substructure);
    }

    private async Task<SubstructureCostProfileDto> UpdateSubstructureCostProfile(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        Guid profileId,
        UpdateSubstructureCostProfileDto dto
    )
    {
        return await UpdateSubstructureTimeSeries<SubstructureCostProfile, SubstructureCostProfileDto, UpdateSubstructureCostProfileDto>(
            projectId,
            caseId,
            substructureId,
            profileId,
            dto,
            repository.GetSubstructureCostProfile,
            repository.UpdateSubstructureCostProfile
        );
    }

    private async Task<SubstructureCostProfileDto> CreateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto,
        Substructure substructure
    )
    {
        SubstructureCostProfile substructureCostProfile = new SubstructureCostProfile
        {
            Substructure = substructure
        };

        var newProfile = mapperService.MapToEntity(dto, substructureCostProfile, substructureId);
        if (newProfile.Substructure.CostProfileOverride != null)
        {
            newProfile.Substructure.CostProfileOverride.Override = false;
        }
        try
        {
            repository.CreateSubstructureCostProfile(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create cost profile for substructure with id {substructureId} for case id {caseId}.", substructureId, caseId);
            throw;
        }

        var newDto = mapperService.MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    public async Task<SubstructureCostProfileOverrideDto> CreateSubstructureCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        CreateSubstructureCostProfileOverrideDto dto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Substructure>(projectId, substructureId);

        var substructure = await substructureRepository.GetSubstructure(substructureId)
            ?? throw new NotFoundInDBException($"Substructure with id {substructureId} not found.");

        var resourceHasProfile = await substructureRepository.SubstructureHasCostProfileOverride(substructureId);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Substructure with id {substructureId} already has a profile of type {nameof(SubstructureCostProfileOverride)}.");
        }

        SubstructureCostProfileOverride profile = new()
        {
            Substructure = substructure,
        };

        var newProfile = mapperService.MapToEntity(dto, profile, substructureId);

        SubstructureCostProfileOverride createdProfile;
        try
        {
            createdProfile = repository.CreateSubstructureCostProfileOverride(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to create profile SubstructureCostProfileOverride for case id {caseId}.", caseId);
            throw;
        }

        var updatedDto = mapperService.MapToDto<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }

    public async Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        Guid costProfileId,
        UpdateSubstructureCostProfileOverrideDto dto
    )
    {
        return await UpdateSubstructureTimeSeries<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto, UpdateSubstructureCostProfileOverrideDto>(
        projectId,
        caseId,
        substructureId,
        costProfileId,
        dto,
        repository.GetSubstructureCostProfileOverride,
        repository.UpdateSubstructureCostProfileOverride
    );
    }

    private async Task<TDto> UpdateSubstructureTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, ISubstructureTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Substructure>(projectId, existingProfile.Substructure.Id);

        if (existingProfile.Substructure.ProspVersion == null)
        {
            if (existingProfile.Substructure.CostProfileOverride != null)
            {
                existingProfile.Substructure.CostProfileOverride.Override = true;
            }
        }
        mapperService.MapToEntity(updatedProfileDto, existingProfile, substructureId);

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
}
