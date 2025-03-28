using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class EmissionCalculationHelperTests
{
    [Fact]
    public void CalculateFuelFlaringAndLosses_ReturnsCorrectValue()
    {
        // Arrange
        var caseItem = new Case
        {
            FlaredGasPerProducedVolume = 1.122765,
            Co2RemovedFromGas = 0.0,
            DailyEmissionFromDrillingRig = 117,

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
            FlaredGasPerProducedVolume = 1.122765,
            Co2RemovedFromGas = 0.0,
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

    [Fact]
    public void CalculateUseOfPower__gas_calculation__where_numbers_are_from_verified_bccst_file()
    {
        var values = new[]
        {
            0.07159535222,
            0.07129710053,
            0.07109665008,

            0.07033006336,
            0.06951503301,
            0.0684243910,

            0.06722300344,
            0.06518925497,
            0.06281304473
        };

        var grossProductionProfile = new TimeSeries
        {
            StartYear = 0,
            Values = values.Select(x => x * 1_000).ToArray()
        };

        var totalUseOfPower = EmissionCalculationHelper.CalculateUseOfPower(grossProductionProfile, 0.41428, 3.22195, 0.2, 90);

        Assert.Equal(0.3323700, totalUseOfPower.Values[0], 3);
        Assert.Equal(0.3365460, totalUseOfPower.Values[1], 3);
        Assert.Equal(0.3393525, totalUseOfPower.Values[2], 3);

        Assert.Equal(0.3500857, totalUseOfPower.Values[3], 3);
        Assert.Equal(0.3614972, totalUseOfPower.Values[4], 3);
        Assert.Equal(0.3767676, totalUseOfPower.Values[5], 3);

        Assert.Equal(0.3935886, totalUseOfPower.Values[6], 3);
        Assert.Equal(0.4220637, totalUseOfPower.Values[7], 3);
        Assert.Equal(0.4553337, totalUseOfPower.Values[8], 3);
    }

    [Fact]
    public void CalculateUseOfPower__water_injection_calculation__where_numbers_are_from_verified_bccst_file()
    {
        var values = new[]
        {
            0.07159535222,
            0.07129710053,
            0.07109665008,

            0.07033006336,
            0.06951503301,
            0.0684243910,

            0.06722300344,
            0.06518925497,
            0.06281304473
        };

        var grossProductionProfile = new TimeSeries
        {
            StartYear = 0,
            Values = values.Select(x => x * 1_000_000).ToArray()
        };

        var totalUseOfPower = EmissionCalculationHelper.CalculateUseOfPower(grossProductionProfile, 0.41428, 3.22195, 1000, 90);

        Assert.Equal(1.13431190, totalUseOfPower.Values[0], 3);
        Assert.Equal(1.13514708, totalUseOfPower.Values[1], 3);
        Assert.Equal(1.13570839, totalUseOfPower.Values[2], 3);

        Assert.Equal(1.13785504, totalUseOfPower.Values[3], 3);
        Assert.Equal(1.14013733, totalUseOfPower.Values[4], 3);
        Assert.Equal(1.14319141, totalUseOfPower.Values[5], 3);

        Assert.Equal(1.14655560, totalUseOfPower.Values[6], 3);
        Assert.Equal(1.15225063, totalUseOfPower.Values[7], 3);
        Assert.Equal(1.15890463, totalUseOfPower.Values[8], 3);
    }

    [Fact]
    public void CalculateUseOfPower__oil_calculation__where_numbers_are_from_verified_bccst_file()
    {
        var values = new[]
        {
            0.36702713296,
            0.36530111389,
            0.36417066856,

            0.36034963702,
            0.35710741704,
            0.35301865030,

            0.34889952688,
            0.34103307272,
            0.33066222992
        };

        var grossProductionProfile = new TimeSeries
        {
            StartYear = 0,
            Values = values.Select(x => x * 1_000_000).ToArray()
        };

        var totalUseOfPower = EmissionCalculationHelper.CalculateUseOfPower(grossProductionProfile, 0.5857174, 0.0009110459, 1000, 90);

        Assert.Equal(0.653901481, totalUseOfPower.Values[0], 3);
        Assert.Equal(0.650828887, totalUseOfPower.Values[1], 3);
        Assert.Equal(0.648816510, totalUseOfPower.Values[2], 3);

        Assert.Equal(0.642014454, totalUseOfPower.Values[3], 3);
        Assert.Equal(0.636242776, totalUseOfPower.Values[4], 3);
        Assert.Equal(0.628964108, totalUseOfPower.Values[5], 3);

        Assert.Equal(0.621631400, totalUseOfPower.Values[6], 3);
        Assert.Equal(0.607627835, totalUseOfPower.Values[7], 3);
        Assert.Equal(0.589166051, totalUseOfPower.Values[8], 3);
    }
}
