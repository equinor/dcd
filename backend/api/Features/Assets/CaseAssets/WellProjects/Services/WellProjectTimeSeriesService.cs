using System.Linq.Expressions;

using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos.Create;
using api.Features.Assets.CaseAssets.WellProjects.Repositories;
using api.Features.CaseProfiles.Enums;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.Features.TechnicalInput.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.WellProjects.Services;

public class WellProjectTimeSeriesService(
    DcdDbContext context,
    WellProjectTimeSeriesRepository repository,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
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
            ?? throw new NotFoundInDbException($"Cost profile with id {profileId} not found.");

        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, existingProfile.WellProject.Id);

        mapperService.MapToEntity(updatedProfileDto, existingProfile, wellProjectId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
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
        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, wellProjectId);

        var wellProject = await context.WellProjects.SingleAsync(x => x.Id == wellProjectId);

        var resourceHasProfile = await WellProjectHasProfile(wellProjectId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Well project with id {wellProjectId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            WellProject = wellProject
        };

        var newProfile = mapperService.MapToEntity(createWellProjectProfileDto, profile, wellProjectId);

        var createdProfile = createProfile(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(createdProfile, createdProfile.Id);
    }

    private async Task<bool> WellProjectHasProfile(Guid wellProjectId, WellProjectProfileNames profileType)
    {
        Expression<Func<WellProject, bool>> profileExistsExpression = profileType switch
        {
            WellProjectProfileNames.OilProducerCostProfileOverride => d => d.OilProducerCostProfileOverride != null,
            WellProjectProfileNames.GasProducerCostProfileOverride => d => d.GasProducerCostProfileOverride != null,
            WellProjectProfileNames.WaterInjectorCostProfileOverride => d => d.WaterInjectorCostProfileOverride != null,
            WellProjectProfileNames.GasInjectorCostProfileOverride => d => d.GasInjectorCostProfileOverride != null,
        };

        return await context.WellProjects
            .Where(d => d.Id == wellProjectId)
            .AnyAsync(profileExistsExpression);
    }
}
