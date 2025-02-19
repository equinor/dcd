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
}
