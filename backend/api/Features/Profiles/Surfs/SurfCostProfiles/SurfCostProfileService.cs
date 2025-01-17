using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Surfs.SurfCostProfiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Surfs.SurfCostProfiles;

public class SurfCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task AddOrUpdateSurfCostProfile(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto)
    {
        await projectIntegrityService.EntityIsConnectedToProject<Surf>(projectId, surfId);

        var surf = await context.Surfs
            .Include(t => t.CostProfile)
            .Include(x => x.CostProfileOverride)
            .SingleAsync(t => t.Id == surfId);

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
        UpdateSurfCostProfileDto dto,
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
        UpdateSurfCostProfileDto updatedProfileDto)
    {
        var existingProfile = await context.SurfCostProfile
            .Include(x => x.Surf).ThenInclude(surf => surf.CostProfileOverride)
            .SingleAsync(x => x.Id == profileId);

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
    }
}
