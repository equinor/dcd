using api.Context;
using api.Context.Recalculation;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Create;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;
using api.Features.Assets.CaseAssets.Topsides.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Topsides.Services;

public class TopsideTimeSeriesService(
    ILogger<TopsideService> logger,
    ITopsideTimeSeriesRepository repository,
    ITopsideRepository topsideRepository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService,
    IRecalculationService recalculationService)
    : ITopsideTimeSeriesService
{
    public async Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        CreateTopsideCostProfileOverrideDto dto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Topside>(projectId, topsideId);

        var topside = await topsideRepository.GetTopside(topsideId)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");

        var resourceHasProfile = await topsideRepository.TopsideHasCostProfileOverride(topsideId);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Topside with id {topsideId} already has a profile of type {nameof(TopsideCostProfileOverride)}.");
        }

        TopsideCostProfileOverride profile = new()
        {
            Topside = topside,
        };

        var newProfile = mapperService.MapToEntity(dto, profile, topsideId);

        TopsideCostProfileOverride createdProfile;
        try
        {
            createdProfile = repository.CreateTopsideCostProfileOverride(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await recalculationService.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to create profile TopsideCostProfileOverride for case id {caseId}.", caseId);
            throw;
        }

        var updatedDto = mapperService.MapToDto<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }

    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid costProfileId,
        UpdateTopsideCostProfileOverrideDto dto
    )
    {
        return await UpdateTopsideTimeSeries<TopsideCostProfileOverride, TopsideCostProfileOverrideDto, UpdateTopsideCostProfileOverrideDto>(
            projectId,
            caseId,
            topsideId,
            costProfileId,
            dto,
            repository.GetTopsideCostProfileOverride,
            repository.UpdateTopsideCostProfileOverride
        );
    }

    public async Task<TopsideCostProfileDto> AddOrUpdateTopsideCostProfile(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Topside>(projectId, topsideId);

        var topside = await topsideRepository.GetTopsideWithCostProfile(topsideId)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");

        if (topside.CostProfile != null)
        {
            return await UpdateTopsideCostProfile(projectId, caseId, topsideId, topside.CostProfile.Id, dto);
        }

        return await CreateTopsideCostProfile(caseId, topsideId, dto, topside);
    }

    private async Task<TopsideCostProfileDto> UpdateTopsideCostProfile(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        UpdateTopsideCostProfileDto dto
    )
    {
        return await UpdateTopsideTimeSeries<TopsideCostProfile, TopsideCostProfileDto, UpdateTopsideCostProfileDto>(
            projectId,
            caseId,
            topsideId,
            profileId,
            dto,
            repository.GetTopsideCostProfile,
            repository.UpdateTopsideCostProfile
        );
    }

    private async Task<TopsideCostProfileDto> CreateTopsideCostProfile(
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto,
        Topside topside
    )
    {
        TopsideCostProfile topsideCostProfile = new()
        {
            Topside = topside
        };

        var newProfile = mapperService.MapToEntity(dto, topsideCostProfile, topsideId);
        if (newProfile.Topside.CostProfileOverride != null)
        {
            newProfile.Topside.CostProfileOverride.Override = false;
        }
        try
        {
            repository.CreateTopsideCostProfile(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await recalculationService.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create cost profile for topside with id {topsideId} for case id {caseId}.", topsideId, caseId);
            throw;
        }

        var newDto = mapperService.MapToDto<TopsideCostProfile, TopsideCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    private async Task<TDto> UpdateTopsideTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, ITopsideTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Topside>(projectId, existingProfile.Topside.Id);

        if (existingProfile.Topside.ProspVersion == null)
        {
            if (existingProfile.Topside.CostProfileOverride != null)
            {
                existingProfile.Topside.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, topsideId);

        try
        {
            await caseRepository.UpdateModifyTime(caseId);
            await recalculationService.SaveChangesAndRecalculateAsync(caseId);
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
