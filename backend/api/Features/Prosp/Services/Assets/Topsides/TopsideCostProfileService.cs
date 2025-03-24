using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Topsides;

public static class TopsideCostProfileService
{
    public static void AddOrUpdateTopsideCostProfile(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile) != null)
        {
            UpdateTopsideTimeSeries(caseItem, startYear, values);

            return;
        }

        CreateTopsideCostProfile(caseItem, startYear, values);
    }

    private static void CreateTopsideCostProfile(Case caseItem, int startYear, double[] values)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.TopsideCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;

        SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride), false);
    }

    private static void UpdateTopsideTimeSeries(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.Topside.ProspVersion == null)
        {
            SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride), false);
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.TopsideCostProfile);

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
