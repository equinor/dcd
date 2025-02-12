using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Prosp.Services.Assets.OnshorePowerSupplies;

public class OnshorePowerSupplyCostProfileService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task AddOrUpdateOnshorePowerSupplyCostProfile(Guid projectId, Guid caseId, UpdateTimeSeriesCostDto dto)
    {
        var profileTypes = new List<string> { ProfileTypes.OnshorePowerSupplyCostProfile, ProfileTypes.OnshorePowerSupplyCostProfileOverride };

        var caseItem = await context.Cases
            .Include(t => t.TimeSeriesProfiles.Where(x => profileTypes.Contains(x.ProfileType)))
            .Include(x => x.OnshorePowerSupply)
            .SingleAsync(x => x.ProjectId == projectId && x.Id == caseId);

        if (caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile) != null)
        {
            await UpdateOnshorePowerSupplyTimeSeries(caseItem, dto);
            return;
        }
        await CreateOnshorePowerSupplyCostProfile(caseItem, dto);
    }

    private async Task CreateOnshorePowerSupplyCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.OnshorePowerSupplyCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateCase(caseItem.Id);
    }

    private async Task UpdateOnshorePowerSupplyTimeSeries(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.OnshorePowerSupply.ProspVersion == null)
        {
            if (caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride) != null)
            {
                caseItem.GetProfile(ProfileTypes.OnshorePowerSupplyCostProfileOverride).Override = true;
            }
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.OnshorePowerSupplyCostProfile);
        existingProfile.StartYear = dto.StartYear;
        existingProfile.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateCase(caseItem.Id);
    }
}
