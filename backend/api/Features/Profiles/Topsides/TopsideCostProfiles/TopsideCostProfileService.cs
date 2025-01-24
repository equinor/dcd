using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Topsides.TopsideCostProfiles;

public class TopsideCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task AddOrUpdateTopsideCostProfile(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        UpdateTimeSeriesCostDto dto)
    {
        var topside = await context.Topsides
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.ProjectId == projectId && t.Id == topsideId);

        if (topside.CostProfile != null)
        {
            await UpdateTopsideTimeSeries(projectId, caseId, topsideId, topside.CostProfile.Id, dto);
            return;
        }

        await CreateTopsideCostProfile(caseId, topsideId, dto, topside);
    }

    private async Task CreateTopsideCostProfile(
        Guid caseId,
        Guid topsideId,
        UpdateTimeSeriesCostDto dto,
        Topside topside)
    {
        var topsideCostProfile = new TopsideCostProfile
        {
            Topside = topside
        };

        var newProfile = mapperService.MapToEntity(dto, topsideCostProfile, topsideId);
        if (newProfile.Topside.CostProfileOverride != null)
        {
            newProfile.Topside.CostProfileOverride.Override = false;
        }

        context.TopsideCostProfiles.Add(newProfile);
        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    private async Task UpdateTopsideTimeSeries(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        UpdateTimeSeriesCostDto updatedProfileDto)
    {
        var existingProfile = await context.TopsideCostProfiles
            .Include(x => x.Topside).ThenInclude(topside => topside.CostProfileOverride)
            .SingleAsync(x => x.Topside.ProjectId == projectId && x.Id == profileId);

        if (existingProfile.Topside.ProspVersion == null)
        {
            if (existingProfile.Topside.CostProfileOverride != null)
            {
                existingProfile.Topside.CostProfileOverride.Override = true;
            }
        }

        mapperService.MapToEntity(updatedProfileDto, existingProfile, topsideId);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }
}
