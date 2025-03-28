using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Surfs;

public static class SurfCostProfileService
{
    public static void AddOrUpdateSurfCostProfile(Case caseItem, int startYear, double[] values, bool overrideValue)
    {
        var overrideProfile = caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride);

        if (overrideProfile != null)
        {
            overrideProfile.Override = overrideValue;
        }

        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SurfCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;
    }
}
