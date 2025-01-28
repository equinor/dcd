using api.Features.Profiles;
using api.Features.TimeSeriesCalculators;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.Helpers;

public static class EmissionCalculationHelper
{
    private const double Cd = 365.25;
    private const int ConversionFactorFromMtoG = 1000;

    public static TimeSeries<double> CalculateTotalFuelConsumptions(Case caseItem, Topside topside)
    {
        var factor = caseItem.FacilitiesAvailability * topside.FuelConsumption * Cd * 1e6;

        var totalUseOfPower = CalculateTotalUseOfPower(caseItem, topside, caseItem.FacilitiesAvailability);

        var fuelConsumptionValues = totalUseOfPower.Values.Select(v => v * factor).ToArray();
        var fuelConsumptions = new TimeSeries<double>
        {
            Values = fuelConsumptionValues,
            StartYear = totalUseOfPower.StartYear,
        };

        return fuelConsumptions;
    }

    public static TimeSeries<double> CalculateTotalUseOfPower(
        Case caseItem,
        Topside topside,
        double pe
    )
    {
        var co2ShareCo2MaxOil = topside.CO2ShareOilProfile * topside.CO2OnMaxOilProfile;
        var co2ShareCo2MaxGas = topside.CO2ShareGasProfile * topside.CO2OnMaxGasProfile;
        var co2ShareCo2MaxWi = topside.CO2ShareWaterInjectionProfile * topside.CO2OnMaxWaterInjectionProfile;

        var co2ShareCo2Max = co2ShareCo2MaxOil + co2ShareCo2MaxGas + co2ShareCo2MaxWi;

        var totalPowerOil = CalculateTotalUseOfPowerOil(caseItem, topside, pe);
        var totalPowerGas = CalculateTotalUseOfPowerGas(caseItem, topside, pe);
        var totalPowerWi = CalculateTotalUseOfPowerWi(caseItem, topside, pe);

        var mergedPowerProfile = CostProfileMerger.MergeCostProfiles(totalPowerOil, totalPowerGas, totalPowerWi);

        var totalUseOfPowerValues = mergedPowerProfile.Values.Select(v => v + co2ShareCo2Max).ToArray();
        var totalUseOfPower = new TimeSeries<double>
        {
            StartYear = mergedPowerProfile.StartYear,
            Values = totalUseOfPowerValues,
        };

        return totalUseOfPower;
    }

    // Formula: 1. WRP = WR/WIC/cd
    //          2. WRP*WSP*(1-WOM)
    private static TimeSeries<double> CalculateTotalUseOfPowerWi(
        Case caseItem,
        Topside topside,
        double pe
    )
    {
        var wic = topside.WaterInjectionCapacity;
        var wr = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWaterInjection)?.Values;

        var wsp = topside.CO2ShareWaterInjectionProfile;
        var wom = topside.CO2OnMaxWaterInjectionProfile;

        if (wr == null || wr.Length == 0 || wic == 0 || pe == 0)
        {
            return new TimeSeries<double>();
        }

        var wrp = wr.Select(v => v / wic / Cd / pe);
        var wrpWspWom = wrp.Select(v => v * wsp * (1 - wom));

