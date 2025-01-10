using System.Linq.Expressions;

using api.Exceptions;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;
using api.Features.Assets.CaseAssets.Transports.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Services;

public class TransportService(
    ICaseRepository caseRepository,
    ITransportRepository transportRepository,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : ITransportService
{
    public async Task<Transport> GetTransportWithIncludes(Guid transportId, params Expression<Func<Transport, object>>[] includes)
    {
        return await transportRepository.GetTransportWithIncludes(transportId, includes)
            ?? throw new NotFoundInDbException($"Transport with id {transportId} not found.");
    }

    public async Task<TransportDto> UpdateTransport<TDto>(Guid projectId, Guid caseId, Guid transportId, TDto updatedTransportDto)
        where TDto : BaseUpdateTransportDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectIntegrityService.EntityIsConnectedToProject<Transport>(projectId, transportId);

        var existing = await transportRepository.GetTransport(transportId)
            ?? throw new NotFoundInDbException($"Transport with id {transportId} not found.");

        mapperService.MapToEntity(updatedTransportDto, existing, transportId);
        existing.LastChangedDate = DateTime.UtcNow;

        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var dto = mapperService.MapToDto<Transport, TransportDto>(existing, transportId);
        return dto;
    }
}
