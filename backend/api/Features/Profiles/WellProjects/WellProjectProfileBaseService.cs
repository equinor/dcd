using System.Linq.Expressions;

using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.WellProjects;

public abstract class WellProjectProfileBaseService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    protected DcdDbContext Context => context;

    protected async Task<TDto> UpdateWellProjectCostProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
        where TProfile : class, IWellProjectTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId);

        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, existingProfile.WellProject.Id);

        mapperService.MapToEntity(updatedProfileDto, existingProfile, wellProjectId);

        await Context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }

    protected async Task<TDto> CreateWellProjectProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        TCreateDto createWellProjectProfileDto,
        Expression<Func<WellProject, bool>> hasProfileExpression
    )
        where TProfile : class, IWellProjectTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        await projectIntegrityService.EntityIsConnectedToProject<WellProject>(projectId, wellProjectId);

        var wellProject = await Context.WellProjects.SingleAsync(x => x.Id == wellProjectId);

        var resourceHasProfile = await Context.WellProjects
            .Where(d => d.Id == wellProjectId)
            .AnyAsync(hasProfileExpression);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Well project with id {wellProjectId} already has a profile of type {typeof(TProfile).Name}.");
        }

        var profile = new TProfile
        {
            WellProject = wellProject
        };

        var newProfile = mapperService.MapToEntity(createWellProjectProfileDto, profile, wellProjectId);

        Context.Add(newProfile);
        await Context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(newProfile, newProfile.Id);
    }
}
