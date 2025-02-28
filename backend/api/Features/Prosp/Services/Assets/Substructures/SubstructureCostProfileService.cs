using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Substructures;

public static class SubstructureCostProfileService
{
    public static void AddOrUpdateSubstructureCostProfile(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile) != null)
        {
            UpdateSubstructureTimeSeries(caseItem, startYear, values);

            return;
        }

        CreateSubstructureCostProfile(caseItem, startYear, values);
    }

    private static void CreateSubstructureCostProfile(Case caseItem, int startYear, double[] values)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SubstructureCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;

        SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride), false);
    }

    private static void UpdateSubstructureTimeSeries(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.Substructure.ProspVersion == null)
        {
            SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride), true);
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.SubstructureCostProfile);

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
