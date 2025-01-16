using System.Linq.Expressions;

using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Explorations;

public abstract class ExplorationProfileBaseService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    protected DcdDbContext Context => context;

    protected async Task<TDto> CreateExplorationProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        TCreateDto createExplorationProfileDto,
        Expression<Func<Exploration, bool>> hasProfileExpression
    )
        where TProfile : class, IExplorationTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        await projectIntegrityService.EntityIsConnectedToProject<Exploration>(projectId, explorationId);

        var exploration = await Context.Explorations.SingleAsync(x => x.Id == explorationId);

        var resourceHasProfile = await Context.Explorations
            .Where(d => d.Id == explorationId)
            .AnyAsync(hasProfileExpression);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Exploration with id {explorationId} already has a profile of type {typeof(TProfile).Name}.");
        }

        var profile = new TProfile
        {
            Exploration = exploration
        };

        var newProfile = mapperService.MapToEntity(createExplorationProfileDto, profile, explorationId);

        Context.Add(newProfile);
        await Context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(newProfile, newProfile.Id);
    }

    protected async Task<TDto> UpdateExplorationCostProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
        where TProfile : class, IExplorationTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId);

        await projectIntegrityService.EntityIsConnectedToProject<Exploration>(projectId, existingProfile.Exploration.Id);

        mapperService.MapToEntity(updatedProfileDto, existingProfile, explorationId);

        await Context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }
}
