using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Substructures.SubstructureCostProfiles.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Substructures.SubstructureCostProfiles;

public class SubstructureCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IRecalculationService recalculationService)
{
    public async Task AddOrUpdateSubstructureCostProfile(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto)
    {
        var substructure = await context.Substructures
            .Include(t => t.CostProfile)
            .SingleAsync(t => t.ProjectId == projectId && t.Id == substructureId);

        if (substructure.CostProfile != null)
        {
            await UpdateSubstructureTimeSeries(projectId, caseId, substructureId, substructure.CostProfile.Id, dto);
            return;
        }

        await CreateSubstructureCostProfile(caseId, substructureId, dto, substructure);
    }

    private async Task CreateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto,
        Substructure substructure)
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
    }

    private async Task UpdateSubstructureTimeSeries(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        Guid profileId,
        UpdateSubstructureCostProfileDto updatedProfileDto)
    {
        var existingProfile = await context.SubstructureCostProfiles
            .Include(x => x.Substructure).ThenInclude(x => x.CostProfileOverride)
            .SingleAsync(x => x.Substructure.ProjectId == projectId && x.Id == profileId);

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
    }
}
