using api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;
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
            DrainageStrategyLink = Guid.NewGuid()
        };

        var drainageStrategy = new DrainageStrategy
        {
            Id = caseItem.DrainageStrategyLink,
            ProductionProfileOil = new ProductionProfileOil
            {
                StartYear = 2020,
                Values = [1000000.0, 2000000.0, 3000000.0] // SM³
            },
            AdditionalProductionProfileOil = new AdditionalProductionProfileOil
            {
                StartYear = 2020,
                Values = [1000000.0, 2000000.0] // SM³
            },
            ProductionProfileGas = new ProductionProfileGas
            {
                StartYear = 2020,
                Values = [1000000000.0, 2000000000.0, 3000000000.0] // SM³
            },
            AdditionalProductionProfileGas = new AdditionalProductionProfileGas
            {
                StartYear = 2020,
                Values = [1000000000.0, 2000000000.0] // SM³
            }
        };

        // Act
        CalculateTotalIncomeService.CalculateTotalIncome(caseItem, drainageStrategy);

        // Assert
        var expectedFirstYearIncome = (2 * 1000000.0 * 75 * 6.29 * 10 + 2 * 1000000000.0 * 3) / 1000000;
        var expectedSecondYearIncome = (4 * 1000000.0 * 75 * 6.29 * 10 + 4 * 1000000000.0 * 3) / 1000000;
        var expectedThirdYearIncome = (3 * 1000000.0 * 75 * 6.29 * 10 + 3 * 1000000000.0 * 3) / 1000000;

        Assert.NotNull(caseItem.CalculatedTotalIncomeCostProfile);
        Assert.Equal(2020, caseItem.CalculatedTotalIncomeCostProfile.StartYear);
        Assert.Equal(3, caseItem.CalculatedTotalIncomeCostProfile.Values.Length);
        Assert.Equal(expectedFirstYearIncome, caseItem.CalculatedTotalIncomeCostProfile.Values[0], precision: 0);
        Assert.Equal(expectedSecondYearIncome, caseItem.CalculatedTotalIncomeCostProfile.Values[1], precision: 0);
        Assert.Equal(expectedThirdYearIncome, caseItem.CalculatedTotalIncomeCostProfile.Values[2], precision: 0);
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
            DrainageStrategyLink = Guid.NewGuid()
        };

        var drainageStrategy = new DrainageStrategy
        {
            Id = caseItem.DrainageStrategyLink,
            ProductionProfileOil = new ProductionProfileOil
            {
                StartYear = 2020,
                Values = [0.0, 0.0, 0.0]
            },
            ProductionProfileGas = new ProductionProfileGas
            {
                StartYear = 2020,
                Values = [0.0, 0.0, 0.0]
            }
        };

        // Act
        CalculateTotalIncomeService.CalculateTotalIncome(caseItem, drainageStrategy);

        // Assert
        Assert.NotNull(caseItem.CalculatedTotalIncomeCostProfile);
        Assert.Equal(2020, caseItem.CalculatedTotalIncomeCostProfile.StartYear);
        Assert.All(caseItem.CalculatedTotalIncomeCostProfile.Values, value => Assert.Equal(0.0, value));
    }
}
