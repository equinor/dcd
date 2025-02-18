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
            DrainageStrategyId = Guid.NewGuid(),
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.CalculatedTotalCostCostProfile,
                    StartYear = 2027,
                    Values = [200.0, 400.0, 100.0, 100.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 2030,
                    Values = [1000000.0, 1000000.0, 1000000.0, 1000000.0, 500000.0, 500000.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileGas,
                    StartYear = 2030,
                    Values = [500000000.0, 500000000.0, 500000000.0, 500000000.0, 200000000.0, 200000000.0]
                }
            ]
        };

        // Act
        CalculateBreakEvenOilPriceService.RunCalculation(caseItem);

        // Assert
        var expectedBreakEvenPrice = 26.29;
        Assert.Equal(expectedBreakEvenPrice, caseItem.BreakEven, precision: 1);
    }
}
