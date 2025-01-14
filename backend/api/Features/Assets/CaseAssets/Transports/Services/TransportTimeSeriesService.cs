using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Create;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Transports.Services;

public class TransportTimeSeriesService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<TransportCostProfileOverrideDto> CreateTransportCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        CreateTransportCostProfileOverrideDto dto
    )
    {
        await projectIntegrityService.EntityIsConnectedToProject<Transport>(projectId, transportId);

        var transport = await context.Transports.SingleAsync(x => x.Id == transportId);

        var resourceHasProfile = await context.Transports.AnyAsync(t => t.Id == transportId && t.CostProfileOverride != null);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Transport with id {transportId} already has a profile of type {nameof(TransportCostProfileOverride)}.");
        }

        TransportCostProfileOverride profile = new()
        {
            Transport = transport,
        };

        var newProfile = mapperService.MapToEntity(dto, profile, transportId);

        context.TransportCostProfileOverride.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(newProfile, newProfile.Id);
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
            id => context.TransportCostProfileOverride.Include(x => x.Transport).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<TransportCostProfileDto> AddOrUpdateTransportCostProfile(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        UpdateTransportCostProfileDto dto
    )
    {
        var transport = await context.Transports
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.Id == transportId);

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
            id => context.TransportCostProfile.Include(x => x.Transport).SingleAsync(x => x.Id == id)
        );
    }

    private async Task<TransportCostProfileDto> CreateTransportCostProfile(
        Guid caseId,
        Guid transportId,
        UpdateTransportCostProfileDto dto,
        Transport transport
    )
    {
        var transportCostProfile = new TransportCostProfile
        {
            Transport = transport
        };

        var newProfile = mapperService.MapToEntity(dto, transportCostProfile, transportId);

        if (newProfile.Transport.CostProfileOverride != null)
        {
            newProfile.Transport.CostProfileOverride.Override = false;
        }

        context.TransportCostProfile.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TransportCostProfile, TransportCostProfileDto>(newProfile, newProfile.Id);
    }

    private async Task<TDto> UpdateTransportTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
        where TProfile : class, ITransportTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId);

        await projectIntegrityService.EntityIsConnectedToProject<Transport>(projectId, existingProfile.Transport.Id);

        if (existingProfile.Transport.ProspVersion == null)
        {
            if (existingProfile.Transport.CostProfileOverride != null)
            {
                existingProfile.Transport.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, transportId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }
}
