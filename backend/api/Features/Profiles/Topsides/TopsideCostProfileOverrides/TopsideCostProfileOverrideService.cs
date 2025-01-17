using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Topsides.TopsideCostProfileOverrides.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Topsides.TopsideCostProfileOverrides;

public class TopsideCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        CreateTopsideCostProfileOverrideDto dto)
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
        UpdateTopsideCostProfileOverrideDto dto)
    {
        return await UpdateTopsideTimeSeries<TopsideCostProfileOverride, TopsideCostProfileOverrideDto, UpdateTopsideCostProfileOverrideDto>(
            projectId,
            caseId,
            topsideId,
            costProfileId,
            dto,
            id => context.TopsideCostProfileOverride.Include(x => x.Topside).SingleAsync(x => x.Id == id)
        );
    }

    private async Task<TDto> UpdateTopsideTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile)
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
