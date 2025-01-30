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
            ExchangeRateUSDToNOK = 10,
        };

        var caseItem = new Case
        {
            Id = caseId,
            Project = project,

            DG4Date = new DateTime(2030, 1, 1),
            DrainageStrategyLink = Guid.NewGuid(),
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.CalculatedTotalCostCostProfile,
                    StartYear = 2027,
                    Values = [2000.0, 4000.0, 1000.0, 1000.0]
                },
                new()
                {
                    ProfileType = ProfileTypes.Co2Emissions,
                    StartYear = 2023,
                    Values = [29400000 / 1000]
                },
                new()
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 2023,
                    Values = [1421000]
                },
            }
        };

        // Act
        Co2IntensityProfileService.RunCalculation(caseItem);

        // Assert
        var expectedCo2Intensity = 3.2899;
        Assert.Equal(expectedCo2Intensity, caseItem.GetProfile(ProfileTypes.Co2Intensity).Values[0], precision: 1);
    }
}
