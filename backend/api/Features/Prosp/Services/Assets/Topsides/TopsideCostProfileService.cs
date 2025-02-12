using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Topsides;

public static class TopsideCostProfileService
{
    public static void AddOrUpdateTopsideCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile) != null)
        {
            UpdateTopsideTimeSeries(caseItem, dto);
            return;
        }

        CreateTopsideCostProfile(caseItem, dto);
    }

    private static void CreateTopsideCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.TopsideCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }
    }

    private static void UpdateTopsideTimeSeries(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.Topside.ProspVersion == null)
        {
            if (caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride) != null)
            {
                caseItem.GetProfile(ProfileTypes.TopsideCostProfileOverride).Override = true;
            }
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.TopsideCostProfile);

        existingProfile.StartYear = dto.StartYear;
        existingProfile.Values = dto.Values;
    }
}
