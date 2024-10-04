using System.Linq.Expressions;

using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TopsideService : ITopsideService
{
    private readonly IProjectAccessService _projectAccessService;
    private readonly ILogger<TopsideService> _logger;
    private readonly ITopsideRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;

    public TopsideService(
        ILoggerFactory loggerFactory,
        ITopsideRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService,
        IProjectAccessService projectAccessService
        )
    {
        _logger = loggerFactory.CreateLogger<TopsideService>();
        _repository = repository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
        _projectAccessService = projectAccessService;
    }

    public async Task<Topside> GetTopsideWithIncludes(Guid topsideId, params Expression<Func<Topside, object>>[] includes)
    {
        return await _repository.GetTopsideWithIncludes(topsideId, includes)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");
    }

    public async Task<TopsideDto> UpdateTopside<TDto>(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        TDto updatedTopsideDto
    )
        where TDto : BaseUpdateTopsideDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<Topside>(projectId, topsideId);

        var existingTopside = await _repository.GetTopside(topsideId)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");

        _mapperService.MapToEntity(updatedTopsideDto, existingTopside, topsideId);
        existingTopside.LastChangedDate = DateTimeOffset.UtcNow;

        try
        {
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update topside with id {topsideId} for case id {caseId}.", topsideId, caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<Topside, TopsideDto>(existingTopside, topsideId);
        return dto;
    }
}
