using api.Exceptions;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Dtos.Create;
using api.Features.Assets.CaseAssets.Explorations.Repositories;
using api.Features.CaseProfiles.Enums;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.Features.TechnicalInput.Dtos;
using api.Models;
using api.Services;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Explorations.Services;

public class ExplorationTimeSeriesService(
    ILogger<ExplorationService> logger,
    ICaseRepository caseRepository,
    IExplorationTimeSeriesRepository repository,
    IExplorationRepository explorationRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : IExplorationTimeSeriesService
{
    public async Task<GAndGAdminCostOverrideDto> CreateGAndGAdminCostOverride(
        Guid projectId,
            Guid caseId,
            Guid explorationId,
            CreateGAndGAdminCostOverrideDto createProfileDto
        )
    {
        return await CreateExplorationProfile<GAndGAdminCostOverride, GAndGAdminCostOverrideDto, CreateGAndGAdminCostOverrideDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            repository.CreateGAndGAdminCostOverride,
            ExplorationProfileNames.GAndGAdminCostOverride
        );
    }
    public async Task<GAndGAdminCostOverrideDto> UpdateGAndGAdminCostOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGAndGAdminCostOverrideDto updateDto
    )
    {
        return await UpdateExplorationCostProfile<GAndGAdminCostOverride, GAndGAdminCostOverrideDto, UpdateGAndGAdminCostOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            repository.GetGAndGAdminCostOverride,
            repository.UpdateGAndGAdminCostOverride
        );
    }
    public async Task<SeismicAcquisitionAndProcessingDto> UpdateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateSeismicAcquisitionAndProcessingDto updateDto
    )
    {
        return await UpdateExplorationCostProfile<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto, UpdateSeismicAcquisitionAndProcessingDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            repository.GetSeismicAcquisitionAndProcessing,
            repository.UpdateSeismicAcquisitionAndProcessing
        );
    }

    public async Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateCountryOfficeCostDto updateDto
    )
    {
        return await UpdateExplorationCostProfile<CountryOfficeCost, CountryOfficeCostDto, UpdateCountryOfficeCostDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            repository.GetCountryOfficeCost,
            repository.UpdateCountryOfficeCost
        );
    }

    public async Task<SeismicAcquisitionAndProcessingDto> CreateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateSeismicAcquisitionAndProcessingDto createProfileDto
    )
    {
        return await CreateExplorationProfile<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto, CreateSeismicAcquisitionAndProcessingDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            repository.CreateSeismicAcquisitionAndProcessing,
            ExplorationProfileNames.SeismicAcquisitionAndProcessing
        );
    }

    public async Task<CountryOfficeCostDto> CreateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateCountryOfficeCostDto createProfileDto
    )
    {
        return await CreateExplorationProfile<CountryOfficeCost, CountryOfficeCostDto, CreateCountryOfficeCostDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            repository.CreateCountryOfficeCost,
            ExplorationProfileNames.CountryOfficeCost
        );
    }

    private async Task<TDto> UpdateExplorationCostProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, IExplorationTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Exploration>(projectId, existingProfile.Exploration.Id);

        mapperService.MapToEntity(updatedProfileDto, existingProfile, explorationId);

        TProfile updatedProfile;
        try
        {
            updatedProfile = updateProfile(existingProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            logger.LogError(ex, "Failed to update profile {profileName} with id {profileId} for case id {caseId}.", profileName, profileId, caseId);
            throw;
        }

        var updatedDto = mapperService.MapToDto<TProfile, TDto>(updatedProfile, profileId);
        return updatedDto;
    }

    private async Task<TDto> CreateExplorationProfile<TProfile, TDto, TCreateDto>(
            Guid projectId,
            Guid caseId,
            Guid explorationId,
            TCreateDto createExplorationProfileDto,
            Func<TProfile, TProfile> createProfile,
            ExplorationProfileNames profileName
        )
            where TProfile : class, IExplorationTimeSeries, new()
            where TDto : class
            where TCreateDto : class
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Exploration>(projectId, explorationId);

        var exploration = await explorationRepository.GetExploration(explorationId)
            ?? throw new NotFoundInDBException($"Exploration with id {explorationId} not found.");

        var resourceHasProfile = await explorationRepository.ExplorationHasProfile(explorationId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Exploration with id {explorationId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            Exploration = exploration,
        };

        var newProfile = mapperService.MapToEntity(createExplorationProfileDto, profile, explorationId);

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
