using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Topsides.TopsideCostProfiles;

public class TopsideCostProfileService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task AddOrUpdateTopsideCostProfile(Guid projectId, Guid caseId, UpdateTimeSeriesCostDto dto)
    {
        var profileTypes = new List<string> { ProfileTypes.TopsideCostProfile, ProfileTypes.TopsideCostProfileOverride };

        var caseItem = await context.Cases
            .Include(t => t.TimeSeriesProfiles.Where(x => profileTypes.Contains(x.ProfileType)))
            .Include(x => x.Topside)
            .SingleAsync(x => x.ProjectId == projectId && x.Id == caseId);

        if (caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile) != null)
        {
            await UpdateTopsideTimeSeries(caseItem, dto);
            return;
        }

        await CreateTopsideCostProfile(caseItem, dto);
    }

    private async Task CreateTopsideCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.TopsideCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateAsync(caseItem.Id);
    }

    private async Task UpdateTopsideTimeSeries(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.Topside!.ProspVersion == null)
        {
            if (caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride) != null)
            {
                caseItem.GetProfile(ProfileTypes.TopsideCostProfileOverride).Override = true;
            }
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.TopsideCostProfile);

        existingProfile.StartYear = dto.StartYear;
        existingProfile.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateAsync(caseItem.Id);
    }
}
