using api.Features.Cases.Recalculation.Cost;
using api.Features.Profiles;
using api.Models;
using api.Models.Enums;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators.OpexCostProfile;

public class CalculateWellInterventionCostProfileTests
{
    [Fact]
    public void CalculateWellInterventionCostProfile_ShouldNotCalculate_WhenOverrideIsEnabled()
    {
        // Arrange
        var caseItem = new Case
        {
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile()
                {
                    ProfileType = ProfileTypes.WellInterventionCostProfileOverride,
                    Values = [],
                    Override = true
                }
            ]
        };

        var developmentWells = new List<CampaignWell>();

        // Act
        OpexCostProfileService.CalculateWellInterventionCostProfile(caseItem, developmentWells, 0, 10);

        // Assert
        Assert.Null(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile));
    }

    [Fact]
    public void CalculateWellInterventionCostProfile_ShouldResetProfile_WhenNoDevelopmentWells()
    {
        // Arrange
        var caseItem = new Case
        {
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.WellInterventionCostProfile,
                    Values = [100, 200, 300],
                    StartYear = 0
                }
            ]
        };

        var developmentWells = new List<CampaignWell>();

        // Act
        OpexCostProfileService.CalculateWellInterventionCostProfile(caseItem, developmentWells, null, null);

        // Assert
        var profile = caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile);
        Assert.NotNull(profile);
        Assert.Empty(profile.Values);
    }

    [Fact]
    public void CalculateWellInterventionCostProfile_ShouldCalculateProfile_WhenDevelopmentWellsExist()
    {
        // Arrange
        var caseItem = new Case
        {
            InitialYearsWithoutWellInterventionCost = 0,
            FinalYearsWithoutWellInterventionCost = 0
        };

        var developmentWells = new List<CampaignWell>
        {
            new()
            {
                Well = new Well
                {
                    WellInterventionCost = 10,
                    WellCost = 0,
                    WellCategory = WellCategory.OilProducer,
                    DrillingDays = 0,
                    Name = "Well 1",
                    PlugingAndAbandonmentCost = 0
                },
                StartYear = 0,
                Values = [1, 2, 3]
            },
            new()
            {
                Well = new Well
                {
                    WellInterventionCost = 20,
                    WellCost = 0,
                    WellCategory = WellCategory.GasProducer,
                    DrillingDays = 0,
                    Name = "Well 2",
                    PlugingAndAbandonmentCost = 0
                },
                StartYear = 0,
                Values = [1, 1, 1]
            }
        };

        // Act
        OpexCostProfileService.CalculateWellInterventionCostProfile(caseItem, developmentWells, 0, 3);

        // Assert
        var profile = caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile);
        Assert.NotNull(profile);
        Assert.Equal(4, profile.Values.Length);
        Assert.Equal(30, profile.Values[0]);
        Assert.Equal(70, profile.Values[1]);
        Assert.Equal(120, profile.Values[2]);
        Assert.Equal(120, profile.Values[3]);
    }

    [Fact]
    public void CalculateWellInterventionCostProfile_ShouldApplyInitialAndFinalYearsWithoutCost()
    {
        // Arrange
        var caseItem = new Case
        {
            InitialYearsWithoutWellInterventionCost = 1,
            FinalYearsWithoutWellInterventionCost = 1
        };

        var developmentWells = new List<CampaignWell>
        {
            new()
            {
                Well = new Well
                {
                    WellInterventionCost = 10,
                    WellCost = 0,
                    WellCategory = WellCategory.OilProducer,
                    DrillingDays = 0,
                    Name = "Well 1",
                    PlugingAndAbandonmentCost = 0
                },
                StartYear = 0,
                Values = [1, 2, 3]
            },
            new()
            {
                Well = new Well
                {
                    WellInterventionCost = 20,
                    WellCost = 0,
                    WellCategory = WellCategory.GasProducer,
                    DrillingDays = 0,
                    Name = "Well 2",
                    PlugingAndAbandonmentCost = 0
                },
                StartYear = 0,
                Values = [1, 1, 1]
            }
        };

        // Act
        OpexCostProfileService.CalculateWellInterventionCostProfile(caseItem, developmentWells, 0, 3);

        // Assert
        var profile = caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile);
        Assert.NotNull(profile);
        Assert.Equal(2, profile.Values.Length);
        Assert.Equal(70, profile.Values[0]);
        Assert.Equal(120, profile.Values[1]);
    }
}
