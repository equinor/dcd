using api.Exceptions;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Create;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Update;
using api.Features.Assets.CaseAssets.Surfs.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Services;

public class SurfTimeSeriesService(
    ISurfTimeSeriesRepository repository,
    ISurfRepository surfRepository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
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
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, surfId);

        var surf = await surfRepository.GetSurf(surfId)
            ?? throw new NotFoundInDbException($"Surf with id {surfId} not found.");

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

        var createdProfile = repository.CreateSurfCostProfileOverride(newProfile);
        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

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
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, surfId);

        var surf = await surfRepository.GetSurfWithCostProfile(surfId)
            ?? throw new NotFoundInDbException($"Surf with id {surfId} not found.");

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

        repository.CreateSurfCostProfile(newProfile);
        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

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
            ?? throw new NotFoundInDbException($"Cost profile with id {profileId} not found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, existingProfile.Surf.Id);

        if (existingProfile.Surf.ProspVersion == null)
        {
            if (existingProfile.Surf.CostProfileOverride != null)
            {
                existingProfile.Surf.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, surfId);

        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var updatedDto = mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
        return updatedDto;
    }
}
