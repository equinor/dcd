using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.Helpers;

public static class EmissionCalculationHelper
{
    private const double Cd = 365.25;
    private const int GasToLiquidConversionFactor = 1000;

    public static TimeSeries CalculateTotalFuelConsumptions(Case caseItem)
    {
        var factor = caseItem.Topside.FuelConsumption * Cd * caseItem.FacilitiesAvailability / 100 * 1_000_000;
        var totalUseOfPower = CalculateTotalUseOfPower(caseItem, caseItem.FacilitiesAvailability);
        var fuelConsumptionValues = totalUseOfPower.Values.Select(v => v * factor).ToArray();

        return new TimeSeries
        {
            Values = fuelConsumptionValues,
            StartYear = totalUseOfPower.StartYear
        };
    }

    public static TimeSeries CalculateTotalUseOfPower(Case caseItem, double facilitiesAvailability)
    {
        var topside = caseItem.Topside;

        var productionOfDesignOil = CalculateProductionOfDesignOil(caseItem, topside, facilitiesAvailability);
        var productionOfDesignGas = CalculateProductionOfDesignGas(caseItem, topside, facilitiesAvailability);
        var productionOfDesignWi = CalculateProductionOfDesignWi(caseItem, topside, facilitiesAvailability);

        var totalProductionOfDesign = TimeSeriesMerger.MergeTimeSeries(productionOfDesignOil, productionOfDesignGas, productionOfDesignWi);

        var totalPowerOil = CalculateTotalUseOfPowerOil(productionOfDesignOil, totalProductionOfDesign, topside);
        var totalPowerGas = CalculateTotalUseOfPowerGas(productionOfDesignGas, totalProductionOfDesign, topside);
        var totalPowerWi = CalculateTotalUseOfPowerWi(productionOfDesignWi, totalProductionOfDesign, topside);

        return TimeSeriesMerger.MergeTimeSeries(totalPowerOil, totalPowerGas, totalPowerWi);
    }

    private static TimeSeries CalculateTotalUseOfPowerWi(TimeSeries productionOfDesignWi, TimeSeries totalProductionOfDesign, Topside topside)
    {
        var co2ShareWaterInjectionProfile = topside.Co2ShareWaterInjectionProfile;
        var co2OnMaxWaterInjectionProfile = topside.Co2OnMaxWaterInjectionProfile;

        return CalculateUseOfPower(productionOfDesignWi, totalProductionOfDesign, co2ShareWaterInjectionProfile, co2OnMaxWaterInjectionProfile);
    }

    private static TimeSeries CalculateTotalUseOfPowerGas(TimeSeries productionOfDesignGas, TimeSeries totalProductionOfDesign, Topside topside)
    {
        var co2ShareGasProfile = topside.Co2ShareGasProfile;
        var co2OnMaxGasProfile = topside.Co2OnMaxGasProfile;

        return CalculateUseOfPower(productionOfDesignGas, totalProductionOfDesign, co2ShareGasProfile, co2OnMaxGasProfile);
    }

    private static TimeSeries CalculateTotalUseOfPowerOil(TimeSeries productionOfDesignOil, TimeSeries totalProductionOfDesign, Topside topside)
    {
        var co2ShareOilProfile = topside.Co2ShareOilProfile;
        var co2OnMaxOilProfile = topside.Co2OnMaxOilProfile;

        return CalculateUseOfPower(productionOfDesignOil, totalProductionOfDesign, co2ShareOilProfile, co2OnMaxOilProfile);
    }

    public static TimeSeries CalculateUseOfPower(TimeSeries productionOfDesign, TimeSeries totalProductionOfDesign, double co2ShareProfile, double co2OnMaxProfile)
    {
        if (totalProductionOfDesign.Values.Length == 0)
        {
            return new TimeSeries();
        }

        var paddedProductionOfDesign = TimeSeriesPadding.PadTimeSeries(productionOfDesign, totalProductionOfDesign);

        var shareOfPowerOnMax = paddedProductionOfDesign.Values
            .Select((v, i) =>
                        totalProductionOfDesign.Values[i] == 0
                            ? 0
                            : co2ShareProfile * co2OnMaxProfile + v * co2ShareProfile * (1 - co2OnMaxProfile))
            .ToArray();

        return new TimeSeries
        {
            Values = shareOfPowerOnMax,
            StartYear = paddedProductionOfDesign.StartYear
        };
    }

