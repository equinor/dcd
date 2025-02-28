using api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;
using api.Features.Profiles;
using api.Models;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class CalculateTotalCostServiceTests
{
    [Fact]
    public void CalculateTotalCostAsync_AllProfilesProvided_ReturnsCorrectTotalCostInUsd()
    {
        var caseId = Guid.NewGuid();
        var project = new Project
        {
            Id = Guid.NewGuid(),
            OilPriceUSD = 75,
            GasPriceNOK = 3,
            ExchangeRateUSDToNOK = 10,
            Currency = api.Models.Enums.Currency.NOK
        };

        var caseItem = new Case
        {
            Id = caseId,
            Project = project,
            ProjectId = project.Id,
            DrainageStrategyId = Guid.NewGuid(),
            SubstructureId = Guid.NewGuid(),
            SurfId = Guid.NewGuid(),
            TopsideId = Guid.NewGuid(),
            TransportId = Guid.NewGuid(),
            OnshorePowerSupplyId = Guid.NewGuid(),
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.TotalOtherStudiesCostProfile,
                    StartYear = 2020,
                    Values = [1000.0, 1500.0, 2000.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.OnshoreRelatedOPEXCostProfile,
                    StartYear = 2020,
                    Values = [500.0, 600.0, 700.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.CessationWellsCost,
                    StartYear = 2020,
                    Values = [300.0, 400.0, 500.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.TopsideCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.TransportCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 70.0, 100.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.SurfCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [30.0, 60.0, 90.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.SubstructureCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [70.0, 110.0, 150.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.OnshorePowerSupplyCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 70.0, 100.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.GAndGAdminCostOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [100.0, 200.0, 300.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ExplorationWellCostProfile,
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AppraisalWellCostProfile,
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.SidetrackCostProfile,
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.SeismicAcquisitionAndProcessing,
                    StartYear = 2020,
                    Values = [150.0, 250.0, 350.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.CountryOfficeCost,
                    StartYear = 2020,
                    Values = [50.0, 100.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.OilProducerCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [100.0, 150.0, 200.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.GasProducerCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.WaterInjectorCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [100.0, 100.0, 130.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.GasInjectorCostProfileOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                }

            ]
        };

        // Act
        CalculateTotalCostService.RunCalculation(caseItem);

        // Assert
        var calculatedTotalCostCostProfileUsd = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfileUsd);
        Assert.NotNull(calculatedTotalCostCostProfileUsd);
        Assert.Equal(2020, calculatedTotalCostCostProfileUsd.StartYear);
        Assert.Equal(3, calculatedTotalCostCostProfileUsd.Values.Length);

        var expectedValues = new[] { 295, 415, 498 };
        for (int i = 0; i < expectedValues.Length; i++)
        {
            Assert.Equal(expectedValues[i], calculatedTotalCostCostProfileUsd.Values[i]);
        }
    }

    [Fact]
    public void CalculateTotalExplorationCostAsync_ValidInput_ReturnsCorrectTotalExplorationCost()
    {
        // Arrange
        var caseItem = new Case
        {
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.GAndGAdminCostOverride,
                    Override = true,
                    StartYear = 2020,
                    Values = [10.0, 20.0, 30.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ExplorationWellCostProfile,
                    StartYear = 2021,
                    Values = [50.0, 80.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AppraisalWellCostProfile,
                    StartYear = 2020,
                    Values = [40.0, 60.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.SidetrackCostProfile,
                    StartYear = 2021,
                    Values = [20.0, 30.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.SeismicAcquisitionAndProcessing,
                    StartYear = 2021,
                    Values = [15.0, 25.0, 35.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.CountryOfficeCost,
                    StartYear = 2020,
                    Values = [5.0, 10.0]
                }
            ]
        };

        // Act
        var result = CalculateTotalCostService.CalculateTotalExplorationCost(caseItem);

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
