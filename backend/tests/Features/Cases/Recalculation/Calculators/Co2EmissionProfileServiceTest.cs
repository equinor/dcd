using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class Co2EmissionProfileServiceTests
{
    [Fact]
    public void CalculateFuelFlaringAndLosses_ReturnsCorrectValue()
    {
        // Arrange
        var caseItem = new Case
        {
            Project = new Project
            {
                FlaredGasPerProducedVolume = 1.122765,
                Co2RemovedFromGas = 0.0,
                DailyEmissionFromDrillingRig = 117
            },
            FacilitiesAvailability = 93, // 93%
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 2023,
                    Values = [100000, 150000, 130000, 110000]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileOil,
                    StartYear = 2023,
                    Values = [60000]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileGas,
                    StartYear = 2023,
                    Values = [200000000, 250000000, 220000000, 200000000]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileGas,
                    StartYear = 2023,
                    Values = [80000000]
                }
            ],
            Topside = new Topside
            {
                FuelConsumption = 0.019,
                OilCapacity = 555,
                Co2ShareOilProfile = 0.173,
                Co2OnMaxOilProfile = 0.208,
                GasCapacity = 0.96,
                Co2ShareGasProfile = 0.827,
                Co2OnMaxGasProfile = 0.057,
                WaterInjectionCapacity = 0,
                Co2ShareWaterInjectionProfile = 0,
                Co2OnMaxWaterInjectionProfile = 0,

                DryWeight = 0,
                ArtificialLift = ArtificialLift.NoArtificialLift,
                Maturity = Maturity.A,
                FlaredGas = 0,
                ProducerCount = 0,
                GasInjectorCount = 0,
                WaterInjectorCount = 0,
                CostYear = 0,
                ProspVersion = null,
                Source = Source.ConceptApp,
                ApprovedBy = "",
                FacilityOpex = 0,
                PeakElectricityImported = 0
            }
        };

        // Act
        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem);
        var flaring = EmissionCalculationHelper.CalculateFlaring(caseItem);
        var losses = EmissionCalculationHelper.CalculateLosses(caseItem);

        var total = TimeSeriesMerger.MergeTimeSeries(fuelConsumptions, flaring, losses);

        // Assert
        var expectedTotalFuelConsumptions = new List<double> { 6102721.5 };

        for (var i = 0; i < expectedTotalFuelConsumptions.Count; i++)
        {
            Assert.Equal(expectedTotalFuelConsumptions[i], total.Values[i], precision: 1);
        }
    }

    [Fact]
    public void CalculateFuelFlaringAndLosses_WithCompleteDataInput_ReturnsCorrectValue()
    {
        // Arrange
        var caseItem = new Case
        {
            Project = new Project
            {
                FlaredGasPerProducedVolume = 1.122765,
                Co2RemovedFromGas = 0.0
            },
            FacilitiesAvailability = 93, // 93%
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 2023,
                    Values = [100000, 150000, 130000, 110000]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileOil,
                    StartYear = 2023,
                    Values = [60000]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileGas,
                    StartYear = 2023,
                    Values = [200000000, 250000000, 220000000, 200000000]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileGas,
                    StartYear = 2023,
                    Values = [80000000]
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileWaterInjection,
                    StartYear = 2023,
                    Values = [1180000]
                }
            ],
            Topside = new Topside
            {
                FuelConsumption = 0.019,
                OilCapacity = 555,
                Co2ShareOilProfile = 0.173,
                Co2OnMaxOilProfile = 0.208,
                GasCapacity = 0.96,
                Co2ShareGasProfile = 0.827,
                Co2OnMaxGasProfile = 0.057,
                WaterInjectionCapacity = 0,
                Co2ShareWaterInjectionProfile = 0,
                Co2OnMaxWaterInjectionProfile = 0,

                DryWeight = 0,
                ArtificialLift = ArtificialLift.NoArtificialLift,
                Maturity = Maturity.A,
                FlaredGas = 0,
                ProducerCount = 0,
                GasInjectorCount = 0,
                WaterInjectorCount = 0,
                CostYear = 0,
                ProspVersion = null,
                Source = Source.ConceptApp,
                ApprovedBy = "",
                FacilityOpex = 0,
                PeakElectricityImported = 0
            }
        };

        // Act
        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem);
        var flaring = EmissionCalculationHelper.CalculateFlaring(caseItem);
        var losses = EmissionCalculationHelper.CalculateLosses(caseItem);

        var total = TimeSeriesMerger.MergeTimeSeries(fuelConsumptions, flaring, losses);

        // Assert
        var expectedTotalFuelConsumptions = new List<double> { 6102721.5 };

        for (var i = 0; i < expectedTotalFuelConsumptions.Count; i++)
        {
            Assert.Equal(expectedTotalFuelConsumptions[i], total.Values[i], precision: 1);
        }
    }
}
