using api.Features.Cases.Recalculation.Calculators.CalculateNpv;
using api.Features.Profiles;
using api.Models;

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
            OilPriceUSD = 75,
            GasPriceNOK = 3,
            ExchangeRateUSDToNOK = 10,
            NpvYear = 2020
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

        var expectedNpvValue = 869.2;
        Assert.Equal(expectedNpvValue, caseItem.NPV, precision: 1);
    }
}
