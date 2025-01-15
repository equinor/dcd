using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Transports.TransportCostProfiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Transports.TransportCostProfiles;

public class TransportCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : TransportProfileBaseService(context, mapperService, projectIntegrityService, recalculationService)
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

        var newProfile = MapperService.MapToEntity(dto, transportCostProfile, transportId);

        if (newProfile.Transport.CostProfileOverride != null)
        {
            newProfile.Transport.CostProfileOverride.Override = false;
        }

        Context.TransportCostProfile.Add(newProfile);
        await Context.UpdateCaseModifyTime(caseId);
        await RecalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateTransportCostProfile(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid profileId,
        UpdateTransportCostProfileDto dto)
    {
        await UpdateTransportTimeSeries<TransportCostProfile, TransportCostProfileDto, UpdateTransportCostProfileDto>(
            projectId,
            caseId,
            transportId,
            profileId,
            dto,
            id => Context.TransportCostProfile.Include(x => x.Transport).SingleAsync(x => x.Id == id));
    }
}
