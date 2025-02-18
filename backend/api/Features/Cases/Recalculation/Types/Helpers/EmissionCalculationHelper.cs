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
        var factor = caseItem.Topside.FuelConsumption * Cd * caseItem.FacilitiesAvailability / 100 * 1000000;
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

        var co2ShareCo2MaxOil = topside.CO2ShareOilProfile * topside.CO2OnMaxOilProfile;
        var co2ShareCo2MaxGas = topside.CO2ShareGasProfile * topside.CO2OnMaxGasProfile;
        var co2ShareCo2MaxWi = topside.CO2ShareWaterInjectionProfile * topside.CO2OnMaxWaterInjectionProfile;

        var co2ShareCo2Max = co2ShareCo2MaxOil + co2ShareCo2MaxGas + co2ShareCo2MaxWi;

        var totalPowerOil = CalculateTotalUseOfPowerOil(caseItem, topside, facilitiesAvailability);
        var totalPowerGas = CalculateTotalUseOfPowerGas(caseItem, topside, facilitiesAvailability);
        var totalPowerWi = CalculateTotalUseOfPowerWi(caseItem, topside, facilitiesAvailability);

        var totalUseOfPowerValues = TimeSeriesMerger.MergeTimeSeries(totalPowerOil, totalPowerGas, totalPowerWi);

        return new TimeSeries
        {
            StartYear = totalUseOfPowerValues.StartYear,
            Values = totalUseOfPowerValues.Values
        };
    }

    private static TimeSeries CalculateTotalUseOfPowerWi(Case caseItem, Topside topside, double facilitiesAvailability)
    {
        var wic = topside.WaterInjectionCapacity;
        var wr = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWaterInjection)?.Values;

        var wsp = topside.CO2ShareWaterInjectionProfile;
        var wom = topside.CO2OnMaxWaterInjectionProfile;

        if (wr == null || wr.Length == 0 || wic == 0 || facilitiesAvailability == 0)
        {
            return new TimeSeries();
        }

        var step1 = wsp * wom;
        var wrp = wr.Select(v => v / (Cd * (facilitiesAvailability / 100)) / wic);
        var wrpWspWom = wrp.Select(v => step1 + (v * 100 * wsp * (1 - wom)));

        return new TimeSeries
        {
            Values = wrpWspWom.ToArray(),
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0
        };
    }

    private static TimeSeries CalculateTotalUseOfPowerGas(Case caseItem, Topside topside, double facilitiesAvailability)
    {
        var gc = topside.GasCapacity;
        var gr = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values ?? [];
        var additionalGr = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values ?? [];

        // Create TimeSeriesCost instances for both profiles
        var productionProfileGas = new TimeSeries
        {
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0,
            Values = gr
        };

        var additionalProductionProfileGas = new TimeSeries
        {
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.StartYear ?? 0,
            Values = additionalGr
        };

        var mergedProfile = TimeSeriesMerger.MergeTimeSeries(productionProfileGas, additionalProductionProfileGas);

        var gsp = topside.CO2ShareGasProfile;
        var gom = topside.CO2OnMaxGasProfile;

        if (mergedProfile.Values.Length == 0 || gc == 0 || facilitiesAvailability == 0)
        {
            return new TimeSeries();
        }

        var step1 = gsp * gom;
        var grp = mergedProfile.Values.Select(v => v / (Cd * (facilitiesAvailability / 100)) / gc / 1000000);
        var grpGspGom = grp.Select(v => step1 + (v * gsp * (1 - gom)));

        return new TimeSeries
        {
            Values = grpGspGom.ToArray(),
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0
        };
    }

    private static TimeSeries CalculateTotalUseOfPowerOil(Case caseItem, Topside topside, double facilitiesAvailability)
    {
        var oc = topside.OilCapacity;
        var or = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.Values ?? [];
        var additionalOr = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.Values ?? [];

        var productionProfileOil = new TimeSeries
        {
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.StartYear ?? 0,
            Values = or
        };

        var additionalProductionProfileOil = new TimeSeries
        {
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.StartYear ?? 0,
            Values = additionalOr
        };

        var mergedProfile = TimeSeriesMerger.MergeTimeSeries(productionProfileOil, additionalProductionProfileOil);

        var osp = topside.CO2ShareOilProfile;
        var oom = topside.CO2OnMaxOilProfile;

        if (mergedProfile.Values.Length == 0 || oc == 0 || facilitiesAvailability == 0)
        {
            return new TimeSeries
            {
                Values = new double[(int)(osp * oom)],
                StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.StartYear ?? 0
            };
        }

        var step1 = osp * oom;
        var orp = mergedProfile.Values.Select(v => v / (Cd * (facilitiesAvailability / 100)) / oc);
        var orpOspOom = orp.Select(v => step1 + (v * osp * (1 - oom)));

        return new TimeSeries
        {
            Values = orpOspOom.ToArray(),
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.StartYear ?? 0
        };
    }

    public static TimeSeries CalculateFlaring(Case caseItem)
    {
        var oilRate = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.Values.Select(v => v).ToArray() ?? [];
        var additionalOilRate = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.Values.Select(v => v).ToArray() ?? [];

        var gasRate = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values.Select(v => v / ConversionFactorFromMtoG).ToArray() ?? [];
        var additionalGasRate = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values.Select(v => v / ConversionFactorFromMtoG).ToArray() ?? [];

        // Create TimeSeriesCost instances for both oil and gas profiles
        var oilRateTs = new TimeSeries
        {
            Values = oilRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.StartYear ?? 0
        };

        var additionalOilRateTs = new TimeSeries
        {
            Values = additionalOilRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.StartYear ?? 0
        };

        var gasRateTs = new TimeSeries
        {
            Values = gasRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0
        };

        var additionalGasRateTs = new TimeSeries
        {
            Values = additionalGasRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.StartYear ?? 0
        };

        var mergedOilProfile = TimeSeriesMerger.MergeTimeSeries(oilRateTs, additionalOilRateTs);
        var mergedGasProfile = TimeSeriesMerger.MergeTimeSeries(gasRateTs, additionalGasRateTs);
        var mergedOilAndGas = TimeSeriesMerger.MergeTimeSeries(mergedOilProfile, mergedGasProfile);

        var flaringValues = mergedOilAndGas.Values.Select(v => v * caseItem.Project.FlaredGasPerProducedVolume).ToArray();

        return new TimeSeries
        {
            Values = flaringValues,
            StartYear = mergedOilAndGas.StartYear
        };
    }

    public static TimeSeries CalculateLosses(Case caseItem)
    {
        var lossesValues = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values.Select(v => v * caseItem.Project.CO2RemovedFromGas).ToArray() ?? [];
        var additionalGasLossesValues = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values.Select(v => v * caseItem.Project.CO2RemovedFromGas).ToArray() ?? [];

        // Create TimeSeriesCost instances for both gas losses profiles
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

        var mergedGasLosses = TimeSeriesMerger.MergeTimeSeries(gasLossesTs, additionalGasLossesTs);

        return new TimeSeries
        {
            Values = mergedGasLosses.Values,
            StartYear = mergedGasLosses.StartYear
        };
    }
}
