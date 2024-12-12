using api.Context.Recalculation;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Create;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;
using api.Features.Assets.CaseAssets.Transports.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Transports.Services;

public class TransportTimeSeriesService(
    ILogger<TransportService> logger,
    ICaseRepository caseRepository,
    ITransportRepository transportRepository,
    ITransportTimeSeriesRepository repository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService,
    IRecalculationService recalculationService)
    : ITransportTimeSeriesService
{
    public async Task<TransportCostProfileOverrideDto> CreateTransportCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        CreateTransportCostProfileOverrideDto dto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Transport>(projectId, transportId);

        var transport = await transportRepository.GetTransport(transportId)
            ?? throw new NotFoundInDBException($"Transport with id {transportId} not found.");

        var resourceHasProfile = await transportRepository.TransportHasCostProfileOverride(transportId);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Transport with id {transportId} already has a profile of type {typeof(TransportCostProfileOverride).Name}.");
        }

        TransportCostProfileOverride profile = new()
        {
            Transport = transport,
        };

        var newProfile = mapperService.MapToEntity(dto, profile, transportId);

        TransportCostProfileOverride createdProfile;
        try
        {
            createdProfile = repository.CreateTransportCostProfileOverride(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await recalculationService.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to create profile TransportCostProfileOverride for case id {caseId}.", caseId);
            throw;
        }

        var updatedDto = mapperService.MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }

    public async Task<TransportCostProfileOverrideDto> UpdateTransportCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid costProfileId,
        UpdateTransportCostProfileOverrideDto dto)
    {
        return await UpdateTransportTimeSeries<TransportCostProfileOverride, TransportCostProfileOverrideDto, UpdateTransportCostProfileOverrideDto>(
            projectId,
            caseId,
            transportId,
            costProfileId,
            dto,
            repository.GetTransportCostProfileOverride,
            repository.UpdateTransportCostProfileOverride
        );
    }

    public async Task<TransportCostProfileDto> AddOrUpdateTransportCostProfile(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        UpdateTransportCostProfileDto dto
    )
    {
        var transport = await transportRepository.GetTransportWithCostProfile(transportId)
            ?? throw new NotFoundInDBException($"Transport with id {transportId} not found.");

        if (transport.CostProfile != null)
        {
            return await UpdateTransportCostProfile(projectId, caseId, transportId, transport.CostProfile.Id, dto);
        }

        return await CreateTransportCostProfile(caseId, transportId, dto, transport);
    }

    private async Task<TransportCostProfileDto> UpdateTransportCostProfile(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid profileId,
        UpdateTransportCostProfileDto dto
    )
    {
        return await UpdateTransportTimeSeries<TransportCostProfile, TransportCostProfileDto, UpdateTransportCostProfileDto>(
            projectId,
            caseId,
            transportId,
            profileId,
            dto,
            repository.GetTransportCostProfile,
            repository.UpdateTransportCostProfile
        );
    }

    private async Task<TransportCostProfileDto> CreateTransportCostProfile(
        Guid caseId,
        Guid transportId,
        UpdateTransportCostProfileDto dto,
        Transport transport
    )
    {
        TransportCostProfile transportCostProfile = new TransportCostProfile
        {
            Transport = transport
        };

        var newProfile = mapperService.MapToEntity(dto, transportCostProfile, transportId);

        if (newProfile.Transport.CostProfileOverride != null)
        {
            newProfile.Transport.CostProfileOverride.Override = false;
        }

        try
        {
            repository.CreateTransportCostProfile(newProfile);
            await caseRepository.UpdateModifyTime(caseId);
            await recalculationService.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create cost profile for transport with id {transportId} for case id {caseId}.", transportId, caseId);
            throw;
        }

        var newDto = mapperService.MapToDto<TransportCostProfile, TransportCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    private async Task<TDto> UpdateTransportTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, ITransportTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Transport>(projectId, existingProfile.Transport.Id);

        if (existingProfile.Transport.ProspVersion == null)
        {
            if (existingProfile.Transport.CostProfileOverride != null)
            {
                existingProfile.Transport.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, transportId);

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
