using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

namespace api.Features.Cases.Recalculation.Cost;

public static class WellCostProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var profileTypes = new Dictionary<string, WellCategory>
        {
            { ProfileTypes.OilProducerCostProfile, WellCategory.OilProducer },
            { ProfileTypes.GasProducerCostProfile, WellCategory.GasProducer },
            { ProfileTypes.WaterInjectorCostProfile, WellCategory.WaterInjector },
            { ProfileTypes.GasInjectorCostProfile, WellCategory.GasInjector },

            { ProfileTypes.ExplorationWellCostProfile, WellCategory.ExplorationWell },
            { ProfileTypes.AppraisalWellCostProfile, WellCategory.AppraisalWell },
            { ProfileTypes.SidetrackCostProfile, WellCategory.Sidetrack }
        };

        foreach (var profileType in profileTypes)
        {
            var profileTypeCampaignWells = ResolveCampaignWellsForProfileType(caseItem, profileType.Value);

            var profilesToMerge = new List<TimeSeries>();

            foreach (var campaignWell in profileTypeCampaignWells)
            {
                if (campaignWell.Values.Length > 0)
                {
                    profilesToMerge.Add(new TimeSeries
                    {
                        Values = campaignWell.Values.Select(ds => ds * campaignWell.Well.WellCost).ToArray(),
                        StartYear = campaignWell.StartYear
                    });
                }
            }

            var profileValues = TimeSeriesMerger.MergeTimeSeries(profilesToMerge);

            var profile = caseItem.CreateProfileIfNotExists(profileType.Key);

            profile.Values = profileValues.Values;
            profile.StartYear = profileValues.StartYear;
        }
    }

    private static List<CampaignWell> ResolveCampaignWellsForProfileType(Case caseItem, WellCategory profileType)
    {
        List<CampaignWell> profileTypeCampaignWells = [];

        foreach (var campaign in caseItem.Campaigns)
        {
            foreach (var campaignWell in campaign.CampaignWells)
            {
                if (campaignWell.Well.WellCategory == profileType)
                {
                    profileTypeCampaignWells.Add(campaignWell);
                }
            }
        }

        return profileTypeCampaignWells;
    }
}
