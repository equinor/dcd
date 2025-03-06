using api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;
using api.Features.Profiles;
using api.Models;

using Xunit;

using static api.Features.Profiles.VolumeConstants;

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
            DrainageStrategyId = Guid.NewGuid(),
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 2020,
                    Values = [1000000.0, 2000000.0, 3000000.0] // SM³
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileOil,
                    StartYear = 2020,
                    Values = [1000000.0, 2000000.0] // SM³
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileGas,
                    StartYear = 2020,
                    Values = [1000000000.0, 2000000000.0, 3000000000.0] // SM³
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileGas,
                    StartYear = 2020,
                    Values = [1000000000.0, 2000000000.0] // SM³
                }
            ]
        };

        // Act
        CalculateTotalIncomeService.RunCalculation(caseItem);

        // Assert
        var expectedFirstYearIncome = (2 * 1000000.0 * 75 * BarrelsPerCubicMeter * 10 + 2 * 1000000000.0 * 3) / 1000000 / caseItem.Project.ExchangeRateUSDToNOK;
        var expectedSecondYearIncome = (4 * 1000000.0 * 75 * BarrelsPerCubicMeter * 10 + 4 * 1000000000.0 * 3) / 1000000 / caseItem.Project.ExchangeRateUSDToNOK;
        var expectedThirdYearIncome = (3 * 1000000.0 * 75 * BarrelsPerCubicMeter * 10 + 3 * 1000000000.0 * 3) / 1000000 / caseItem.Project.ExchangeRateUSDToNOK;

        var calculatedTotalIncomeCostProfileUsd = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfileUsd);
        Assert.NotNull(calculatedTotalIncomeCostProfileUsd);
        Assert.Equal(2020, calculatedTotalIncomeCostProfileUsd.StartYear);
        Assert.Equal(3, calculatedTotalIncomeCostProfileUsd.Values.Length);
        Assert.Equal(expectedFirstYearIncome, calculatedTotalIncomeCostProfileUsd.Values[0], precision: 0);
        Assert.Equal(expectedSecondYearIncome, calculatedTotalIncomeCostProfileUsd.Values[1], precision: 0);
        Assert.Equal(expectedThirdYearIncome, calculatedTotalIncomeCostProfileUsd.Values[2], precision: 0);
    }
}
