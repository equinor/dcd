using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Prosp.Services.Assets;

public class TransportCostProfileService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task AddOrUpdateTransportCostProfile(Guid projectId, Guid caseId, UpdateTimeSeriesCostDto dto)
    {
        var profileTypes = new List<string> { ProfileTypes.TransportCostProfile, ProfileTypes.TransportCostProfileOverride };

        var caseItem = await context.Cases
            .Include(t => t.TimeSeriesProfiles.Where(x => profileTypes.Contains(x.ProfileType)))
            .Include(x => x.Transport)
            .SingleAsync(x => x.ProjectId == projectId && x.Id == caseId);

        if (caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile) != null)
        {
            await UpdateTransportCostProfile(caseItem, dto);
            return;
        }

        await CreateTransportCostProfile(caseItem, dto);
    }

    private async Task CreateTransportCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.TransportCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateCase(caseItem.Id);
    }

    private async Task UpdateTransportCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.Transport!.ProspVersion == null)
        {
            if (caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride) != null)
            {
                caseItem.GetProfile(ProfileTypes.TransportCostProfileOverride).Override = true;
            }
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.TransportCostProfile);

        existingProfile.StartYear = dto.StartYear;
        existingProfile.Values = dto.Values;

        await context.UpdateCaseUpdatedUtc(caseItem.Id);
        await recalculationService.SaveChangesAndRecalculateCase(caseItem.Id);
    }
}
