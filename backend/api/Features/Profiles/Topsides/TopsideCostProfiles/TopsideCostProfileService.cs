using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Topsides.TopsideCostProfileOverrides.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Topsides.TopsideCostProfiles;

public class TopsideCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task AddOrUpdateTopsideCostProfile(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Topside>(projectId, topsideId);

        var topside = await context.Topsides
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.Id == topsideId);

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
        UpdateTopsideCostProfileDto dto,
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
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    private async Task UpdateTopsideTimeSeries(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        UpdateTopsideCostProfileDto updatedProfileDto)
    {
        var existingProfile = await context.TopsideCostProfiles
            .Include(x => x.Topside).ThenInclude(topside => topside.CostProfileOverride)
            .SingleAsync(x => x.Id == profileId);

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
    }
}
