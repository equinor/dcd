using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Transports.TransportCostProfileOverrides.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Transports.TransportCostProfileOverrides;

public class TransportCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : TransportProfileBaseService(context, mapperService, projectIntegrityService, recalculationService)
{
    public async Task<TransportCostProfileOverrideDto> CreateTransportCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        CreateTransportCostProfileOverrideDto dto)
    {
        await ProjectIntegrityService.EntityIsConnectedToProject<Transport>(projectId, transportId);

        var transport = await Context.Transports.SingleAsync(x => x.Id == transportId);

        var resourceHasProfile = await Context.Transports.AnyAsync(t => t.Id == transportId && t.CostProfileOverride != null);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Transport with id {transportId} already has a profile of type {nameof(TransportCostProfileOverride)}.");
        }

        TransportCostProfileOverride profile = new()
        {
            Transport = transport,
        };

        var newProfile = MapperService.MapToEntity(dto, profile, transportId);

        Context.TransportCostProfileOverride.Add(newProfile);
        await Context.UpdateCaseModifyTime(caseId);
        await RecalculationService.SaveChangesAndRecalculateAsync(caseId);

        return MapperService.MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(newProfile, newProfile.Id);
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
            id => Context.TransportCostProfileOverride.Include(x => x.Transport).SingleAsync(x => x.Id == id));
    }
}
