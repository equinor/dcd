using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Transports.TransportCostProfiles;

public class TransportCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task AddOrUpdateTransportCostProfile(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        UpdateTimeSeriesCostDto dto)
    {
        var transport = await context.Transports
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.Id == transportId);

        if (transport.CostProfile != null)
        {
            await UpdateTransportCostProfile(projectId, caseId, transportId, transport.CostProfile.Id, dto);
            return;
        }

        await CreateTransportCostProfile(caseId, transportId, dto, transport);
    }

    private async Task CreateTransportCostProfile(
        Guid caseId,
        Guid transportId,
        UpdateTimeSeriesCostDto dto,
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
        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    private async Task UpdateTransportCostProfile(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid profileId,
        UpdateTimeSeriesCostDto dto)
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

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
