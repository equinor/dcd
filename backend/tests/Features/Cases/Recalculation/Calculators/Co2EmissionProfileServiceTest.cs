using api.Features.Cases.Recalculation.Types.Co2EmissionsProfile;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.TimeSeriesCalculators;
using api.Models;

using DocumentFormat.OpenXml.Drawing.Charts;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class Co2EmissionProfileServiceTests
{

    [Fact]
    public void CalculateFuelFlaringAndLosses_ReturnsCorrectValue()
    {
        // Arrange
        var project = new Project
        {
            FlaredGasPerProducedVolume = 1.122765,
            CO2RemovedFromGas = 0.0,
        };

        var caseItem = new Case
        {
            FacilitiesAvailability = 93, // 93%
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 2023,
                    Values = [100000, 150000, 130000, 110000]
                },
                new()
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileOil,
                    StartYear = 2023,
                    Values = [60000]
                },
                new()
                {
                    ProfileType = ProfileTypes.ProductionProfileGas,
                    StartYear = 2023,
                    Values = [200000000, 250000000, 220000000, 200000000]
                },
                new()
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileGas,
                    StartYear = 2023,
                    Values = [80000000]
                }
        }
        };
        var topside = new Topside
        {
            FuelConsumption = 0.019,
            OilCapacity = 555,
            CO2ShareOilProfile = 0.173,
            CO2OnMaxOilProfile = 0.208,
            GasCapacity = 0.96,
            CO2ShareGasProfile = 0.827,
            CO2OnMaxGasProfile = 0.057,
            WaterInjectionCapacity = 0,
            CO2ShareWaterInjectionProfile = 0,
            CO2OnMaxWaterInjectionProfile = 0,
        };

        // Act
        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside);
        var flaring = EmissionCalculationHelper.CalculateFlaring(project, caseItem);
        var losses = EmissionCalculationHelper.CalculateLosses(project, caseItem);

        var total = CostProfileMerger.MergeCostProfiles([fuelConsumptions, flaring, losses]);

        // Assert
        var expectedTotalFuelConsumptions = new List<double> { 0.00595393613229385 * 1000000000, 0.005467970774412518 * 1000000000, 0.004930208439871723 * 1000000000, 0.004537168899071554 * 1000000000 };
        for (int i = 0; i < expectedTotalFuelConsumptions.Count; i++)
        {
            Assert.Equal(expectedTotalFuelConsumptions[i], total.Values[i], precision: 1);
        }
    }

    [Fact]
    public void CalculateFuelFlaringAndLosses_WithCompleteDataInput_ReturnsCorrectValue()
    {
        // Arrange
        var project = new Project
        {
            FlaredGasPerProducedVolume = 1.122765,
            CO2RemovedFromGas = 0.0,
        };

        var caseItem = new Case
        {
            FacilitiesAvailability = 93, // 93%
        };

        var drainageStrategy = new DrainageStrategy
        {
            ProductionProfileOil = new ProductionProfileOil
            {
                StartYear = 2023,
                Values = [100000, 150000, 130000, 110000]
            },
            AdditionalProductionProfileOil = new AdditionalProductionProfileOil
            {
                StartYear = 2023,
                Values = [60000]
            },
            ProductionProfileGas = new ProductionProfileGas
            {
                StartYear = 2023,
                Values = [200000000, 250000000, 220000000, 200000000]
            },
            AdditionalProductionProfileGas = new AdditionalProductionProfileGas
            {
                StartYear = 2023,
                Values = [80000000]
            },
            ProductionProfileWaterInjection = new ProductionProfileWaterInjection
            {
                StartYear = 2023,
                Values = [1180000]
            },
        };

        var topside = new Topside
        {
            FuelConsumption = 0.019,
            OilCapacity = 555,
            CO2ShareOilProfile = 0.173,
            CO2OnMaxOilProfile = 0.208,
            GasCapacity = 0.96,
            CO2ShareGasProfile = 0.827,
            CO2OnMaxGasProfile = 0.057,
            WaterInjectionCapacity = 0,
            CO2ShareWaterInjectionProfile = 0,
            CO2OnMaxWaterInjectionProfile = 0,
        };

        // Act
        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside);
        var flaring = EmissionCalculationHelper.CalculateFlaring(project, caseItem);
        var losses = EmissionCalculationHelper.CalculateLosses(project, caseItem);

        var total = CostProfileMerger.MergeCostProfiles([fuelConsumptions, flaring, losses]);

        // Assert
        var expectedTotalFuelConsumptions = new List<double> { 0.00595393613229385 * 1000000000, 0.005467970774412518 * 1000000000, 0.004930208439871723 * 1000000000, 0.004537168899071554 * 1000000000 };
        for (int i = 0; i < expectedTotalFuelConsumptions.Count; i++)
        {
            Assert.Equal(expectedTotalFuelConsumptions[i], total.Values[i], precision: 1);
        }
    }

}
