using api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;
using api.Features.Profiles;
using api.Models;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class CalculateBreakEvenOilPriceServiceTests
{
    [Fact]
    public void CalculateBreakEvenOilPrice_ValidCaseId_ReturnsCorrectBreakEvenPrice()
    {
        // Arrange
        var caseId = Guid.NewGuid();
        var project = new Project
        {
            DiscountRate = 8,
            OilPriceUSD = 75,
            GasPriceNOK = 3,
            ExchangeRateUSDToNOK = 10
        };

        var caseItem = new Case
        {
            Id = caseId,
            Project = project,
            DG4Date = new DateTime(2030, 1, 1),
            DrainageStrategyLink = Guid.NewGuid(),
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.CalculatedTotalCostCostProfile,
                    StartYear = 2027,
                    Values = [2000.0, 4000.0, 1000.0, 1000.0]
                }
            }
        };

        var drainageStrategy = new DrainageStrategy
        {
            Id = caseItem.DrainageStrategyLink,

            ProductionProfileOil = new ProductionProfileOil
            {
                StartYear = 2030,
                Values = [1000000.0, 1000000.0, 1000000.0, 1000000.0, 500000.0, 500000.0]
            },
            ProductionProfileGas = new ProductionProfileGas
            {
                StartYear = 2030,
                Values = [500000000.0, 500000000.0, 500000000.0, 500000000.0, 200000000.0, 200000000.0]
            },
        };

        // Act
        CalculateBreakEvenOilPriceService.CalculateBreakEvenOilPrice(caseItem, drainageStrategy);

        // Assert
        var expectedBreakEvenPrice = 26.29;
        Assert.Equal(expectedBreakEvenPrice, caseItem.BreakEven, precision: 1);
    }
}
