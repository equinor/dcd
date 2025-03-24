using api.Features.Profiles;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Transports;

public static class TransportCostProfileService
{
    public static void AddOrUpdateTransportCostProfile(Case caseItem, int startYear, double[] values, bool overrideValue)
    {
        var overrideProfile = caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride);

        if (overrideProfile != null)
        {
            overrideProfile.Override = overrideValue;
        }

        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.TransportCostProfile);

        costProfile.StartYear = startYear;
        costProfile.Values = values;
    }
}
