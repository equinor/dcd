using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Transports;

public static class TransportCostProfileService
{
    public static void AddOrUpdateTransportCostProfile(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile) != null)
        {
            UpdateTransportCostProfile(caseItem, startYear, values);

            return;
        }

        CreateTransportCostProfile(caseItem, startYear, values);
    }

    private static void CreateTransportCostProfile(Case caseItem, int startYear, double[] values)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.TransportCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;

        SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride), false);
    }

    private static void UpdateTransportCostProfile(Case caseItem, int startYear, double[] values)
    {
        if (caseItem.Transport.ProspVersion == null)
        {
            SetOverrideFlag(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride), true);
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.TransportCostProfile);

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
