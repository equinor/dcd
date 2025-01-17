using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Surfs.SurfCostProfileOverrides.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Surfs.SurfCostProfileOverrides;

public class SurfTimeSeriesService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task<SurfCostProfileOverrideDto> CreateSurfCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        CreateSurfCostProfileOverrideDto dto)
    {
        var surf = await context.Surfs.SingleAsync(x => x.ProjectId == projectId && x.Id == surfId);

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

    public async Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto)
    {
        var existingProfile = await context.SurfCostProfileOverride
            .Include(x => x.Surf)
            .SingleAsync(x => x.Surf.ProjectId == projectId && x.Id ==costProfileId);

        if (existingProfile.Surf.ProspVersion == null)
        {
            if (existingProfile.Surf.CostProfileOverride != null)
            {
                existingProfile.Surf.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedSurfCostProfileOverrideDto, existingProfile, surfId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<SurfCostProfileOverride, SurfCostProfileOverrideDto>(existingProfile, costProfileId);
    }
}
