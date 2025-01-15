using System.Linq.Expressions;

using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies;

public abstract class DrainageStrategyProfileBaseService(
    DcdDbContext context,
    IRecalculationService recalculationService,
    IProjectIntegrityService projectIntegrityService,
    IConversionMapperService conversionMapperService)
{
    protected DcdDbContext Context => context;

    protected async Task<TDto> CreateDrainageStrategyProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        TCreateDto createProductionProfileDto,
        Expression<Func<DrainageStrategy, bool>> hasProfileExpression
    )
        where TProfile : class, IDrainageStrategyTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        await projectIntegrityService.EntityIsConnectedToProject<DrainageStrategy>(projectId, drainageStrategyId);

        var drainageStrategy = await Context.DrainageStrategies.SingleAsync(x => x.Id == drainageStrategyId);

        var resourceHasProfile = await Context.DrainageStrategies
            .Where(d => d.Id == drainageStrategyId)
            .AnyAsync(hasProfileExpression);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Drainage strategy with id {drainageStrategyId} already has a profile of type {typeof(TProfile).Name}.");
        }

        var project = await Context.Projects.SingleAsync(p => p.Id == projectId);

        var profile = new TProfile
        {
            DrainageStrategy = drainageStrategy
        };

        var newProfile = conversionMapperService.MapToEntity(createProductionProfileDto, profile, drainageStrategyId, project.PhysicalUnit);

        Context.Add(newProfile);

        await Context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return conversionMapperService.MapToDto<TProfile, TDto>(newProfile, newProfile.Id, project.PhysicalUnit);
    }

    protected async Task<TDto> UpdateDrainageStrategyProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        TUpdateDto updatedProductionProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
        where TProfile : class, IDrainageStrategyTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(productionProfileId);

        var projectPk = await Context.GetPrimaryKeyForProjectId(projectId);

        await projectIntegrityService.EntityIsConnectedToProject<DrainageStrategy>(projectPk, existingProfile.DrainageStrategy.Id);

        var project = await Context.Projects.SingleAsync(p => p.Id == projectPk);

        conversionMapperService.MapToEntity(updatedProductionProfileDto, existingProfile, drainageStrategyId, project.PhysicalUnit);

        await Context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return conversionMapperService.MapToDto<TProfile, TDto>(existingProfile, productionProfileId, project.PhysicalUnit);
    }
}
