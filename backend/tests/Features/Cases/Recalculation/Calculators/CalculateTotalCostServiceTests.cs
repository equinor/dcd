using api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;
using api.Features.Profiles;
using api.Models;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class CalculateTotalCostServiceTests
{
    [Fact]
    public void CalculateTotalCostAsync_AllProfilesProvided_ReturnsCorrectTotalCost()
    {
        var caseId = Guid.NewGuid();
        var project = new Project
        {
            Id = Guid.NewGuid(),
            OilPriceUSD = 75,
            GasPriceNOK = 3,
            ExchangeRateUSDToNOK = 10
        };

        var caseItem = new Case
        {
            Id = caseId,
            Project = project,
            ProjectId = project.Id,
            DrainageStrategyLink = Guid.NewGuid(),
            WellProjectLink = Guid.NewGuid(),
            SubstructureLink = Guid.NewGuid(),
            SurfLink = Guid.NewGuid(),
            TopsideLink = Guid.NewGuid(),
            TransportLink = Guid.NewGuid(),
            OnshorePowerSupplyLink = Guid.NewGuid(),
            ExplorationLink = Guid.NewGuid(),
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.TotalOtherStudiesCostProfile,
                    StartYear = 2020,
                    Values = [1000.0, 1500.0, 2000.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.OnshoreRelatedOPEXCostProfile,
                    StartYear = 2020,
                    Values = [500.0, 600.0, 700.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.CessationWellsCost,
                    StartYear = 2020,
                    Values = [300.0, 400.0, 500.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.TopsideCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.TransportCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 70.0, 100.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.SurfCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [30.0, 60.0, 90.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.SubstructureCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [70.0, 110.0, 150.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.OnshorePowerSupplyCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 70.0, 100.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.GAndGAdminCostOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [100.0, 200.0, 300.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.ExplorationWellCostProfile,
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.AppraisalWellCostProfile,
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.SidetrackCostProfile,
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                }
            }
        };

        var wellProject = new WellProject
        {
            OilProducerCostProfileOverride = new OilProducerCostProfileOverride
            {
                Override = true,
                StartYear = 2020,
                Values = [100.0, 150.0, 200.0]
            },
            GasProducerCostProfileOverride = new GasProducerCostProfileOverride
            {
                Override = true,
                StartYear = 2020,
                Values = [50.0, 80.0, 120.0]
            },
            WaterInjectorCostProfileOverride = new WaterInjectorCostProfileOverride
            {
                Override = true,
                StartYear = 2020,
                Values = [100.0, 100.0, 130.0]
            },
            GasInjectorCostProfileOverride = new GasInjectorCostProfileOverride
            {
                Override = true,
                StartYear = 2020,
                Values = [50.0, 80.0, 120.0]
            },
        };

        var exploration = new Exploration
        {

            SeismicAcquisitionAndProcessing = new SeismicAcquisitionAndProcessing
            {
                StartYear = 2020,
                Values = [150.0, 250.0, 350.0]
            },
            CountryOfficeCost = new CountryOfficeCost
            {
                StartYear = 2020,
                Values = [50.0, 100.0]
            }
        };

        // Act
        CalculateTotalCostService.CalculateTotalCost(caseItem, wellProject, exploration);

        // Assert
        var calculatedTotalCostCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile);
        Assert.NotNull(calculatedTotalCostCostProfile);
        Assert.Equal(2020, calculatedTotalCostCostProfile.StartYear);
        Assert.Equal(3, calculatedTotalCostCostProfile.Values.Length);

        var expectedValues = new[] { 2950.0, 4150.0, 4980.0 };
        for (int i = 0; i < expectedValues.Length; i++)
        {
            Assert.Equal(expectedValues[i], calculatedTotalCostCostProfile.Values[i]);
        }
    }

    [Fact]
    public void CalculateTotalExplorationCostAsync_ValidInput_ReturnsCorrectTotalExplorationCost()
    {
        // Arrange
        var caseItem = new Case
        {
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.GAndGAdminCostOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [10.0, 20.0, 30.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.ExplorationWellCostProfile,
                    StartYear = 2021,
                    Values = [50.0, 80.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.AppraisalWellCostProfile,
                    StartYear = 2020,
                    Values = [40.0, 60.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.SidetrackCostProfile,
                    StartYear = 2021,
                    Values = [20.0, 30.0]
                }
            }
        };

        var exploration = new Exploration
        {
            SeismicAcquisitionAndProcessing = new SeismicAcquisitionAndProcessing
            {
                StartYear = 2021,
                Values = [15.0, 25.0, 35.0]
            },
            CountryOfficeCost = new CountryOfficeCost
            {
                StartYear = 2020,
                Values = [5.0, 10.0]
            }
        };

        // Act
        var result = CalculateTotalCostService.CalculateTotalExplorationCost(caseItem, exploration);

        // Assert
        var expectedStartYear = 2020;
        var expectedValues = new[] { 55.0, 175.0, 165.0, 35.0 };

        Assert.Equal(expectedStartYear, result.StartYear);
        Assert.Equal(expectedValues.Length, result.Values.Length);

        for (int i = 0; i < expectedValues.Length; i++)
        {
            Assert.Equal(expectedValues[i], result.Values[i]);
        }
    }
}
