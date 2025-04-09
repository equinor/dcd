using api.Features.Profiles;
using api.Features.Recalculation.RevenuesAndCashflow;
using api.Models;
using api.Models.Enums;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class CalculateNpvServiceTests
{
    [Fact]
    public void CalculateNPV_ValidCaseId_ReturnsCorrectNPV()
    {
        // Arrange
        var caseId = Guid.NewGuid();

        var project = new Project
        {
            DiscountRate = 8,
            OilPriceUsd = 75,
            GasPriceNok = 3,
            ExchangeRateUsdToNok = 10,
            NpvYear = 2020,
            Currency = Currency.Usd
        };

        var caseItem = new Case
        {
            Id = caseId,
            Project = project,
            Dg4Date = new DateTime(2030, 1, 1),
            DrainageStrategyId = Guid.NewGuid(),
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.CalculatedTotalCostCostProfile,
                    StartYear = -10,
                    Values = [200.0, 400.0, 100.0, 100.0]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.CalculatedTotalIncomeCostProfile,
                    StartYear = -7,
                    Values = [4717.5, 4717.5, 4717.5, 4717.5, 2358.75, 2358.75]
                }
            ]
        };

        CalculateNpvService.RunCalculation(caseItem);

        const double expectedNpvValue = 15311.106;
        Assert.Equal(expectedNpvValue, caseItem.Npv, precision: 1);
    }
}
