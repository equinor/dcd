using System.Linq.Expressions;

using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TransportService : ITransportService
{
    private readonly IProjectAccessService _projectAccessService;
    private readonly ILogger<TransportService> _logger;
    private readonly ICaseRepository _caseRepository;
    private readonly ITransportRepository _repository;
    private readonly IMapperService _mapperService;

    public TransportService(
        ILoggerFactory loggerFactory,
        ICaseRepository caseRepository,
        ITransportRepository transportRepository,
        IMapperService mapperService,
        IProjectAccessService projectAccessService
        )
    {
        _logger = loggerFactory.CreateLogger<TransportService>();
        _caseRepository = caseRepository;
        _repository = transportRepository;
        _mapperService = mapperService;
        _projectAccessService = projectAccessService;
    }

    public async Task<Transport> GetTransportWithIncludes(Guid transportId, params Expression<Func<Transport, object>>[] includes)
    {
        return await _repository.GetTransportWithIncludes(transportId, includes)
            ?? throw new NotFoundInDBException($"Transport with id {transportId} not found.");
    }

    public async Task<TransportDto> UpdateTransport<TDto>(Guid projectId, Guid caseId, Guid transportId, TDto updatedTransportDto)
        where TDto : BaseUpdateTransportDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<Transport>(projectId, transportId);

        var existing = await _repository.GetTransport(transportId)
            ?? throw new NotFoundInDBException($"Transport with id {transportId} not found.");

        _mapperService.MapToEntity(updatedTransportDto, existing, transportId);
        existing.LastChangedDate = DateTimeOffset.UtcNow;

        // Transport updatedTransport;
        try
        {
            // updatedTransport = _repository.UpdateTransport(existing);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Failed to update transport with id {transportId} for case id {caseId}.", transportId, caseId);
            throw;
        }


        var dto = _mapperService.MapToDto<Transport, TransportDto>(existing, transportId);
        return dto;
    }
}
