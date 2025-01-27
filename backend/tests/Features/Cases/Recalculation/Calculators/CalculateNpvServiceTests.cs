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
            ExchangeRateUSDToNOK = 10
        };

        var caseItem = new Case
        {
            Id = caseId,
            Project = project,
            DG4Date = new DateTime(DateTime.Now.Year + 6, 1, 1),
            DrainageStrategyLink = Guid.NewGuid(),
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.CalculatedTotalCostCostProfile,
                    StartYear = -3,
                    Values = [2000.0, 4000.0, 1000.0, 1000.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.CalculatedTotalIncomeCostProfile,
                    StartYear = 0,
                    Values = [6217.5, 6217.5, 6217.5, 6217.5, 2958.75, 2958.75]
                }
            }
        };

        CalculateNpvService.CalculateNpv(caseItem);

        var actualNpvValue = 1081.62;
        Assert.Equal(actualNpvValue, caseItem.NPV, precision: 1);
    }
}
