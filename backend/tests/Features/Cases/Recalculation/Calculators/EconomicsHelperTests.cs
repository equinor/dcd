using api.Features.Cases.Recalculation.Calculators.Helpers;
using api.Features.Profiles.Dtos;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class EconomicsHelperTests
{
    [Fact]
    public void CalculateCashFlow_ValidInput_ReturnsCorrectCashFlow()
    {
        // Arrange
        var income = new TimeSeries
        {
            StartYear = 2020,
            Values = [500.0, 700.0, 900.0]
        };

        var totalCost = new TimeSeries
        {
            StartYear = 2020,
            Values = [200.0, 300.0, 400.0, 500.0]
        };

        const int expectedStartYear = 2020;

        var expectedValues = new[] { 300.0, 400.0, 500.0, -500.0 };

        // Act
        var result = EconomicsHelper.CalculateCashFlow(income, totalCost);

        // Assert
        Assert.Equal(expectedStartYear, result.StartYear);
        Assert.Equal(expectedValues.Length, result.Values.Length);

        for (var i = 0; i < expectedValues.Length; i++)
        {
            Assert.Equal(expectedValues[i], result.Values[i]);
        }
    }

    [Fact]
    public void GetDiscountFactors_returns_correct_factor()
    {
        const double discountRatePercentage = 8.0;
        const int numYears = 10;

        var expectedValues = new[] { 1.00000, 0.92593, 0.85734, 0.79383, 0.73503, 0.68058, 0.63017, 0.58349, 0.54027, 0.50025 };

        var rates = EconomicsHelper.GetDiscountFactors(discountRatePercentage, numYears);

        for (var i = 0; i < expectedValues.Length; i++)
        {
            Assert.Equal(expectedValues[i], rates[i], 5);
        }
    }
}
