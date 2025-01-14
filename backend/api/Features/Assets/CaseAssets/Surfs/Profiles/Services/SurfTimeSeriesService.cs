using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos.Create;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos.Update;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Surfs.Profiles.Services;

public class SurfTimeSeriesService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<SurfCostProfileOverrideDto> CreateSurfCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        CreateSurfCostProfileOverrideDto dto
    )
    {
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, surfId);

        var surf = await context.Surfs.SingleAsync(x => x.Id == surfId);

        var resourceHasProfile = await context.Surfs.AnyAsync(t => t.Id == surfId && t.CostProfileOverride != null);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Surf with id {surfId} already has a profile of type {nameof(SurfCostProfileOverride)}.");
        }

        SurfCostProfileOverride profile = new()
        {
            Surf = surf
        };

        var newProfile = mapperService.MapToEntity(dto, profile, surfId);

        context.SurfCostProfileOverride.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<SurfCostProfileOverride, SurfCostProfileOverrideDto>(newProfile, newProfile.Id);
    }

    public async Task<SurfCostProfileDto> AddOrUpdateSurfCostProfile(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto
    )
    {
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, surfId);

        var surf = await context.Surfs
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.Id == surfId);

        if (surf.CostProfile != null)
        {
            return await UpdateSurfCostProfile(projectId, caseId, surfId, surf.CostProfile.Id, dto);
        }

        return await CreateSurfCostProfile(caseId, surfId, dto, surf);
    }

    private async Task<SurfCostProfileDto> UpdateSurfCostProfile(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        Guid profileId,
        UpdateSurfCostProfileDto dto
    )
    {
        return await UpdateSurfTimeSeries<SurfCostProfile, SurfCostProfileDto, UpdateSurfCostProfileDto>(
            projectId,
            caseId,
            surfId,
            profileId,
            dto,
            id => context.SurfCostProfile.Include(x => x.Surf).SingleAsync(x => x.Id == id)
        );
    }

    private async Task<SurfCostProfileDto> CreateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto,
        Surf surf
    )
    {
        var surfCostProfile = new SurfCostProfile
        {
            Surf = surf
        };

        var newProfile = mapperService.MapToEntity(dto, surfCostProfile, surfId);

        if (newProfile.Surf.CostProfileOverride != null)
        {
            newProfile.Surf.CostProfileOverride.Override = false;
        }

        context.SurfCostProfile.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<SurfCostProfile, SurfCostProfileDto>(newProfile, newProfile.Id);
    }

    public async Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto
    )
    {
        return await UpdateSurfTimeSeries<SurfCostProfileOverride, SurfCostProfileOverrideDto, UpdateSurfCostProfileOverrideDto>(
            projectId,
            caseId,
            surfId,
            costProfileId,
            updatedSurfCostProfileOverrideDto,
            id => context.SurfCostProfileOverride.Include(x => x.Surf).SingleAsync(x => x.Id == id)
        );
    }

    private async Task<TDto> UpdateSurfTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
        where TProfile : class, ISurfTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId);

        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, existingProfile.Surf.Id);

        if (existingProfile.Surf.ProspVersion == null)
        {
            if (existingProfile.Surf.CostProfileOverride != null)
            {
                existingProfile.Surf.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, surfId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }
}
