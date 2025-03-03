using api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;
using api.Features.Profiles;
using api.Models;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class Co2IntensityProfileServiceTests
{
    [Fact]
    public void CalculateCo2Intensity_ValidInput_ReturnsCorrectCo2Intensity()
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
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.TotalExportedVolumes,
                    StartYear = -7,
                    Values = [3.23 / 6.29]
                },
                new()
                {
                    ProfileType = ProfileTypes.Co2Emissions,
                    StartYear = -7,
                    Values = [0.0249]
                },
                new()
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = -7,
                    Values = [0.20306]
                }
            }
        };

        // Act
        Co2IntensityProfileService.RunCalculation(caseItem);

        // Assert
        var expectedCo2Intensity = 7.72;
        Assert.Equal(expectedCo2Intensity, caseItem.GetProfile(ProfileTypes.Co2Intensity).Values[0] * 1_000_000, precision: 1);
    }
}
