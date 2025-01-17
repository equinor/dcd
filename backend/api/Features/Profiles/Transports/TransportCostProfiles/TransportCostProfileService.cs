using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Transports.TransportCostProfiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Transports.TransportCostProfiles;

public class TransportCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task CreateTransportCostProfile(
        Guid caseId,
        Guid transportId,
        UpdateTransportCostProfileDto dto,
        Transport transport)
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
    }

    public async Task UpdateTransportCostProfile(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid profileId,
        UpdateTransportCostProfileDto dto)
    {
        var existingProfile = await context.TransportCostProfile
            .Include(x => x.Transport).ThenInclude(transport => transport.CostProfileOverride)
            .SingleAsync(x => x.Transport.ProjectId == projectId && x.Id == profileId);

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
    }
}
