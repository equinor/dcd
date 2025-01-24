using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Substructures.SubstructureCostProfileOverrides;

public class SubstructureCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateSubstructureCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        CreateTimeSeriesCostOverrideDto dto)
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
        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<SubstructureCostProfileOverride, TimeSeriesCostOverrideDto>(newProfile, newProfile.Id);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateSubstructureCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        Guid costProfileId,
        UpdateTimeSeriesCostOverrideDto dto)
    {
        return await UpdateSubstructureTimeSeries<SubstructureCostProfileOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            substructureId,
            costProfileId,
            dto,
            id => context.SubstructureCostProfileOverride.Include(x => x.Substructure).SingleAsync(x => x.Id == id));
    }

    private async Task<TDto> UpdateSubstructureTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile)
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

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }
}
