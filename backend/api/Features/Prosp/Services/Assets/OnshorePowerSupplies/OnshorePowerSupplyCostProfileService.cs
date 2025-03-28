using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.OnshorePowerSupplies;

public static class OnshorePowerSupplyCostProfileService
{
    public static void AddOrUpdateOnshorePowerSupplyCostProfile(Case caseItem, int startYear, double[] values, bool overrideValue)
    {
        var overrideProfile = caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride);

        if (overrideProfile != null)
        {
            overrideProfile.Override = overrideValue;
        }

        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.OnshorePowerSupplyCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;
    }
}
