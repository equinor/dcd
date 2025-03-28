using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Topsides;

public static class TopsideCostProfileService
{
    public static void AddOrUpdateTopsideCostProfile(Case caseItem, int startYear, double[] values, bool overrideValue)
    {
        var overrideProfile = caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride);

        if (overrideProfile != null)
        {
            overrideProfile.Override = overrideValue;
        }

        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.TopsideCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;
    }
}
