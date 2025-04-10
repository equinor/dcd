using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

namespace api.Features.Recalculation.Cost;

// dependency order 1
public static class RigCostProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var explorationRigUpgradingList = CreateTimeSeriesList(
            caseItem,
            CampaignType.ExplorationCampaign,
            c => c.RigUpgradingCostValues,
            c => c.RigUpgradingCost,
            c => c.RigUpgradingCostStartYear
        );

        var explorationRigMobDemobList = CreateTimeSeriesList(
            caseItem,
            CampaignType.ExplorationCampaign,
            c => c.RigMobDemobCostValues,
            c => c.RigMobDemobCost,
            c => c.RigMobDemobCostStartYear
        );

        var developmentRigUpgradingList = CreateTimeSeriesList(
            caseItem,
            CampaignType.DevelopmentCampaign,
            c => c.RigUpgradingCostValues,
            c => c.RigUpgradingCost,
            c => c.RigUpgradingCostStartYear
        );

        var developmentRigMobDemobList = CreateTimeSeriesList(
            caseItem,
            CampaignType.DevelopmentCampaign,
            c => c.RigMobDemobCostValues,
            c => c.RigMobDemobCost,
            c => c.RigMobDemobCostStartYear
        );

        MergeAndSetProfile(caseItem, explorationRigUpgradingList, ProfileTypes.ExplorationRigUpgradingCostProfile);
        MergeAndSetProfile(caseItem, explorationRigMobDemobList, ProfileTypes.ExplorationRigMobDemob);
        MergeAndSetProfile(caseItem, developmentRigUpgradingList, ProfileTypes.DevelopmentRigUpgradingCostProfile);
        MergeAndSetProfile(caseItem, developmentRigMobDemobList, ProfileTypes.DevelopmentRigMobDemob);
    }

    private static List<TimeSeries> CreateTimeSeriesList(
        Case caseItem,
        CampaignType campaignType,
        Func<Campaign, double[]> valueSelector,
        Func<Campaign, double> costSelector,
        Func<Campaign, int> startYearSelector
    )
    {
        return caseItem.Campaigns
            .Where(c => c.CampaignType == campaignType)
            .Select(c => new TimeSeries
            {
                Values = valueSelector(c).Select(v => v * costSelector(c)).ToArray(),
                StartYear = startYearSelector(c)
            })
            .ToList();
    }

    private static void MergeAndSetProfile(Case caseItem, List<TimeSeries> timeSeriesList, string profileType)
    {
        var mergedTimeSeries = TimeSeriesMerger.MergeTimeSeries(timeSeriesList);
        var profile = caseItem.CreateProfileIfNotExists(profileType);
        profile.StartYear = mergedTimeSeries.StartYear;
        profile.Values = mergedTimeSeries.Values;
    }
}
