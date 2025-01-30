using api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;
using api.Features.Profiles;
using api.Models;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class CalculateTotalIncomeServiceTests
{
    [Fact]
    public void CalculateIncome_ValidInput_ReturnsCorrectIncome()
    {
        // Arrange
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
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 2020,
                    Values = [1000000.0, 2000000.0, 3000000.0] // SM³
                },
                new()
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileOil,
                    StartYear = 2020,
                    Values = [1000000.0, 2000000.0] // SM³
                },
                new()
                {
                    ProfileType = ProfileTypes.ProductionProfileGas,
                    StartYear = 2020,
                    Values = [1000000000.0, 2000000000.0, 3000000000.0] // SM³
                },
                new()
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileGas,
                    StartYear = 2020,
                    Values = [1000000000.0, 2000000000.0] // SM³
                }
            }
        };

        // Act
        CalculateTotalIncomeService.RunCalculation(caseItem);

        // Assert
        var expectedFirstYearIncome = (2 * 1000000.0 * 75 * 6.29 * 10 + 2 * 1000000000.0 * 3) / 1000000;
        var expectedSecondYearIncome = (4 * 1000000.0 * 75 * 6.29 * 10 + 4 * 1000000000.0 * 3) / 1000000;
        var expectedThirdYearIncome = (3 * 1000000.0 * 75 * 6.29 * 10 + 3 * 1000000000.0 * 3) / 1000000;

        var calculatedTotalIncomeCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfile);
        Assert.NotNull(calculatedTotalIncomeCostProfile);
        Assert.Equal(2020, calculatedTotalIncomeCostProfile.StartYear);
        Assert.Equal(3, calculatedTotalIncomeCostProfile.Values.Length);
        Assert.Equal(expectedFirstYearIncome, calculatedTotalIncomeCostProfile.Values[0], precision: 0);
        Assert.Equal(expectedSecondYearIncome, calculatedTotalIncomeCostProfile.Values[1], precision: 0);
        Assert.Equal(expectedThirdYearIncome, calculatedTotalIncomeCostProfile.Values[2], precision: 0);
    }

    [Fact]
    public void CalculateIncome_ZeroValues_ReturnsZeroIncome()
    {
        // Arrange
        var caseId = Guid.NewGuid();
        var project = new Project
        {
            Id = Guid.NewGuid(),
            OilPriceUSD = 0,
            GasPriceNOK = 0,
            ExchangeRateUSDToNOK = 0
        };

        var caseItem = new Case
        {
            Id = caseId,
            Project = project,
            ProjectId = project.Id,
            DrainageStrategyLink = Guid.NewGuid(),
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 2020,
                    Values = [0.0, 0.0, 0.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.ProductionProfileGas,
                    StartYear = 2020,
                    Values = [0.0, 0.0, 0.0]
                }
            }
        };

        // Act
        CalculateTotalIncomeService.RunCalculation(caseItem);

        // Assert
        var calculatedTotalIncomeCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfile);
        Assert.NotNull(calculatedTotalIncomeCostProfile);
        Assert.Equal(2020, calculatedTotalIncomeCostProfile.StartYear);
        Assert.All(calculatedTotalIncomeCostProfile.Values, value => Assert.Equal(0.0, value));
    }
}
