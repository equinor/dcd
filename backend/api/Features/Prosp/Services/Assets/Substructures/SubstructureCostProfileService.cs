using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

namespace api.Features.Prosp.Services.Assets.Substructures;

public static class SubstructureCostProfileService
{
    public static void AddOrUpdateSubstructureCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile) != null)
        {
            UpdateSubstructureTimeSeries(caseItem, dto);
            return;
        }

        CreateSubstructureCostProfile(caseItem, dto);
    }

    private static void CreateSubstructureCostProfile(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        var costProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.SubstructureCostProfile);

        costProfile.StartYear = dto.StartYear;
        costProfile.Values = dto.Values;

        var costProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride);

        if (costProfileOverride != null)
        {
            costProfileOverride.Override = false;
        }
    }

    private static void UpdateSubstructureTimeSeries(Case caseItem, UpdateTimeSeriesCostDto dto)
    {
        if (caseItem.Substructure.ProspVersion == null)
        {
            if (caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride) != null)
            {
                caseItem.GetProfile(ProfileTypes.SubstructureCostProfileOverride).Override = true;
            }
        }

        var existingProfile = caseItem.GetProfile(ProfileTypes.SubstructureCostProfile);

        existingProfile.StartYear = dto.StartYear;
        existingProfile.Values = dto.Values;
    }
}
