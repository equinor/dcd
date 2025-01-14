using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Transports.Services;

public class TransportService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<TransportDto> UpdateTransport<TDto>(Guid projectId, Guid caseId, Guid transportId, TDto updatedTransportDto)
        where TDto : BaseUpdateTransportDto
    {
        await projectIntegrityService.EntityIsConnectedToProject<Transport>(projectId, transportId);

        var existing = await context.Transports.SingleAsync(x => x.Id == transportId);

        mapperService.MapToEntity(updatedTransportDto, existing, transportId);
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<Transport, TransportDto>(existing, transportId);
    }
}
