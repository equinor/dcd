using System.Linq.Expressions;

using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;


namespace api.Services;

public class DrainageStrategyService(
    ILoggerFactory loggerFactory,
    ICaseRepository caseRepository,
    IDrainageStrategyRepository repository,
    IConversionMapperService conversionMapperService,
    IProjectRepository projectRepository,
    IProjectAccessService projectAccessService)
    : IDrainageStrategyService
{
    private readonly ILogger<DrainageStrategyService> _logger = loggerFactory.CreateLogger<DrainageStrategyService>();

    public async Task<DrainageStrategy> GetDrainageStrategyWithIncludes(
        Guid drainageStrategyId,
        params Expression<Func<DrainageStrategy, object>>[] includes
        )
    {
        return await repository.GetDrainageStrategyWithIncludes(drainageStrategyId, includes)
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
        await projectAccessService.ProjectExists<DrainageStrategy>(projectId, drainageStrategyId);

        var existingDrainageStrategy = await repository.GetDrainageStrategy(drainageStrategyId)
            ?? throw new NotFoundInDBException($"Drainage strategy with id {drainageStrategyId} not found.");

        var project = await projectRepository.GetProject(projectId)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found.");

        conversionMapperService.MapToEntity(updatedDrainageStrategyDto, existingDrainageStrategy, drainageStrategyId, project.PhysicalUnit);

        try
        {
            await caseRepository.UpdateModifyTime(caseId);
            await repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update drainage strategy with id {drainageStrategyId} for case id {caseId}.", drainageStrategyId, caseId);
            throw;
        }

        var dto = conversionMapperService.MapToDto<DrainageStrategy, DrainageStrategyDto>(existingDrainageStrategy, drainageStrategyId, project.PhysicalUnit);
        return dto;
    }
}
