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
        var income = new TimeSeriesCost
        {
            StartYear = 2020,
            Values = [500.0, 700.0, 900.0]
        };

        var totalCost = new TimeSeriesCost
        {
            StartYear = 2020,
            Values = [200.0, 300.0, 400.0, 500.0]
        };

        var expectedStartYear = 2020;

        var expectedValues = new[] { 300.0, 400.0, 500.0, -500.0 };

        // Act
        var result = EconomicsHelper.CalculateCashFlow(income, totalCost);

        // Assert
        Assert.Equal(expectedStartYear, result.StartYear);
        Assert.Equal(expectedValues.Length, result.Values.Length);

        for (int i = 0; i < expectedValues.Length; i++)
        {
            Assert.Equal(expectedValues[i], result.Values[i]);
        }
    }

    [Fact]
    public void CalculateDiscountedVolume_ValidInput_ReturnsCorrectDiscountedVolume()
    {
        // Arrange
        var values = new[] { 1.0, 1.0, 1.0, 1.0, 0.5, 0.5 };
        var discountRate = 8;
        var startIndex = 0; // Assuming starting from 2030

        // Act
        var discountedVolume = EconomicsHelper.CalculateDiscountedVolume(values, discountRate, startIndex);

        // Assert
        var expectedDiscountedVolume = (1.0 / Math.Pow(1 + 0.08, 0)) +
                                       (1.0 / Math.Pow(1 + 0.08, 1)) +
                                       (1.0 / Math.Pow(1 + 0.08, 2)) +
                                       (1.0 / Math.Pow(1 + 0.08, 3)) +
                                       (0.5 / Math.Pow(1 + 0.08, 4)) +
                                       (0.5 / Math.Pow(1 + 0.08, 5));
        Assert.Equal(expectedDiscountedVolume, discountedVolume, precision: 5);
    }
}
