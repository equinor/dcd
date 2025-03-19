using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

namespace api.Features.Cases.Recalculation.Types.WellCostProfile;

public static class ExplorationWellCostProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var profileTypes = new Dictionary<string, WellCategory>
        {
            { ProfileTypes.ExplorationWellCostProfile, WellCategory.ExplorationWell },
            { ProfileTypes.AppraisalWellCostProfile, WellCategory.AppraisalWell },
            { ProfileTypes.SidetrackCostProfile, WellCategory.Sidetrack }
        };

        foreach (var profileType in profileTypes)
        {
            List<CampaignWell> wells = [];

            foreach (var campaign in caseItem.Campaigns.Where(x => x.CampaignType == CampaignType.ExplorationCampaign))
            {
                foreach (var explorationWell in campaign.CampaignWells)
                {
                    if (explorationWell.Well.WellCategory == profileType.Value)
                    {
                        wells.Add(explorationWell);
                    }
                }
            }

            var profilesToMerge = new List<TimeSeries>();

            foreach (var explorationWell in wells)
            {
                if (explorationWell.Values.Length > 0)
                {
                    profilesToMerge.Add(new TimeSeries
                    {
                        Values = explorationWell.Values.Select(ds => ds * explorationWell.Well.WellCost).ToArray(),
                        StartYear = explorationWell.StartYear
                    });
                }
            }

            var profileValues = TimeSeriesMerger.MergeTimeSeries(profilesToMerge);

            var profile = caseItem.CreateProfileIfNotExists(profileType.Key);

            profile.Values = profileValues.Values;
            profile.StartYear = profileValues.StartYear;
        }
    }
}
