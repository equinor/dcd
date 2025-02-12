using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Surfs;

public static class SurfCostProfileService
{
    public static void AddOrUpdateSurfCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile) != null)
        {
            UpdateSurfTimeSeries(caseItem, dto);
            return;
        }

        CreateSurfCostProfile(caseItem, dto);
    }

    private static void CreateSurfCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SurfCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }
    }

    private static void UpdateSurfTimeSeries(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.Surf.ProspVersion == null)
        {
            if (caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride) != null)
            {
                caseItem.GetProfile(ProfileTypes.SurfCostProfileOverride).Override = true;
            }
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.SurfCostProfile);

        existingProfile.StartYear = dto.StartYear;
        existingProfile.Values = dto.Values;
    }
}
