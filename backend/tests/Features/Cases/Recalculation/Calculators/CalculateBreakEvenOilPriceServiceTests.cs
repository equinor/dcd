using api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;
using api.Features.Profiles;
using api.Models;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class CalculateBreakEvenOilPriceServiceTests
{
    [Fact]
    public void CalculateBreakEvenOilPrice_ReturnsCorrectBreakEvenPrice()
    {
        // Arrange
        var caseId = Guid.NewGuid();

        var project = new Project
        {
            DiscountRate = 8,
            OilPriceUSD = 70,
            GasPriceNOK = 3.59,
            ExchangeRateUSDToNOK = 10,
            NpvYear = 2024
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
                    ProfileType = ProfileTypes.CalculatedTotalCostCostProfileUsd,
                    StartYear = -3,
                    Values = [63.112300000000005, 2.4, 68.1772, 112.5595, 181.9396, 15.2671, 3, 4, 4, 4, 4, 4, 3, 3, 3, 7, 9, 2]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 1,
                    Values = [203120, 240400, 203500, 166700, 156000, 121600, 90100, 66500, 34100, 24300, 2500]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.NetSalesGas,
                    StartYear = 1,
                    Values = [343400000, 406700000, 344100000, 291200000, 312200000, 277100000, 234500000, 198400000, 140300000, 118700000, 12600000]
                }
            ]
        };

        // Act
        CalculateBreakEvenOilPriceService.RunCalculation(caseItem);

        // Assert
        var expectedBreakEvenPrice = 29.82;
        Assert.Equal(expectedBreakEvenPrice, caseItem.BreakEven, precision: 1);
    }
}
