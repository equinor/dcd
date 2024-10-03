using System.Linq.Expressions;

using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;


namespace api.Services;

public class DrainageStrategyService : IDrainageStrategyService
{
    private readonly IProjectAccessService _projectAccessService;
    private readonly ILogger<DrainageStrategyService> _logger;
    private readonly ICaseRepository _caseRepository;
    private readonly IDrainageStrategyRepository _repository;
    private readonly IConversionMapperService _conversionMapperService;
    private readonly IProjectRepository _projectRepository;

    public DrainageStrategyService(
        ILoggerFactory loggerFactory,
        ICaseRepository caseRepository,
        IDrainageStrategyRepository repository,
        IConversionMapperService conversionMapperService,
        IProjectRepository projectRepository,
        IProjectAccessService projectAccessService
        )
    {
        _logger = loggerFactory.CreateLogger<DrainageStrategyService>();
        _caseRepository = caseRepository;
        _repository = repository;
        _conversionMapperService = conversionMapperService;
        _projectRepository = projectRepository;
        _projectAccessService = projectAccessService;
    }

    public async Task<DrainageStrategy> GetDrainageStrategyWithIncludes(
        Guid drainageStrategyId,
        params Expression<Func<DrainageStrategy, object>>[] includes
        )
    {
        return await _repository.GetDrainageStrategyWithIncludes(drainageStrategyId, includes)
            ?? throw new NotFoundInDBException($"Drainage strategy with id {drainageStrategyId} not found.");
    }

    public async Task<DrainageStrategyDto> UpdateDrainageStrategy(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        UpdateDrainageStrategyDto updatedDrainageStrategyDto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<DrainageStrategy>(projectId, drainageStrategyId);

        var existingDrainageStrategy = await _repository.GetDrainageStrategy(drainageStrategyId)
            ?? throw new NotFoundInDBException($"Drainage strategy with id {drainageStrategyId} not found.");

        var project = await _projectRepository.GetProject(projectId)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found.");

        _conversionMapperService.MapToEntity(updatedDrainageStrategyDto, existingDrainageStrategy, drainageStrategyId, project.PhysicalUnit);

        try
        {
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update drainage strategy with id {drainageStrategyId} for case id {caseId}.", drainageStrategyId, caseId);
            throw;
        }

        var dto = _conversionMapperService.MapToDto<DrainageStrategy, DrainageStrategyDto>(existingDrainageStrategy, drainageStrategyId, project.PhysicalUnit);
        return dto;
    }
}
