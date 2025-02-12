using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

namespace api.Features.Prosp.Services.Assets.OnshorePowerSupplies;

public static class OnshorePowerSupplyCostProfileService
{
    public static void AddOrUpdateOnshorePowerSupplyCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile) != null)
        {
            UpdateOnshorePowerSupplyTimeSeries(caseItem, dto);
            return;
        }

        CreateOnshorePowerSupplyCostProfile(caseItem, dto);
    }

    private static void CreateOnshorePowerSupplyCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.OnshorePowerSupplyCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }
    }

    private static void UpdateOnshorePowerSupplyTimeSeries(Case caseItem, UpdateTimeSeriesCostDto dto)
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
    }
}
