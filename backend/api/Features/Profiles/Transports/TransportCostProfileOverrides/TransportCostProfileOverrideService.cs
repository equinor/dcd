using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Transports.TransportCostProfileOverrides;

public class TransportCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateTransportCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        CreateTimeSeriesCostOverrideDto dto)
    {
        var transport = await context.Transports.SingleAsync(x => x.ProjectId == projectId && x.Id == transportId);

        var resourceHasProfile = await context.Transports.AnyAsync(t => t.Id == transportId && t.CostProfileOverride != null);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Transport with id {transportId} already has a profile of type {nameof(TransportCostProfileOverride)}.");
        }

        var profile = new TransportCostProfileOverride
        {
            Transport = transport
        };

        var newProfile = mapperService.MapToEntity(dto, profile, transportId);

        context.TransportCostProfileOverride.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TransportCostProfileOverride, TimeSeriesCostOverrideDto>(newProfile, newProfile.Id);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateTransportCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid costProfileId,
        UpdateTimeSeriesCostOverrideDto dto)
    {
        var existingProfile = await context.TransportCostProfileOverride
            .Include(x => x.Transport).ThenInclude(x => x.CostProfileOverride)
            .SingleAsync(x => x.Transport.ProjectId == projectId && x.Id == costProfileId);

        if (existingProfile.Transport.ProspVersion == null)
        {
            if (existingProfile.Transport.CostProfileOverride != null)
            {
                existingProfile.Transport.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(dto, existingProfile, transportId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TransportCostProfileOverride, TimeSeriesCostOverrideDto>(existingProfile, costProfileId);
    }
}
