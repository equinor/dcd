using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Topsides.TopsideCostProfileOverrides;

public class TopsideCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        CreateTimeSeriesCostOverrideDto dto)
    {
        var topside = await context.Topsides.SingleAsync(x => x.ProjectId == projectId && x.Id == topsideId);

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
        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TopsideCostProfileOverride, TimeSeriesCostOverrideDto>(newProfile, newProfile.Id);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid costProfileId,
        UpdateTimeSeriesCostOverrideDto dto)
    {
        var existingProfile = await context.TopsideCostProfileOverride
            .Include(x => x.Topside)
            .SingleAsync(x => x.Topside.ProjectId == projectId && x.Id == costProfileId);

        if (existingProfile.Topside.ProspVersion == null)
        {
            if (existingProfile.Topside.CostProfileOverride != null)
            {
                existingProfile.Topside.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(dto, existingProfile, topsideId);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TopsideCostProfileOverride, TimeSeriesCostOverrideDto>(existingProfile, costProfileId);
    }
}
