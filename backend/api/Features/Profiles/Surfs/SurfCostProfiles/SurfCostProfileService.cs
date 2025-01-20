using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Surfs.SurfCostProfiles;

public class SurfCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task AddOrUpdateSurfCostProfile(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        UpdateTimeSeriesCostDto dto)
    {
        var surf = await context.Surfs
            .Include(t => t.CostProfile)
            .Include(x => x.CostProfileOverride)
            .SingleAsync(t => t.ProjectId == projectId && t.Id == surfId);

        if (surf.CostProfile != null)
        {
            await UpdateSurfTimeSeries(projectId, caseId, surfId, surf.CostProfile.Id, dto);
            return;
        }

        await CreateSurfCostProfile(caseId, surfId, dto, surf);
    }

    private async Task CreateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        UpdateTimeSeriesCostDto dto,
        Surf surf)
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
    }

    private async Task UpdateSurfTimeSeries(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        Guid profileId,
        UpdateTimeSeriesCostDto updatedProfileDto)
    {
        var existingProfile = await context.SurfCostProfile
            .Include(x => x.Surf).ThenInclude(surf => surf.CostProfileOverride)
            .SingleAsync(x => x.Surf.ProjectId == projectId && x.Id == profileId);

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
    }
}
