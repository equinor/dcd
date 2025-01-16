using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Create;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.Features.Stea.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Substructures.Services;

public class SubstructureTimeSeriesService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<SubstructureCostProfileDto> AddOrUpdateSubstructureCostProfile(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto
    )
    {
        await projectIntegrityService.EntityIsConnectedToProject<Substructure>(projectId, substructureId);

        var substructure = await context.Substructures
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.Id == substructureId);

        if (substructure.CostProfile != null)
        {
            return await UpdateSubstructureCostProfile(projectId, caseId, substructureId, substructure.CostProfile.Id, dto);
        }

        return await CreateSubstructureCostProfile(caseId, substructureId, dto, substructure);
    }

    private async Task<SubstructureCostProfileDto> UpdateSubstructureCostProfile(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        Guid profileId,
        UpdateSubstructureCostProfileDto dto
    )
    {
        return await UpdateSubstructureTimeSeries<SubstructureCostProfile, SubstructureCostProfileDto, UpdateSubstructureCostProfileDto>(
            projectId,
            caseId,
            substructureId,
            profileId,
            dto,
            id => context.SubstructureCostProfiles.Include(x => x.Substructure).SingleAsync(x => x.Id == id)
        );
    }

    private async Task<SubstructureCostProfileDto> CreateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto,
        Substructure substructure
    )
    {
        var substructureCostProfile = new SubstructureCostProfile
        {
            Substructure = substructure
        };

        var newProfile = mapperService.MapToEntity(dto, substructureCostProfile, substructureId);
        if (newProfile.Substructure.CostProfileOverride != null)
        {
            newProfile.Substructure.CostProfileOverride.Override = false;
        }

        context.SubstructureCostProfiles.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(newProfile, newProfile.Id);
    }

    public async Task<SubstructureCostProfileOverrideDto> CreateSubstructureCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        CreateSubstructureCostProfileOverrideDto dto
    )
    {
        await projectIntegrityService.EntityIsConnectedToProject<Substructure>(projectId, substructureId);

        var substructure = await context.Substructures.SingleAsync(x => x.Id == substructureId);

        var resourceHasProfile = await context.Substructures.AnyAsync(t => t.Id == substructureId && t.CostProfileOverride != null);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Substructure with id {substructureId} already has a profile of type {nameof(SubstructureCostProfileOverride)}.");
        }

        SubstructureCostProfileOverride profile = new()
        {
            Substructure = substructure,
        };

        var newProfile = mapperService.MapToEntity(dto, profile, substructureId);

        context.SubstructureCostProfileOverride.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>(newProfile, newProfile.Id);
    }

    public async Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        Guid costProfileId,
        UpdateSubstructureCostProfileOverrideDto dto
    )
    {
        return await UpdateSubstructureTimeSeries<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto, UpdateSubstructureCostProfileOverrideDto>(
            projectId,
            caseId,
            substructureId,
            costProfileId,
            dto,
            id => context.SubstructureCostProfileOverride.Include(x => x.Substructure).SingleAsync(x => x.Id == id)
        );
    }

    private async Task<TDto> UpdateSubstructureTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
        where TProfile : class, ISubstructureTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId);

        await projectIntegrityService.EntityIsConnectedToProject<Substructure>(projectId, existingProfile.Substructure.Id);

        if (existingProfile.Substructure.ProspVersion == null)
        {
            if (existingProfile.Substructure.CostProfileOverride != null)
            {
                existingProfile.Substructure.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, substructureId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }
}
