using System.Linq.Expressions;

using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.DrainageStrategies.Repositories;
using api.Features.CaseProfiles.Repositories;
using api.Features.Cases.Recalculation;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Services;

public class DrainageStrategyService(
    DcdDbContext context,
    ICaseRepository caseRepository,
    IDrainageStrategyRepository repository,
    IConversionMapperService conversionMapperService,
    IProjectAccessService projectAccessService,
    IRecalculationService recalculationService)
    : IDrainageStrategyService
{
    public async Task<DrainageStrategy> GetDrainageStrategyWithIncludes(
        Guid drainageStrategyId,
        params Expression<Func<DrainageStrategy, object>>[] includes
        )
    {
        return await repository.GetDrainageStrategyWithIncludes(drainageStrategyId, includes)
            ?? throw new NotFoundInDbException($"Drainage strategy with id {drainageStrategyId} not found.");
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
            ?? throw new NotFoundInDbException($"Drainage strategy with id {drainageStrategyId} not found.");

        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var project = await context.Projects.SingleAsync(p => p.Id == projectPk);

        conversionMapperService.MapToEntity(updatedDrainageStrategyDto, existingDrainageStrategy, drainageStrategyId, project.PhysicalUnit);

        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var dto = conversionMapperService.MapToDto<DrainageStrategy, DrainageStrategyDto>(existingDrainageStrategy, drainageStrategyId, project.PhysicalUnit);
        return dto;
    }
}
