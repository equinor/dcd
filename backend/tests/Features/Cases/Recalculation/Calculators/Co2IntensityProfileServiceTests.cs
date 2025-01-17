using api.Features.Profiles.Cases.GeneratedProfiles.GenerateCo2Intensity;
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
            CalculatedTotalCostCostProfile = new CalculatedTotalCostCostProfile
            {
                StartYear = 2027,
                Values = [2000.0, 4000.0, 1000.0, 1000.0]
            },
        };

        var drainageStrategy = new DrainageStrategy
        {
            Id = caseItem.DrainageStrategyLink,

            ProductionProfileOil = new ProductionProfileOil
            {
                StartYear = 2023,
                Values = [1421000]
            },
            Co2Emissions = new Co2Emissions
            {
                StartYear = 2023,
                Values = [29400000/1000]
            }
        };



        // Act
        Co2IntensityProfileService.CalculateCo2Intensity(drainageStrategy);

        // Assert
        var expectedCo2Intensity = 3.2899;
        Assert.Equal(expectedCo2Intensity, drainageStrategy.Co2Intensity.Values[0], precision: 1);
    }
}
