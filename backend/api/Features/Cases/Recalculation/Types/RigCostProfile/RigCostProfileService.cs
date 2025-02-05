using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.RigCostProfile;

public class RigCostProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var explorationRigUpgradingList = new List<TimeSeriesCost>();
        var explorationRigMobDemobList = new List<TimeSeriesCost>();

        var developmentRigUpgradingList = new List<TimeSeriesCost>();
        var developmentRigMobDemobList = new List<TimeSeriesCost>();

        foreach (var campaign in caseItem.Campaigns)
        {
            var campaignRigUpgrading = new TimeSeriesCost
            {
                Values = campaign.RigUpgradingCostValues.Select(v => v * campaign.RigUpgradingCost).ToArray(),
                StartYear = campaign.RigUpgradingCostStartYear,
            };

            var campaignRigMobDemob = new TimeSeriesCost
            {
                Values = campaign.RigMobDemobCostValues.Select(v => v * campaign.RigMobDemobCost).ToArray(),
                StartYear = campaign.RigMobDemobCostStartYear,
            };

            if (campaign.CampaignType == CampaignTypes.ExplorationCampaign)
            {
                explorationRigUpgradingList.Add(campaignRigUpgrading);
                explorationRigMobDemobList.Add(campaignRigMobDemob);
            }
            else
            {
                developmentRigUpgradingList.Add(campaignRigUpgrading);
                developmentRigMobDemobList.Add(campaignRigMobDemob);
            }
        }

        var explorationRigUpgrading = TimeSeriesMerger.MergeTimeSeries(explorationRigUpgradingList);
        var caseExplorationRigUpgrading = caseItem.CreateProfileIfNotExists(ProfileTypes.ExplorationRigUpgradingCostProfile);
        caseExplorationRigUpgrading.StartYear = explorationRigUpgrading.StartYear;
        caseExplorationRigUpgrading.Values = explorationRigUpgrading.Values;

        var explorationRigMobDemob  = TimeSeriesMerger.MergeTimeSeries(explorationRigMobDemobList);
        var caseExplorationRigMobDemob = caseItem.CreateProfileIfNotExists(ProfileTypes.ExplorationRigMobDemob);
        caseExplorationRigMobDemob.StartYear = explorationRigMobDemob.StartYear;
        caseExplorationRigMobDemob.Values = explorationRigMobDemob.Values;

        var developmentRigUpgrading = TimeSeriesMerger.MergeTimeSeries(developmentRigUpgradingList);
        var caseDevelopmentRigUpgrading = caseItem.GetProfile(ProfileTypes.DevelopmentRigUpgradingCostProfile);
        caseDevelopmentRigUpgrading.StartYear = caseDevelopmentRigUpgrading.StartYear;
        caseDevelopmentRigUpgrading.Values = developmentRigUpgrading.Values;

        var developmentRigMobDemob = TimeSeriesMerger.MergeTimeSeries(developmentRigMobDemobList);
        var caseDevelopmentRigMobDemob = caseItem.GetProfile(ProfileTypes.DevelopmentRigMobDemob);
        caseDevelopmentRigMobDemob.StartYear = caseDevelopmentRigMobDemob.StartYear;
        caseDevelopmentRigMobDemob.Values = developmentRigMobDemob.Values;
    }
}
