using System.Linq.Expressions;

using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SurfService : ISurfService
{
    private readonly IProjectAccessService _projectAccessService;
    private readonly ILogger<SurfService> _logger;
    private readonly ISurfRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;
    public SurfService(
        ILoggerFactory loggerFactory,
        ISurfRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService,
        IProjectAccessService projectAccessService
        )
    {
        _logger = loggerFactory.CreateLogger<SurfService>();
        _repository = repository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
        _projectAccessService = projectAccessService;
    }

    public async Task<Surf> GetSurfWithIncludes(Guid surfId, params Expression<Func<Surf, object>>[] includes)
    {
        return await _repository.GetSurfWithIncludes(surfId, includes)
            ?? throw new NotFoundInDBException($"Topside with id {surfId} not found.");
    }

    public async Task<SurfDto> UpdateSurf<TDto>(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    )
        where TDto : BaseUpdateSurfDto
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<Surf>(projectId, surfId);

        var existingSurf = await _repository.GetSurf(surfId)
            ?? throw new ArgumentException(string.Format($"Surf with id {surfId} not found."));

        _mapperService.MapToEntity(updatedSurfDto, existingSurf, surfId);
        existingSurf.LastChangedDate = DateTimeOffset.UtcNow;

        try
        {
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update surf with id {surfId} for case id {caseId}.", surfId, caseId);
            throw;
        }


        var dto = _mapperService.MapToDto<Surf, SurfDto>(existingSurf, surfId);
        return dto;
    }
}
