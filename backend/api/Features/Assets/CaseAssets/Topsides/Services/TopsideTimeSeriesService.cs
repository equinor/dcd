using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Create;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Topsides.Services;

public class TopsideTimeSeriesService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        CreateTopsideCostProfileOverrideDto dto
    )
    {
        await projectIntegrityService.EntityIsConnectedToProject<Topside>(projectId, topsideId);

        var topside = await context.Topsides.SingleAsync(x => x.Id == topsideId);

        var resourceHasProfile = await context.Topsides.AnyAsync(t => t.Id == topsideId && t.CostProfileOverride != null);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Topside with id {topsideId} already has a profile of type {nameof(TopsideCostProfileOverride)}.");
        }

        TopsideCostProfileOverride profile = new()
        {
            Topside = topside
        };

        var newProfile = mapperService.MapToEntity(dto, profile, topsideId);

        context.TopsideCostProfileOverride.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>(newProfile, newProfile.Id);
    }

    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid costProfileId,
        UpdateTopsideCostProfileOverrideDto dto
    )
    {
        return await UpdateTopsideTimeSeries<TopsideCostProfileOverride, TopsideCostProfileOverrideDto, UpdateTopsideCostProfileOverrideDto>(
            projectId,
            caseId,
            topsideId,
            costProfileId,
            dto,
            id => context.TopsideCostProfileOverride.Include(x => x.Topside).SingleAsync(x => x.Id == id),
            profile => context.TopsideCostProfileOverride.Update(profile)
        );
    }

    public async Task<TopsideCostProfileDto> AddOrUpdateTopsideCostProfile(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto
    )
    {
        await projectIntegrityService.EntityIsConnectedToProject<Topside>(projectId, topsideId);

        var topside = await context.Topsides
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.Id == topsideId);

        if (topside.CostProfile != null)
        {
            return await UpdateTopsideCostProfile(projectId, caseId, topsideId, topside.CostProfile.Id, dto);
        }

        return await CreateTopsideCostProfile(caseId, topsideId, dto, topside);
    }

    private async Task<TopsideCostProfileDto> UpdateTopsideCostProfile(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        UpdateTopsideCostProfileDto dto
    )
    {
        return await UpdateTopsideTimeSeries<TopsideCostProfile, TopsideCostProfileDto, UpdateTopsideCostProfileDto>(
            projectId,
            caseId,
            topsideId,
            profileId,
            dto,
            id => context.TopsideCostProfiles.Include(x => x.Topside).SingleAsync(x => x.Id == id),
            profile => context.TopsideCostProfiles.Update(profile)
        );
    }

    private async Task<TopsideCostProfileDto> CreateTopsideCostProfile(
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto,
        Topside topside
    )
    {
        TopsideCostProfile topsideCostProfile = new()
        {
            Topside = topside
        };

        var newProfile = mapperService.MapToEntity(dto, topsideCostProfile, topsideId);
        if (newProfile.Topside.CostProfileOverride != null)
        {
            newProfile.Topside.CostProfileOverride.Override = false;
        }

        context.TopsideCostProfiles.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TopsideCostProfile, TopsideCostProfileDto>(newProfile, newProfile.Id);
    }

    private async Task<TDto> UpdateTopsideTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile,
        Action<TProfile> updateProfile
    )
        where TProfile : class, ITopsideTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId);

        await projectIntegrityService.EntityIsConnectedToProject<Topside>(projectId, existingProfile.Topside.Id);

        if (existingProfile.Topside.ProspVersion == null)
        {
            if (existingProfile.Topside.CostProfileOverride != null)
            {
                existingProfile.Topside.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, topsideId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }
}
