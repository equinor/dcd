using System.Linq.Expressions;

using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TransportService(
    ILogger<TransportService> logger,
    ICaseRepository caseRepository,
    ITransportRepository transportRepository,
    IMapperService mapperService,
    IProjectAccessService projectAccessService)
    : ITransportService
{
    public async Task<Transport> GetTransportWithIncludes(Guid transportId, params Expression<Func<Transport, object>>[] includes)
    {
        return await transportRepository.GetTransportWithIncludes(transportId, includes)
            ?? throw new NotFoundInDBException($"Transport with id {transportId} not found.");
    }

    public async Task<TransportDto> UpdateTransport<TDto>(Guid projectId, Guid caseId, Guid transportId, TDto updatedTransportDto)
        where TDto : BaseUpdateTransportDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectAccessService.ProjectExists<Transport>(projectId, transportId);

        var existing = await transportRepository.GetTransport(transportId)
            ?? throw new NotFoundInDBException($"Transport with id {transportId} not found.");

        mapperService.MapToEntity(updatedTransportDto, existing, transportId);
        existing.LastChangedDate = DateTimeOffset.UtcNow;

        // Transport updatedTransport;
        try
        {
            // updatedTransport = _repository.UpdateTransport(existing);
            await caseRepository.UpdateModifyTime(caseId);
            await transportRepository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Failed to update transport with id {transportId} for case id {caseId}.", transportId, caseId);
            throw;
        }


        var dto = mapperService.MapToDto<Transport, TransportDto>(existing, transportId);
        return dto;
    }
}
