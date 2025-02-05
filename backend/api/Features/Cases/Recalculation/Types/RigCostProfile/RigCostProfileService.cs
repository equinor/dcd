using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.RigCostProfile;

public static class RigCostProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var explorationRigUpgradingList = CreateTimeSeriesList(
            caseItem,
            CampaignTypes.ExplorationCampaign,
            c => c.RigUpgradingCostValues,
            c => c.RigUpgradingCost,
            c => c.RigUpgradingCostStartYear
        );
        var explorationRigMobDemobList = CreateTimeSeriesList(
            caseItem,
            CampaignTypes.ExplorationCampaign,
            c => c.RigMobDemobCostValues,
            c => c.RigMobDemobCost,
            c => c.RigMobDemobCostStartYear
        );

        var developmentRigUpgradingList = CreateTimeSeriesList(
            caseItem,
            CampaignTypes.DevelopmentCampaign,
            c => c.RigUpgradingCostValues,
            c => c.RigUpgradingCost,
            c => c.RigUpgradingCostStartYear
        );
        var developmentRigMobDemobList = CreateTimeSeriesList(
            caseItem,
            CampaignTypes.DevelopmentCampaign,
            c => c.RigMobDemobCostValues,
            c => c.RigMobDemobCost,
            c => c.RigMobDemobCostStartYear
        );

        MergeAndSetProfile(caseItem, explorationRigUpgradingList, ProfileTypes.ExplorationRigUpgradingCostProfile);
        MergeAndSetProfile(caseItem, explorationRigMobDemobList, ProfileTypes.ExplorationRigMobDemob);
        MergeAndSetProfile(caseItem, developmentRigUpgradingList, ProfileTypes.DevelopmentRigUpgradingCostProfile);
        MergeAndSetProfile(caseItem, developmentRigMobDemobList, ProfileTypes.DevelopmentRigMobDemob);
    }

    private static List<TimeSeriesCost> CreateTimeSeriesList(
        Case caseItem,
        string campaignType,
        Func<Campaign, double[]> valueSelector,
        Func<Campaign, double> costSelector,
        Func<Campaign, int> startYearSelector
    )
    {
        return caseItem.Campaigns
            .Where(c => c.CampaignType == campaignType)
            .Select(c => new TimeSeriesCost
            {
                Values = valueSelector(c).Select(v => v * costSelector(c)).ToArray(),
                StartYear = startYearSelector(c)
            })
            .ToList();
    }

    private static void MergeAndSetProfile(Case caseItem, List<TimeSeriesCost> timeSeriesList, string profileType)
    {
        var mergedTimeSeries = TimeSeriesMerger.MergeTimeSeries(timeSeriesList);
        var profile = caseItem.CreateProfileIfNotExists(profileType);
        profile.StartYear = mergedTimeSeries.StartYear;
        profile.Values = mergedTimeSeries.Values;
    }
}
