using System.Linq.Expressions;

using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services;

public abstract class UpdateCaseProfileBaseService(
    DcdDbContext context,
    IRecalculationService recalculationService,
    IProjectIntegrityService projectIntegrityService,
    IMapperService mapperService)
{
    protected DcdDbContext Context { get; } = context;

    protected async Task<TDto> UpdateCaseCostProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        TUpdateDto updatedCostProfileDto,
        Func<Guid, Task<TProfile>> getProfile)
        where TProfile : class, ICaseTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(costProfileId);

        await projectIntegrityService.EntityIsConnectedToProject<Case>(projectId, existingProfile.Case.Id);

        mapperService.MapToEntity(updatedCostProfileDto, existingProfile, caseId);

        await Context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, costProfileId);
    }

    protected async Task<TDto> CreateCaseProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
        Guid caseId,
        TCreateDto createProfileDto,
        Expression<Func<Case, bool>> caseHasProfileExpression)
        where TProfile : class, ICaseTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        await projectIntegrityService.EntityIsConnectedToProject<Case>(projectId, caseId);

        var caseEntity = await Context.Cases.SingleAsync(x => x.ProjectId == projectId && x.Id == caseId);

        var resourceHasProfile = await Context.Cases
            .Where(d => d.ProjectId == projectId && d.Id == caseId)
            .AnyAsync(caseHasProfileExpression);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Case with id {caseId} already has a profile of type {typeof(TProfile).Name}.");
        }

        var profile = new TProfile
        {
            Case = caseEntity
        };

        var newProfile = mapperService.MapToEntity(createProfileDto, profile, caseId);

        Context.Add(newProfile);
        await Context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(newProfile, newProfile.Id);
    }
}
