using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.OnshorePowerSupplies;

public static class OnshorePowerSupplyCostProfileService
{
    public static void AddOrUpdateOnshorePowerSupplyCostProfile(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile) != null)
        {
            UpdateOnshorePowerSupplyTimeSeries(caseItem, startYear, values);
            return;
        }

        CreateOnshorePowerSupplyCostProfile(caseItem, startYear, values);
    }

    private static void CreateOnshorePowerSupplyCostProfile(Case caseItem, int startYear, double[] values)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.OnshorePowerSupplyCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;

        SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride), false);
    }

    private static void UpdateOnshorePowerSupplyTimeSeries(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.OnshorePowerSupply.ProspVersion == null)
        {
            SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride), true);
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.OnshorePowerSupplyCostProfile);

        existingProfile.StartYear = startYear;
        existingProfile.Values = values;
    }

    private static void SetOverrideFlag(TimeSeriesProfile? overrideProfile, bool overrideValue)
    {
        if (overrideProfile != null)
        {
            overrideProfile.Override = overrideValue;
        }
    }
}
