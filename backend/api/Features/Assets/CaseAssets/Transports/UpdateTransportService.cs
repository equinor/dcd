using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Transports;

public class UpdateTransportService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task UpdateTransport(Guid projectId, Guid caseId, Guid transportId, UpdateTransportDto updatedTransportDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Transport>(projectId, transportId);

        var existing = await context.Transports.SingleAsync(x => x.Id == transportId);

        mapperService.MapToEntity(updatedTransportDto, existing, transportId);
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateTransport(Guid projectId, Guid caseId, Guid transportId, ProspUpdateTransportDto updatedTransportDto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Transport>(projectId, transportId);

        var existing = await context.Transports.SingleAsync(x => x.Id == transportId);

        mapperService.MapToEntity(updatedTransportDto, existing, transportId);
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
