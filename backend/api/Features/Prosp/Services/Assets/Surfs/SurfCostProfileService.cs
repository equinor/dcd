using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Surfs;

public static class SurfCostProfileService
{
    public static void AddOrUpdateSurfCostProfile(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile) != null)
        {
            UpdateSurfTimeSeries(caseItem, startYear, values);
            return;
        }

        CreateSurfCostProfile(caseItem, startYear, values);
    }

    private static void CreateSurfCostProfile(Case caseItem, int startYear, double[] values)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SurfCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;

        SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride), false);
    }

    private static void UpdateSurfTimeSeries(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.Surf.ProspVersion == null)
        {
            SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride), true);
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.SurfCostProfile);

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