    private static TimeSeries CalculateProductionOfDesignOil(Case caseItem, Topside topside, double facilitiesAvailability)
    {
        var oilCapacity = topside.OilCapacity;
        var productionProfileOil = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil));
        var additionalProductionProfileOil = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil));
        var grossProductionProfileOil = TimeSeriesMerger.MergeTimeSeries(productionProfileOil, additionalProductionProfileOil);

        return CalculateProductionOfDesign(grossProductionProfileOil, oilCapacity, facilitiesAvailability);
    }

    private static TimeSeries CalculateProductionOfDesignGas(Case caseItem, Topside topside, double facilitiesAvailability)
    {
        var gasCapacity = topside.GasCapacity;

        var productionProfileGas = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas));
        var additionalProductionProfileGas = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas));
        var grossProductionProfileGas = TimeSeriesMerger.MergeTimeSeries(productionProfileGas, additionalProductionProfileGas);

        var unitAdjustedGrossProductionProfileGas = new TimeSeries
        {
            Values = grossProductionProfileGas.Values.Select(v => v / 1_000_000).ToArray(),
            StartYear = grossProductionProfileGas.StartYear
        };

        return CalculateProductionOfDesign(unitAdjustedGrossProductionProfileGas, gasCapacity, facilitiesAvailability);
    }

    private static TimeSeries CalculateProductionOfDesignWi(Case caseItem, Topside topside, double facilitiesAvailability)
    {
        var waterInjectionCapacity = topside.WaterInjectionCapacity;
        var grossProductionProfileWaterInjection = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWaterInjection));

        return CalculateProductionOfDesign(grossProductionProfileWaterInjection, waterInjectionCapacity, facilitiesAvailability);
    }

    public static TimeSeries CalculateProductionOfDesign(TimeSeries grossProductionProfile, double capacity, double facilitiesAvailability)
    {
        if (grossProductionProfile.Values.Length == 0 || capacity == 0 || facilitiesAvailability == 0)
        {
            return new TimeSeries();
        }

        var facilitiesAvailabilityDecimal = facilitiesAvailability / 100;
        var productionOfDesign = grossProductionProfile.Values.Select(v => v / (Cd * facilitiesAvailabilityDecimal) / capacity).ToArray();

        return new TimeSeries
        {
            StartYear = grossProductionProfile.StartYear,
            Values = productionOfDesign
        };
    }

    public static TimeSeries CalculateFlaring(Case caseItem)
    {
        var oilRateTs = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil));
        var additionalOilRateTs = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil));

        var gasRate = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values.Select(v => v / GasToLiquidConversionFactor).ToArray() ?? [];

        var gasRateTs = new TimeSeries
        {
            Values = gasRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0
        };

        var additionalGasRate = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values.Select(v => v / GasToLiquidConversionFactor).ToArray() ?? [];

        var additionalGasRateTs = new TimeSeries
        {
            Values = additionalGasRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.StartYear ?? 0
        };

        var mergedOilAndGas = TimeSeriesMerger.MergeTimeSeries(oilRateTs, additionalOilRateTs, gasRateTs, additionalGasRateTs);

        return new TimeSeries
        {
            Values = mergedOilAndGas.Values.Select(v => v * caseItem.FlaredGasPerProducedVolume).ToArray(),
            StartYear = mergedOilAndGas.StartYear
        };
    }

    public static TimeSeries CalculateLosses(Case caseItem)
    {
        var lossesValues = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values.Select(v => v * caseItem.Co2RemovedFromGas).ToArray() ?? [];
        var additionalGasLossesValues = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values.Select(v => v * caseItem.Co2RemovedFromGas).ToArray() ?? [];

        var gasLossesTs = new TimeSeries
        {
            Values = lossesValues,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0
        };

        var additionalGasLossesTs = new TimeSeries
        {
            Values = additionalGasLossesValues,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.StartYear ?? 0
        };

        return TimeSeriesMerger.MergeTimeSeries(gasLossesTs, additionalGasLossesTs);
    }

    private static TimeSeries AdjustWhenNoGasProduction(TimeSeries total, Case caseItem)
    {
        var totalGasProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileGas);
        var paddedTotalGasProduction = TimeSeriesPadding.PadTimeSeries(totalGasProduction, total);
        var totalValuesWhenNoGasProduction = total.Values
            .Select((value, i) =>
                        paddedTotalGasProduction.Values[i] == 0
                            ? 0
                            : value)
            .ToArray();

        return new TimeSeries
        {
            Values = totalValuesWhenNoGasProduction,
            StartYear = total.StartYear
        };
    }

    public static TimeSeries CalculateFuelFlaringAndLosses(Case caseItem)
    {
        var fuelConsumptions = CalculateTotalFuelConsumptions(caseItem);
        var flaring = CalculateFlaring(caseItem);
        var losses = CalculateLosses(caseItem);

        var totalFuelFlaringLosses = TimeSeriesMerger.MergeTimeSeries(fuelConsumptions, flaring, losses);
        var adjustedTotalFuelFlaringLosses = AdjustWhenNoGasProduction(totalFuelFlaringLosses, caseItem);

        return adjustedTotalFuelFlaringLosses;
    }
}
