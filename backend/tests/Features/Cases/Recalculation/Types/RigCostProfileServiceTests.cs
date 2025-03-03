using api.Features.Cases.Recalculation.Types.RigCostProfile;
using api.Features.Profiles;
using api.Models;
using api.Models.Enums;

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
            CampaignType = CampaignType.ExplorationCampaign,
            RigUpgradingCost = 2,
            RigMobDemobCost = 5,
            RigUpgradingCostValues = [0.3, 0, 0, 0.7],
            RigUpgradingCostStartYear = -4,
            RigMobDemobCostValues = [0.1, 0.2, 0, 0.6, 0.1],
            RigMobDemobCostStartYear = -1
        };

        var explorationCampaign2 = new Campaign
        {
            CampaignType = CampaignType.ExplorationCampaign,
            RigUpgradingCost = 1,
            RigMobDemobCost = 8,
            RigUpgradingCostValues = [0.2, 0.1, 0.7],
            RigUpgradingCostStartYear = -2,
            RigMobDemobCostValues = [0.6, 0, 0.4],
            RigMobDemobCostStartYear = 3
        };

        var developmentCampaign = new Campaign
        {
            CampaignType = CampaignType.DevelopmentCampaign,
            RigUpgradingCost = 3,
            RigMobDemobCost = 4,
            RigUpgradingCostValues = [0.5, 0.2, 0.3],
            RigUpgradingCostStartYear = -1,
            RigMobDemobCostValues = [0.3, 0.1, 0.2, 0, 0, 0.4],
            RigMobDemobCostStartYear = 0
        };

        var caseItem = new Case
        {
            Campaigns = [explorationCampaign1, developmentCampaign, explorationCampaign2]
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

        var expectedDevelopmentRigUpgradingProfile = new TimeSeriesProfile
        {
            ProfileType = ProfileTypes.DevelopmentRigUpgradingCostProfile,
            StartYear = -1,
            Values = [1.5, 0.6, 0.9]
        };

        var expectedDevelopmentRigMobDemobProfile = new TimeSeriesProfile
        {
            ProfileType = ProfileTypes.DevelopmentRigMobDemob,
            StartYear = 0,
            Values = [1.2, 0.4, 0.8, 0, 0, 1.6]
        };

        // Act
        RigCostProfileService.RunCalculation(caseItem);

        // Assert
        AssertEqualWithTolerance(expectedExplorationRigUpgradingProfile.Values, caseItem.GetProfile(ProfileTypes.ExplorationRigUpgradingCostProfile).Values);
        Assert.Equal(expectedExplorationRigUpgradingProfile.StartYear, caseItem.GetProfile(ProfileTypes.ExplorationRigUpgradingCostProfile).StartYear);

        AssertEqualWithTolerance(expectedExplorationRigMobDemobProfile.Values, caseItem.GetProfile(ProfileTypes.ExplorationRigMobDemob).Values);
        Assert.Equal(expectedExplorationRigMobDemobProfile.StartYear, caseItem.GetProfile(ProfileTypes.ExplorationRigMobDemob).StartYear);

        AssertEqualWithTolerance(expectedDevelopmentRigUpgradingProfile.Values, caseItem.GetProfile(ProfileTypes.DevelopmentRigUpgradingCostProfile).Values);
        Assert.Equal(expectedDevelopmentRigUpgradingProfile.StartYear, caseItem.GetProfile(ProfileTypes.DevelopmentRigUpgradingCostProfile).StartYear);

        AssertEqualWithTolerance(expectedDevelopmentRigMobDemobProfile.Values, caseItem.GetProfile(ProfileTypes.DevelopmentRigMobDemob).Values);
        Assert.Equal(expectedDevelopmentRigMobDemobProfile.StartYear, caseItem.GetProfile(ProfileTypes.DevelopmentRigMobDemob).StartYear);
    }

    private static void AssertEqualWithTolerance(double[] expected, double[] actual, double tolerance = 1e-9)
    {
        Assert.Equal(expected.Length, actual.Length);

        for (var i = 0; i < expected.Length; i++)
        {
            Assert.True(Math.Abs(expected[i] - actual[i]) < tolerance, $"Expected: {expected[i]}, Actual: {actual[i]}");
        }
    }
}