        var totalUseOfPower = new TimeSeries<double>
        {
            Values = wrpWspWom.ToArray(),
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0,
        };
        return totalUseOfPower;
    }

    // Formula: 1. GRP = GR/GC/cd/1000000
    //          2. GRP*GSP*(1-GOM)
    private static TimeSeries<double> CalculateTotalUseOfPowerGas(Case caseItem, Topside topside, double pe)
    {
        var gc = topside.GasCapacity;
        var gr = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values ?? [];
        var additionalGr = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values ?? [];

        // Create TimeSeries<double> instances for both profiles
        var productionProfileGas = new TimeSeries<double>
        {
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0,
            Values = gr
        };

        var additionalProductionProfileGas = new TimeSeries<double>
        {
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.StartYear ?? 0,
            Values = additionalGr
        };

        var mergedProfile = CostProfileMerger.MergeCostProfiles(productionProfileGas, additionalProductionProfileGas);

        var gsp = topside.CO2ShareGasProfile;
        var gom = topside.CO2OnMaxGasProfile;

        if (mergedProfile.Values == null || mergedProfile.Values.Length == 0 || gc == 0 || pe == 0)
        {
            return new TimeSeries<double>();
        }

        // Convert merged values to appropriate units
        var grp = mergedProfile.Values.Select(v => v / gc / Cd / pe / 1e6);

        // Apply CO2 Share and CO2 On Max multipliers
        var grpGspGom = grp.Select(v => v * gsp * (1 - gom));

        // Create the final TimeSeries<double> result
        var totalUseOfPower = new TimeSeries<double>
        {
            Values = grpGspGom.ToArray(),
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0,
        };

        return totalUseOfPower;
    }

    // Formula: 1. WRP = WR/WIC/cd
    //          2. ORP*OSP*(1-OOM)
    private static TimeSeries<double> CalculateTotalUseOfPowerOil(Case caseItem, Topside topside, double pe)
    {
        var oc = topside.OilCapacity;
        var or = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.Values ?? [];
        var additionalOr = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.Values ?? [];

        // Create TimeSeries<double> instances for both profiles
        var productionProfileOil = new TimeSeries<double>
        {
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.StartYear ?? 0,
            Values = or
        };

        var additionalProductionProfileOil = new TimeSeries<double>
        {
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.StartYear ?? 0,
            Values = additionalOr
        };

        var mergedProfile = CostProfileMerger.MergeCostProfiles(productionProfileOil, additionalProductionProfileOil);

        var osp = topside.CO2ShareOilProfile;
        var oom = topside.CO2OnMaxOilProfile;

        if (mergedProfile.Values.Length == 0 || oc == 0 || pe == 0)
        {
            return new TimeSeries<double>();
        }

        var orp = mergedProfile.Values.Select(v => v / oc / Cd / pe);
        var orpOspOom = orp.Select(v => v * osp * (1 - oom));

        var totalUseOfPower = new TimeSeries<double>
        {
            Values = orpOspOom.ToArray(),
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.StartYear ?? 0,
        };

        return totalUseOfPower;
    }

    public static TimeSeries<double> CalculateFlaring(Project project, Case caseItem, DrainageStrategy drainageStrategy)
    {
        var oilRate = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.Values.Select(v => v).ToArray() ?? [];
        var additionalOilRate = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.Values.Select(v => v).ToArray() ?? [];

        var gasRate = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values.Select(v => v / ConversionFactorFromMtoG).ToArray() ?? [];
        var additionalGasRate = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values.Select(v => v / ConversionFactorFromMtoG).ToArray() ?? [];

        // Create TimeSeries<double> instances for both oil and gas profiles
        var oilRateTs = new TimeSeries<double>
        {
            Values = oilRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.StartYear ?? 0,
        };

        var additionalOilRateTs = new TimeSeries<double>
        {
            Values = additionalOilRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)?.StartYear ?? 0,
        };

        var gasRateTs = new TimeSeries<double>
        {
            Values = gasRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0,
        };

        var additionalGasRateTs = new TimeSeries<double>
        {
            Values = additionalGasRate,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.StartYear ?? 0,
        };

        var mergedOilProfile = CostProfileMerger.MergeCostProfiles(oilRateTs, additionalOilRateTs);
        var mergedGasProfile = CostProfileMerger.MergeCostProfiles(gasRateTs, additionalGasRateTs);
        var mergedOilAndGas = CostProfileMerger.MergeCostProfiles(mergedOilProfile, mergedGasProfile);

        var flaringValues = mergedOilAndGas.Values.Select(v => v * project.FlaredGasPerProducedVolume).ToArray();
        var flaring = new TimeSeries<double>
        {
            Values = flaringValues,
            StartYear = mergedOilAndGas.StartYear,
        };

        return flaring;
    }

    public static TimeSeries<double> CalculateLosses(Project project, Case caseItem, DrainageStrategy drainageStrategy)
    {
        var lossesValues = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values.Select(v => v * project.CO2RemovedFromGas).ToArray() ?? [];
        var additionalGasLossesValues = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values.Select(v => v * project.CO2RemovedFromGas).ToArray() ?? [];

        // Create TimeSeries<double> instances for both gas losses profiles
        var gasLossesTs = new TimeSeries<double>
        {
            Values = lossesValues,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.StartYear ?? 0,
        };

        var additionalGasLossesTs = new TimeSeries<double>
        {
            Values = additionalGasLossesValues,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.StartYear ?? 0,
        };

        var mergedGasLosses = CostProfileMerger.MergeCostProfiles(gasLossesTs, additionalGasLossesTs);

        var losses = new TimeSeries<double>
        {
            Values = mergedGasLosses.Values,
            StartYear = mergedGasLosses.StartYear,
        };
        return losses;
    }
}
