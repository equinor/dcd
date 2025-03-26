using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.Helpers;

public static class EmissionCalculationHelper
{
    private const double Cd = 365.25;
    private const int ConversionFactorFromMtoG = 1000;

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

        var totalPowerOil = CalculateTotalUseOfPowerOil(caseItem, topside, facilitiesAvailability);
        var totalPowerGas = CalculateTotalUseOfPowerGas(caseItem, topside, facilitiesAvailability);
        var totalPowerWi = CalculateTotalUseOfPowerWi(caseItem, topside, facilitiesAvailability);

        return TimeSeriesMerger.MergeTimeSeries(totalPowerOil, totalPowerGas, totalPowerWi);
    }

    private static TimeSeries CalculateTotalUseOfPowerWi(Case caseItem, Topside topside, double facilitiesAvailability)
    {
        var waterInjectionCapacity = topside.WaterInjectionCapacity;
        var co2ShareWaterInjectionProfile = topside.Co2ShareWaterInjectionProfile;
        var co2OnMaxWaterInjectionProfile = topside.Co2OnMaxWaterInjectionProfile;
        var grossProductionProfileWaterInjection = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWaterInjection));

        return CalculateUseOfPower(grossProductionProfileWaterInjection, co2ShareWaterInjectionProfile, co2OnMaxWaterInjectionProfile, waterInjectionCapacity, facilitiesAvailability);
    }

    private static TimeSeries CalculateTotalUseOfPowerGas(Case caseItem, Topside topside, double facilitiesAvailability)
    {
        var gasCapacity = topside.GasCapacity;
        var co2ShareGasProfile = topside.Co2ShareGasProfile;
        var co2OnMaxGasProfile = topside.Co2OnMaxGasProfile;

        var productionProfileGas = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas));
        var additionalProductionProfileGas = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas));
        var grossProductionProfileGas = TimeSeriesMerger.MergeTimeSeries(productionProfileGas, additionalProductionProfileGas);

        var unitAdjustedGrossProductionProfileGas = new TimeSeries
        {
            Values = grossProductionProfileGas.Values.Select(v => v / 1_000_000).ToArray(),
            StartYear = grossProductionProfileGas.StartYear
        };

        return CalculateUseOfPower(unitAdjustedGrossProductionProfileGas, co2ShareGasProfile, co2OnMaxGasProfile, gasCapacity, facilitiesAvailability);
    }

    private static TimeSeries CalculateTotalUseOfPowerOil(Case caseItem, Topside topside, double facilitiesAvailability)
    {
        var oilCapacity = topside.OilCapacity;
        var co2ShareOilProfile = topside.Co2ShareOilProfile;
        var co2OnMaxOilProfile = topside.Co2OnMaxOilProfile;

        var productionProfileOil = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil));
        var additionalProductionProfileOil = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil));
        var grossProductionProfileOil = TimeSeriesMerger.MergeTimeSeries(productionProfileOil, additionalProductionProfileOil);

        return CalculateUseOfPower(grossProductionProfileOil, co2ShareOilProfile, co2OnMaxOilProfile, oilCapacity, facilitiesAvailability);
    }

    public static TimeSeries CalculateUseOfPower(TimeSeries grossProductionProfile, double co2ShareProfile, double co2OnMaxProfile, double capacity, double facilitiesAvailability)
    {
        if (grossProductionProfile.Values.Length == 0 || capacity == 0 || facilitiesAvailability == 0)
        {
            return new TimeSeries();
        }

        var facilitiesAvailabilityDecimal = facilitiesAvailability / 100;
        var shareTimesMax = co2ShareProfile * co2OnMaxProfile;
        var rateProductionOfDesign = grossProductionProfile.Values.Select(v => v / (Cd * facilitiesAvailabilityDecimal) / capacity);

        var rateShareOfPowerOnMax = rateProductionOfDesign
            .Select(v => v == 0
                        ? 0
                        : shareTimesMax + v * co2ShareProfile * (1 - co2OnMaxProfile))
            .ToArray();

        return new TimeSeries
        {
            Values = rateShareOfPowerOnMax,
            StartYear = grossProductionProfile.StartYear
        };
    }

    public static TimeSeries CalculateFlaring(Case caseItem)
    {
        var oilRateTs = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil));
        var additionalOilRateTs = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil));

        var gasRate = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values.Select(v => v / ConversionFactorFromMtoG).ToArray() ?? [];

        var gasRateTs = new TimeSeries
        {
            Values = gasRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0
        };

        var additionalGasRate = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values.Select(v => v / ConversionFactorFromMtoG).ToArray() ?? [];

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
}
