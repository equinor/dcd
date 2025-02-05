using api.Features.Cases.Recalculation.Types.RigCostProfile;
using api.Features.Profiles;
using api.Models;

using Xunit;

namespace tests.Features.Cases.Recalculation.Types;

public class RigCostProfileServiceTests
{
    [Fact]
    public void CalculateRigCostProfile()
    {
        // Arrange
        var explorationCampaign1 = new Campaign
        {
            CampaignType = CampaignTypes.ExplorationCampaign,
            RigUpgradingCost = 2,
            RigMobDemobCost = 5,
            RigUpgradingCostValues = [0.3, 0, 0, 0.7],
            RigUpgradingCostStartYear = -4,
            RigMobDemobCostValues = [0.1, 0.2, 0, 0.6, 0.1],
            RigMobDemobCostStartYear = -1
        };

        var explorationCampaign2 = new Campaign
        {
            CampaignType = CampaignTypes.ExplorationCampaign,
            RigUpgradingCost = 1,
            RigMobDemobCost = 8,
            RigUpgradingCostValues = [0.2, 0.1, 0.7],
            RigUpgradingCostStartYear = -2,
            RigMobDemobCostValues = [0.6, 0, 0.4],
            RigMobDemobCostStartYear = 3
        };

        var caseItem = new Case
        {
            Campaigns = [explorationCampaign1, explorationCampaign2],
        };

        var expectedExplorationRigUpgradingProfile = new TimeSeriesProfile
        {
            ProfileType = ProfileTypes.ExplorationRigUpgradingCostProfile,
            StartYear = -4,
            Values = [0.6, 0, 0.2, 1.5, 0.7]
        };

        var expectedExplorationRigMobDemobProfile = new TimeSeriesProfile
        {
            ProfileType = ProfileTypes.ExplorationRigMobDemob,
            StartYear = -1,
            Values = [0.5, 1, 0, 3, 5.3, 0, 3.2]
        };

        // Act
        RigCostProfileService.RunCalculation(caseItem);

        // Assert
        Assert.Equal(expectedExplorationRigUpgradingProfile.Values, caseItem.GetProfile(ProfileTypes.ExplorationRigUpgradingCostProfile).Values);
        Assert.Equal(expectedExplorationRigUpgradingProfile.StartYear, caseItem.GetProfile(ProfileTypes.ExplorationRigUpgradingCostProfile).StartYear);

        Assert.Equal(expectedExplorationRigMobDemobProfile.Values, caseItem.GetProfile(ProfileTypes.ExplorationRigMobDemob).Values);
        Assert.Equal(expectedExplorationRigMobDemobProfile.StartYear, caseItem.GetProfile(ProfileTypes.ExplorationRigMobDemob).StartYear);
    }
}
