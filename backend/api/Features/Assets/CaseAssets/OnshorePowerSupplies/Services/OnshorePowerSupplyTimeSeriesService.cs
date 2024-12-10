using api.Exceptions;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Create;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Repositories;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Services;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;


public class OnshorePowerSupplyTimeSeriesService(
    ILogger<OnshorePowerSupplyService> logger,
    ICaseRepository caseRepository,
    IOnshorePowerSupplyRepository onshorePowerSupplyRepository,
    IOnshorePowerSupplyTimeSeriesRepository repository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : IOnshorePowerSupplyTimeSeriesService
{
    public async Task<OnshorePowerSupplyCostProfileOverrideDto> CreateOnshorePowerSupplyCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        CreateOnshorePowerSupplyCostProfileOverrideDto dto
    )
    {
        await projectAccessService.ProjectExists<OnshorePowerSupply>(projectId, onshorePowerSupplyId);

        var onshorePowerSupply = await onshorePowerSupplyRepository.GetOnshorePowerSupply(onshorePowerSupplyId)
            ?? throw new NotFoundInDBException($"OnshorePowerSupply with id {onshorePowerSupplyId} not found.");

        var resourceHasProfile = await onshorePowerSupplyRepository.OnshorePowerSupplyHasCostProfileOverride(onshorePowerSupplyId);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"OnshorePowerSupply with id {onshorePowerSupplyId} already has a profile of type {typeof(OnshorePowerSupplyCostProfileOverride).Name}.");
        }

        OnshorePowerSupplyCostProfileOverride profile = new()
        {
            OnshorePowerSupply = onshorePowerSupply,
        };

        var newProfile = mapperService.MapToEntity(dto, profile, onshorePowerSupplyId);

        OnshorePowerSupplyCostProfileOverride createdProfile;
        try
        {
            createdProfile = repository.CreateOnshorePowerSupplyCostProfileOverride(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to create profile OnshorePowerSupplyCostProfileOverride for case id {caseId}.", caseId);
            throw;
        }

        var updatedDto = mapperService.MapToDto<OnshorePowerSupplyCostProfileOverride, OnshorePowerSupplyCostProfileOverrideDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }

    public async Task<OnshorePowerSupplyCostProfileOverrideDto> UpdateOnshorePowerSupplyCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid costProfileId,
        UpdateOnshorePowerSupplyCostProfileOverrideDto dto)
    {
        return await UpdateOnshorePowerSupplyTimeSeries<OnshorePowerSupplyCostProfileOverride, OnshorePowerSupplyCostProfileOverrideDto, UpdateOnshorePowerSupplyCostProfileOverrideDto>(
            projectId,
            caseId,
            onshorePowerSupplyId,
            costProfileId,
            dto,
            repository.GetOnshorePowerSupplyCostProfileOverride,
            repository.UpdateOnshorePowerSupplyCostProfileOverride
        );
    }

    public async Task<OnshorePowerSupplyCostProfileDto> AddOrUpdateOnshorePowerSupplyCostProfile(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        UpdateOnshorePowerSupplyCostProfileDto dto
    )
    {
        var onshorePowerSupply = await onshorePowerSupplyRepository.GetOnshorePowerSupplyWithCostProfile(onshorePowerSupplyId)
            ?? throw new NotFoundInDBException($"OnshorePowerSupply with id {onshorePowerSupplyId} not found.");

        if (onshorePowerSupply.CostProfile != null)
        {
            return await UpdateOnshorePowerSupplyCostProfile(projectId, caseId, onshorePowerSupplyId, onshorePowerSupply.CostProfile.Id, dto);
        }

        return await CreateOnshorePowerSupplyCostProfile(caseId, onshorePowerSupplyId, dto, onshorePowerSupply);
    }

    private async Task<OnshorePowerSupplyCostProfileDto> UpdateOnshorePowerSupplyCostProfile(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid profileId,
        UpdateOnshorePowerSupplyCostProfileDto dto
    )
    {
        return await UpdateOnshorePowerSupplyTimeSeries<OnshorePowerSupplyCostProfile, OnshorePowerSupplyCostProfileDto, UpdateOnshorePowerSupplyCostProfileDto>(
            projectId,
            caseId,
            onshorePowerSupplyId,
            profileId,
            dto,
            repository.GetOnshorePowerSupplyCostProfile,
            repository.UpdateOnshorePowerSupplyCostProfile
        );
    }

    private async Task<OnshorePowerSupplyCostProfileDto> CreateOnshorePowerSupplyCostProfile(
        Guid caseId,
        Guid onshorePowerSupplyId,
        UpdateOnshorePowerSupplyCostProfileDto dto,
        OnshorePowerSupply onshorePowerSupply
    )
    {
        OnshorePowerSupplyCostProfile onshorePowerSupplyCostProfile = new OnshorePowerSupplyCostProfile
        {
            OnshorePowerSupply = onshorePowerSupply
        };

        var newProfile = mapperService.MapToEntity(dto, onshorePowerSupplyCostProfile, onshorePowerSupplyId);

        if (newProfile.OnshorePowerSupply.CostProfileOverride != null)
        {
            newProfile.OnshorePowerSupply.CostProfileOverride.Override = false;
        }

        try
        {
            repository.CreateOnshorePowerSupplyCostProfile(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create cost profile for onshorePowerSupply with id {onshorePowerSupplyId} for case id {caseId}.", onshorePowerSupplyId, caseId);
            throw;
        }

        var newDto = mapperService.MapToDto<OnshorePowerSupplyCostProfile, OnshorePowerSupplyCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    private async Task<TDto> UpdateOnshorePowerSupplyTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, IOnshorePowerSupplyTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        await projectAccessService.ProjectExists<OnshorePowerSupply>(projectId, existingProfile.OnshorePowerSupply.Id);

        if (existingProfile.OnshorePowerSupply.ProspVersion == null)
        {
            if (existingProfile.OnshorePowerSupply.CostProfileOverride != null)
            {
                existingProfile.OnshorePowerSupply.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, onshorePowerSupplyId);

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
