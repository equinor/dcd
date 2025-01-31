using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Prosp.Services.Assets;

public class SurfCostProfileService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task AddOrUpdateSurfCostProfile(Guid projectId, Guid caseId, UpdateTimeSeriesCostDto dto)
    {
        var profileTypes = new List<string> { ProfileTypes.SurfCostProfile, ProfileTypes.SurfCostProfileOverride };

        var caseItem = await context.Cases
            .Include(t => t.TimeSeriesProfiles.Where(x => profileTypes.Contains(x.ProfileType)))
            .Include(x => x.Surf)
            .SingleAsync(x => x.ProjectId == projectId && x.Id == caseId);

        if (caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile) != null)
        {
            await UpdateSurfTimeSeries(caseItem, dto);
            return;
        }

        await CreateSurfCostProfile(caseItem, dto);
    }

    private async Task CreateSurfCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SurfCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateCase(caseItem.Id);
    }

    private async Task UpdateSurfTimeSeries(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.Surf!.ProspVersion == null)
        {
            if (caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride) != null)
            {
                caseItem.GetProfile(ProfileTypes.SurfCostProfileOverride).Override = true;
            }
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.SurfCostProfile);

        existingProfile.StartYear = dto.StartYear;
        existingProfile.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateCase(caseItem.Id);
    }
}
