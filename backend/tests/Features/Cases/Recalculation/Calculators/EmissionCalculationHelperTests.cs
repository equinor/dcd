using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Features.Recalculation.Helpers;
using api.Models;
using api.Models.Enums;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class EmissionCalculationHelperTests
{
    #region Values from bccst file

    private const double FacilitiesAvailability = 90;

    private const double OilCapacity = 1000;
    private const double GasCapacity = 0.2;
    private const double WaterInjectionCapacity = 1000;

    private const double Co2ShareGasProfile = 0.41428;
    private const double Co2OnMaxGasProfile = 3.22195;

    private const double Co2ShareWiProfile = 0.41428;
    private const double Co2OnMaxWiProfile = 3.22195;

    private const double Co2ShareOilProfile = 0.5857174;
    private const double Co2OnMaxOilProfile = 0.0009110459;

    private static readonly double[] GrossOilProductionValues =
    [
        0.36702713296,
        0.36530111389,
        0.36417066856,

        0.36034963702,
        0.35710741704,
        0.35301865030,

        0.34889952688,
        0.34103307272,
        0.33066222992
    ];

    private static readonly double[] GrossGasProductionValues =
    [
        0.07159535222,
        0.07129710053,
        0.07109665008,

        0.07033006336,
        0.06951503301,
        0.0684243910,

        0.06722300344,
        0.06518925497,
        0.06281304473
    ];

    private static readonly double[] GrossWiProductionValues =
    [
        0.07159535222,
        0.07129710053,
        0.07109665008,

        0.07033006336,
        0.06951503301,
        0.0684243910,

        0.06722300344,
        0.06518925497,
        0.06281304473
    ];

    private static readonly double[] TotalUseOfOilPowerExpectedValues =
    [
        0.653901481,
        0.650828887,
        0.648816510,
        0.642014454,
        0.636242776,
        0.628964108,
        0.621631400,
        0.607627835,
        0.589166051
    ];

    private static readonly double[] TotalUseOfGasPowerExpectedValues =
    [
        0.3323700,
        0.3365460,
        0.3393525,
        0.3500857,
        0.3614972,
        0.3767676,
        0.3935886,
        0.4220637,
        0.4553337
    ];

    private static readonly double[] TotalUseOfWiPowerExpectedValues =
    [
        1.13431190,
        1.13514708,
        1.13570839,
        1.13785504,
        1.14013733,
        1.14319141,
        1.14655560,
        1.15225063,
        1.15890463
    ];

    private static readonly double[] TotalUseOfWiPowerExpectedValuesWhenNoWiProduction =
    [
        1.33479736,
        1.33479736,
        1.33479736,
        1.33479736,
        1.33479736,
        1.33479736,
        1.33479736,
        1.33479736,
        1.33479736
    ];

    #endregion

    #region Convert bccst to profiles with correct unit values

    private static readonly TimeSeries GrossProductionOilProfile = new()
    {
        StartYear = 0,
        Values = GrossOilProductionValues.Select(x => x * 1_000_000).ToArray()
    };

    private static readonly TimeSeries GrossProductionWiProfile = new()
    {
        StartYear = 0,
        Values = GrossWiProductionValues.Select(x => x * 1_000_000).ToArray()
    };

    private static readonly TimeSeries GrossProductionGasProfile = new()
    {
        StartYear = 0,
        Values = GrossGasProductionValues.Select(x => x * 1_000).ToArray()
    };

    #endregion

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
    public void CalculateFuelFlaringAndLosses_WithDataInput_ReturnsCorrectValue()
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
    public void CalculateUseOfPower_gas_calculation_where_numbers_are_from_bccst_file()
    {
        var productionOfDesignGas = EmissionCalculationHelper.CalculateProductionOfDesign(GrossProductionGasProfile, GasCapacity, FacilitiesAvailability);

        var totalUseOfGasPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignGas, productionOfDesignGas, Co2ShareGasProfile, Co2OnMaxGasProfile);

        CompareResultAndExpectedValues(TotalUseOfGasPowerExpectedValues, totalUseOfGasPower);
    }

    [Fact]
    public void CalculateUseOfPower_water_injection_calculation_where_numbers_are_from_bccst_file()
    {
        var productionOfDesignWi = EmissionCalculationHelper.CalculateProductionOfDesign(GrossProductionWiProfile, WaterInjectionCapacity, FacilitiesAvailability);

        var totalUseOfWiPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignWi, productionOfDesignWi, Co2ShareWiProfile, Co2OnMaxWiProfile);

        CompareResultAndExpectedValues(TotalUseOfWiPowerExpectedValues, totalUseOfWiPower);
    }

    [Fact]
    public void CalculateUseOfPower_oil_calculation_where_numbers_are_from_bccst_file()
    {
        var productionOfDesignOil = EmissionCalculationHelper.CalculateProductionOfDesign(GrossProductionOilProfile, OilCapacity, FacilitiesAvailability);

        var totalUseOfOilPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignOil, productionOfDesignOil, Co2ShareOilProfile, Co2OnMaxOilProfile);

        CompareResultAndExpectedValues(TotalUseOfOilPowerExpectedValues, totalUseOfOilPower);
    }

    [Fact]
    public void CalculateTotalUseOfPower_where_numbers_are_from_bccst_file()
    {
        var productionOfDesignOil = EmissionCalculationHelper.CalculateProductionOfDesign(GrossProductionOilProfile, OilCapacity, FacilitiesAvailability);
        var productionOfDesignGas = EmissionCalculationHelper.CalculateProductionOfDesign(GrossProductionGasProfile, GasCapacity, FacilitiesAvailability);
        var productionOfDesignWi = EmissionCalculationHelper.CalculateProductionOfDesign(GrossProductionWiProfile, WaterInjectionCapacity, FacilitiesAvailability);

        var totalProductionOfDesign = TimeSeriesMerger.MergeTimeSeries(productionOfDesignOil, productionOfDesignGas, productionOfDesignWi);

        var totalUseOfOilPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignOil, totalProductionOfDesign, Co2ShareOilProfile, Co2OnMaxOilProfile);

        CompareResultAndExpectedValues(TotalUseOfOilPowerExpectedValues, totalUseOfOilPower);

        var totalUseOfWiPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignWi, totalProductionOfDesign, Co2ShareWiProfile, Co2OnMaxWiProfile);

        CompareResultAndExpectedValues(TotalUseOfWiPowerExpectedValues, totalUseOfWiPower);

        var totalUseOfGasPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignGas, totalProductionOfDesign, Co2ShareGasProfile, Co2OnMaxGasProfile);

        CompareResultAndExpectedValues(TotalUseOfGasPowerExpectedValues, totalUseOfGasPower);
    }

    [Fact]
    public void CalculateTotalUseOfPower_with_no_water_injection_production_where_numbers_are_from_bccst_file()
    {
        var emptyProductionWiProfile = new TimeSeries();

        var productionOfDesignOil = EmissionCalculationHelper.CalculateProductionOfDesign(GrossProductionOilProfile, OilCapacity, FacilitiesAvailability);
        var productionOfDesignGas = EmissionCalculationHelper.CalculateProductionOfDesign(GrossProductionGasProfile, GasCapacity, FacilitiesAvailability);
        var productionOfDesignWi = EmissionCalculationHelper.CalculateProductionOfDesign(emptyProductionWiProfile, WaterInjectionCapacity, FacilitiesAvailability);

        Assert.All(productionOfDesignWi.Values, x => Assert.Equal(0, x));

        var totalProductionOfDesign = TimeSeriesMerger.MergeTimeSeries(productionOfDesignOil, productionOfDesignGas, productionOfDesignWi);

        var totalUseOfOilPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignOil, totalProductionOfDesign, Co2ShareOilProfile, Co2OnMaxOilProfile);

        CompareResultAndExpectedValues(TotalUseOfOilPowerExpectedValues, totalUseOfOilPower);

        var totalUseOfWiPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignWi, totalProductionOfDesign, Co2ShareWiProfile, Co2OnMaxWiProfile);

        CompareResultAndExpectedValues(TotalUseOfWiPowerExpectedValuesWhenNoWiProduction, totalUseOfWiPower);

        var totalUseOfGasPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignGas, totalProductionOfDesign, Co2ShareGasProfile, Co2OnMaxGasProfile);

        CompareResultAndExpectedValues(TotalUseOfGasPowerExpectedValues, totalUseOfGasPower);
    }

    [Fact]
    public void CalculateTotalUseOfPower_with_stop_in_production_where_numbers_are_from_bccst_file()
    {
        var modifiedGrossProductionOilProfile = new TimeSeries
        {
            StartYear = GrossProductionOilProfile.StartYear,
            Values = GrossProductionOilProfile.Values.Select(v => v).ToArray()
        };

        var modifiedTotalUseOfOilPowerExpectedValues = TotalUseOfOilPowerExpectedValues.Select(v => v).ToArray();
        modifiedGrossProductionOilProfile.Values[4] = 0;
        modifiedGrossProductionOilProfile.Values[5] = 0;
        modifiedTotalUseOfOilPowerExpectedValues[4] = 0;
        modifiedTotalUseOfOilPowerExpectedValues[5] = 0;

        var modifiedGrossProductionGasProfile = new TimeSeries
        {
            StartYear = GrossProductionGasProfile.StartYear,
            Values = GrossProductionGasProfile.Values.Select(v => v).ToArray()
        };

        var modifiedTotalUseOfGasPowerExpectedValues = TotalUseOfGasPowerExpectedValues.Select(v => v).ToArray();
        modifiedGrossProductionGasProfile.Values[4] = 0;
        modifiedGrossProductionGasProfile.Values[5] = 0;
        modifiedTotalUseOfGasPowerExpectedValues[4] = 0;
        modifiedTotalUseOfGasPowerExpectedValues[5] = 0;

        var modifiedGrossProductionWiProfile = new TimeSeries
        {
            StartYear = GrossProductionWiProfile.StartYear,
            Values = GrossProductionWiProfile.Values.Select(v => v).ToArray()
        };

        var modifiedTotalUseOfWiPowerExpectedValues = TotalUseOfWiPowerExpectedValues.Select(v => v).ToArray();
        modifiedGrossProductionWiProfile.Values[4] = 0;
        modifiedGrossProductionWiProfile.Values[5] = 0;
        modifiedTotalUseOfWiPowerExpectedValues[4] = 0;
        modifiedTotalUseOfWiPowerExpectedValues[5] = 0;

        var productionOfDesignOil = EmissionCalculationHelper.CalculateProductionOfDesign(modifiedGrossProductionOilProfile, OilCapacity, FacilitiesAvailability);
        var productionOfDesignGas = EmissionCalculationHelper.CalculateProductionOfDesign(modifiedGrossProductionGasProfile, GasCapacity, FacilitiesAvailability);
        var productionOfDesignWi = EmissionCalculationHelper.CalculateProductionOfDesign(modifiedGrossProductionWiProfile, WaterInjectionCapacity, FacilitiesAvailability);

        var totalProductionOfDesign = TimeSeriesMerger.MergeTimeSeries(productionOfDesignOil, productionOfDesignGas, productionOfDesignWi);

        var totalUseOfOilPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignOil, totalProductionOfDesign, Co2ShareOilProfile, Co2OnMaxOilProfile);

        CompareResultAndExpectedValues(modifiedTotalUseOfOilPowerExpectedValues, totalUseOfOilPower);

        var totalUseOfWiPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignWi, totalProductionOfDesign, Co2ShareWiProfile, Co2OnMaxWiProfile);

        CompareResultAndExpectedValues(modifiedTotalUseOfWiPowerExpectedValues, totalUseOfWiPower);

        var totalUseOfGasPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignGas, totalProductionOfDesign, Co2ShareGasProfile, Co2OnMaxGasProfile);

        CompareResultAndExpectedValues(modifiedTotalUseOfGasPowerExpectedValues, totalUseOfGasPower);
    }

    [Fact]
    public void CalculateTotalUseOfPower_with_different_years_of_production_where_numbers_are_from_bccst_file()
    {
        var modifiedGrossProductionOilProfile = new TimeSeries
        {
            StartYear = GrossProductionOilProfile.StartYear,
            Values = GrossProductionOilProfile.Values.Take(3).ToArray()
        };

        var modifiedTotalUseOfOilPowerExpectedValues = TotalUseOfOilPowerExpectedValues
            .Select((v, i) => i < 3 ? v : Co2ShareOilProfile * Co2OnMaxOilProfile).ToArray();

        var modifiedGrossProductionGasProfile = new TimeSeries
        {
            StartYear = GrossProductionGasProfile.StartYear + 6,
            Values = GrossProductionGasProfile.Values.TakeLast(3).ToArray()
        };

        var modifiedTotalUseOfGasPowerExpectedValues = TotalUseOfGasPowerExpectedValues
            .Select((v, i) => i >= 6 ? v : Co2ShareGasProfile * Co2OnMaxGasProfile).ToArray();

        var modifiedGrossProductionWiProfile = new TimeSeries
        {
            StartYear = GrossProductionWiProfile.StartYear + 3,
            Values = GrossProductionWiProfile.Values.Take(6).TakeLast(3).ToArray()
        };

        var modifiedTotalUseOfWiPowerExpectedValues = TotalUseOfWiPowerExpectedValues
            .Select((v, i) => i is < 6 and >= 3 ? v : Co2ShareWiProfile * Co2OnMaxWiProfile).ToArray();

        var productionOfDesignOil = EmissionCalculationHelper.CalculateProductionOfDesign(modifiedGrossProductionOilProfile, OilCapacity, FacilitiesAvailability);
        var productionOfDesignGas = EmissionCalculationHelper.CalculateProductionOfDesign(modifiedGrossProductionGasProfile, GasCapacity, FacilitiesAvailability);
        var productionOfDesignWi = EmissionCalculationHelper.CalculateProductionOfDesign(modifiedGrossProductionWiProfile, WaterInjectionCapacity, FacilitiesAvailability);

        var totalProductionOfDesign = TimeSeriesMerger.MergeTimeSeries(productionOfDesignOil, productionOfDesignGas, productionOfDesignWi);

        var totalUseOfOilPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignOil, totalProductionOfDesign, Co2ShareOilProfile, Co2OnMaxOilProfile);

        CompareResultAndExpectedValues(modifiedTotalUseOfOilPowerExpectedValues, totalUseOfOilPower);

        var totalUseOfWiPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignWi, totalProductionOfDesign, Co2ShareWiProfile, Co2OnMaxWiProfile);

        CompareResultAndExpectedValues(modifiedTotalUseOfWiPowerExpectedValues, totalUseOfWiPower);

        var totalUseOfGasPower = EmissionCalculationHelper.CalculateUseOfPower(productionOfDesignGas, totalProductionOfDesign, Co2ShareGasProfile, Co2OnMaxGasProfile);

        CompareResultAndExpectedValues(modifiedTotalUseOfGasPowerExpectedValues, totalUseOfGasPower);
    }

    private static void CompareResultAndExpectedValues(double[] expectedValues, TimeSeries result, int precision = 3)
    {
        for (var i = 0; i < expectedValues.Length; i++)
        {
            Assert.Equal(expectedValues[i], result.Values[i], precision);
        }
    }
}
