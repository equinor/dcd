using api.Models;

namespace api.Helpers;

public static class EmissionCalculationHelper
{
    private const int cd = 365;
    private const int ConversionFactorFromMtoG = 1000;

    public static TimeSeries<double> CalculateTotalFuelConsumptions(
        Case caseItem,
        Topside topside,
        DrainageStrategy drainageStrategy
    )
    {
        var factor = caseItem.FacilitiesAvailability * topside.FuelConsumption * cd * 1e6;

        var totalUseOfPower = CalculateTotalUseOfPower(topside, drainageStrategy, caseItem.FacilitiesAvailability);

        var fuelConsumptionValues = totalUseOfPower.Values.Select(v => v * factor).ToArray();
        var fuelConsumptions = new TimeSeries<double>
        {
            Values = fuelConsumptionValues,
            StartYear = totalUseOfPower.StartYear,
        };

        return fuelConsumptions;
    }

    public static TimeSeries<double> CalculateTotalUseOfPower(
        Topside topside,
        DrainageStrategy drainageStrategy,
        double pe
    )
    {
        var cO2ShareCO2MaxOil = topside.CO2ShareOilProfile * topside.CO2OnMaxOilProfile;
        var cO2ShareCO2MaxGas = topside.CO2ShareGasProfile * topside.CO2OnMaxGasProfile;
        var cO2ShareCO2MaxWI = topside.CO2ShareWaterInjectionProfile * topside.CO2OnMaxWaterInjectionProfile;

        var cO2ShareCO2Max = cO2ShareCO2MaxOil + cO2ShareCO2MaxGas + cO2ShareCO2MaxWI;

        var totalPowerOil = CalculateTotalUseOfPowerOil(topside, drainageStrategy, pe);
        var totalPowerGas = CalculateTotalUseOfPowerGas(topside, drainageStrategy, pe);
        var totalPowerWI = CalculateTotalUseOfPowerWI(topside, drainageStrategy, pe);

        var mergedPowerProfile = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>?> { totalPowerOil, totalPowerGas, totalPowerWI });

        var totalUseOFPowerValues = mergedPowerProfile.Values.Select(v => v + cO2ShareCO2Max).ToArray();
        var totalUseOfPower = new TimeSeries<double>
        {
            StartYear = mergedPowerProfile.StartYear,
            Values = totalUseOFPowerValues,
        };

