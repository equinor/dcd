using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Substructures;

public static class SubstructureCostProfileService
{
    public static void AddOrUpdateSubstructureCostProfile(Case caseItem, int startYear, double[] values, bool overrideValue)
    {
        var overrideProfile = caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride);

        if (overrideProfile != null)
        {
            overrideProfile.Override = overrideValue;
        }

        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SubstructureCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;
    }
}
