using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Prosp.Services.Assets;

public class SubstructureCostProfileService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task AddOrUpdateSubstructureCostProfile(Guid projectId, Guid caseId, UpdateTimeSeriesCostDto dto)
    {
        var profileTypes = new List<string> { ProfileTypes.SubstructureCostProfile, ProfileTypes.SubstructureCostProfileOverride };

        var caseItem = await context.Cases
            .Include(t => t.TimeSeriesProfiles.Where(x => profileTypes.Contains(x.ProfileType)))
            .Include(x => x.Substructure)
            .SingleAsync(x => x.ProjectId == projectId && x.Id == caseId);

        if (caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile) != null)
        {
            await UpdateSubstructureTimeSeries(caseItem, dto);
            return;
        }

        await CreateSubstructureCostProfile(caseItem, dto);
    }

    private async Task CreateSubstructureCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SubstructureCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateCase(caseItem.Id);
    }

    private async Task UpdateSubstructureTimeSeries(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.Substructure!.ProspVersion == null)
        {
            if (caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride) != null)
            {
                caseItem.GetProfile(ProfileTypes.SubstructureCostProfileOverride).Override = true;
            }
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.SubstructureCostProfile);

        existingProfile.StartYear = dto.StartYear;
        existingProfile.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateCase(caseItem.Id);
    }
}