        return totalUseOfPower;
    }

    // Formula: 1. WRP = WR/WIC/cd
    //          2. WRP*WSP*(1-WOM)
    private static TimeSeries<double> CalculateTotalUseOfPowerWI(
        Topside topside,
        DrainageStrategy drainageStrategy,
        double pe
    )
    {
        var wic = topside.WaterInjectionCapacity;
        var wr = drainageStrategy.ProductionProfileWaterInjection?.Values;

        var wsp = topside.CO2ShareWaterInjectionProfile;
        var wom = topside.CO2OnMaxWaterInjectionProfile;

        if (wr == null || wr.Length == 0 || wic == 0 || pe == 0)
        {
            return new TimeSeries<double>();
        }

        var wrp = wr.Select(v => v / wic / cd / pe);
        var wrp_wsp_wom = wrp.Select(v => v * wsp * (1 - wom));

        var totalUseOfPower = new TimeSeries<double>
        {
            Values = wrp_wsp_wom.ToArray(),
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
        };
        return totalUseOfPower;
    }

    // Formula: 1. GRP = GR/GC/cd/1000000
    //          2. GRP*GSP*(1-GOM)
    private static TimeSeries<double> CalculateTotalUseOfPowerGas(Topside topside, DrainageStrategy drainageStrategy, double pe)
    {
        var gc = topside.GasCapacity;
        var gr = drainageStrategy.ProductionProfileGas?.Values ?? Array.Empty<double>();
        var additionalGr = drainageStrategy.AdditionalProductionProfileGas?.Values ?? Array.Empty<double>();

        // Create TimeSeries<double> instances for both profiles
        var productionProfileGas = new TimeSeries<double>
        {
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
            Values = gr
        };

        var additionalProductionProfileGas = new TimeSeries<double>
        {
            StartYear = drainageStrategy.AdditionalProductionProfileGas?.StartYear ?? 0,
            Values = additionalGr
        };

        var mergedProfile = TimeSeriesCost.MergeCostProfiles(productionProfileGas, additionalProductionProfileGas);

        var gsp = topside.CO2ShareGasProfile;
        var gom = topside.CO2OnMaxGasProfile;

        if (mergedProfile.Values == null || mergedProfile.Values.Length == 0 || gc == 0 || pe == 0)
        {
            return new TimeSeries<double>();
        }

        // Convert merged values to appropriate units
        var grp = mergedProfile.Values.Select(v => v / gc / cd / pe / 1e6);

        // Apply CO2 Share and CO2 On Max multipliers
        var grp_gsp_gom = grp.Select(v => v * gsp * (1 - gom));

        // Create the final TimeSeries<double> result
        var totalUseOfPower = new TimeSeries<double>
        {
            Values = grp_gsp_gom.ToArray(),
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
        };

        return totalUseOfPower;
    }

    // Formula: 1. WRP = WR/WIC/cd
    //          2. ORP*OSP*(1-OOM)
    private static TimeSeries<double> CalculateTotalUseOfPowerOil(Topside topside, DrainageStrategy drainageStrategy, double pe)
    {
        var oc = topside.OilCapacity;
        var or = drainageStrategy.ProductionProfileOil?.Values ?? Array.Empty<double>();
        var additionalOr = drainageStrategy.AdditionalProductionProfileOil?.Values ?? Array.Empty<double>();

        // Create TimeSeries<double> instances for both profiles
        var productionProfileOil = new TimeSeries<double>
        {
            StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
            Values = or
        };

        var additionalProductionProfileOil = new TimeSeries<double>
        {
            StartYear = drainageStrategy.AdditionalProductionProfileOil?.StartYear ?? 0,
            Values = additionalOr
        };

        var mergedProfile = TimeSeriesCost.MergeCostProfiles(productionProfileOil, additionalProductionProfileOil);

        var osp = topside.CO2ShareOilProfile;
        var oom = topside.CO2OnMaxOilProfile;

        if (mergedProfile.Values.Length == 0 || oc == 0 || pe == 0)
        {
            return new TimeSeries<double>();
        }

        var orp = mergedProfile.Values.Select(v => v / oc / cd / pe);
        var orp_osp_oom = orp.Select(v => v * osp * (1 - oom));

        var totalUseOfPower = new TimeSeries<double>
        {
            Values = orp_osp_oom.ToArray(),
            StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
        };

        return totalUseOfPower;
    }

    public static TimeSeries<double> CalculateFlaring(Project project, DrainageStrategy drainageStrategy)
    {
        var oilRate = drainageStrategy.ProductionProfileOil?.Values.Select(v => v).ToArray() ?? Array.Empty<double>();
        var additionalOilRate = drainageStrategy.AdditionalProductionProfileOil?.Values.Select(v => v).ToArray() ?? Array.Empty<double>();

        var gasRate = drainageStrategy.ProductionProfileGas?.Values.Select(v => v / ConversionFactorFromMtoG).ToArray() ?? Array.Empty<double>();
        var additionalGasRate = drainageStrategy.AdditionalProductionProfileGas?.Values.Select(v => v / ConversionFactorFromMtoG).ToArray() ?? Array.Empty<double>();

        // Create TimeSeries<double> instances for both oil and gas profiles
        var oilRateTS = new TimeSeries<double>
        {
            Values = oilRate,
            StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
        };

        var additionalOilRateTS = new TimeSeries<double>
        {
            Values = additionalOilRate,
            StartYear = drainageStrategy.AdditionalProductionProfileOil?.StartYear ?? 0,
        };

        var gasRateTS = new TimeSeries<double>
        {
            Values = gasRate,
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
        };

        var additionalGasRateTS = new TimeSeries<double>
        {
            Values = additionalGasRate,
            StartYear = drainageStrategy.AdditionalProductionProfileGas?.StartYear ?? 0,
        };

        var mergedOilProfile = TimeSeriesCost.MergeCostProfiles(oilRateTS, additionalOilRateTS);
        var mergedGasProfile = TimeSeriesCost.MergeCostProfiles(gasRateTS, additionalGasRateTS);
        var mergedOilAndGas = TimeSeriesCost.MergeCostProfiles(mergedOilProfile, mergedGasProfile);

        var flaringValues = mergedOilAndGas.Values.Select(v => v * project.FlaredGasPerProducedVolume).ToArray() ?? Array.Empty<double>();
        var flaring = new TimeSeries<double>
        {
            Values = flaringValues,
            StartYear = mergedOilAndGas.StartYear,
        };

        return flaring;
    }

    public static TimeSeries<double> CalculateLosses(Project project, DrainageStrategy drainageStrategy)
    {
        var lossesValues = drainageStrategy.ProductionProfileGas?.Values.Select(v => v * project.CO2RemovedFromGas).ToArray() ?? Array.Empty<double>();
        var additionalGasLossesValues = drainageStrategy.AdditionalProductionProfileGas?.Values.Select(v => v * project.CO2RemovedFromGas).ToArray() ?? Array.Empty<double>();

        // Create TimeSeries<double> instances for both gas losses profiles
        var gasLossesTS = new TimeSeries<double>
        {
            Values = lossesValues,
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
        };

        var additionalGasLossesTS = new TimeSeries<double>
        {
            Values = additionalGasLossesValues,
            StartYear = drainageStrategy.AdditionalProductionProfileGas?.StartYear ?? 0,
        };

        var mergedGasLosses = TimeSeriesCost.MergeCostProfiles(gasLossesTS, additionalGasLossesTS);

        var losses = new TimeSeries<double>
        {
            Values = mergedGasLosses.Values,
            StartYear = mergedGasLosses.StartYear,
        };
        return losses;
    }
}
