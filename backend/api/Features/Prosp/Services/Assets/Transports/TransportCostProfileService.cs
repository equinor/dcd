using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Transports;

public static class TransportCostProfileService
{
    public static void AddOrUpdateTransportCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile) != null)
        {
            UpdateTransportCostProfile(caseItem, dto);
            return;
        }

        CreateTransportCostProfile(caseItem, dto);
    }

    private static void CreateTransportCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.TransportCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }
    }

    private static void UpdateTransportCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.Transport.ProspVersion == null)
        {
            if (caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride) != null)
            {
                caseItem.GetProfile(ProfileTypes.TransportCostProfileOverride).Override = true;
            }
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.TransportCostProfile);

        existingProfile.StartYear = dto.StartYear;
        existingProfile.Values = dto.Values;
    }
}
