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
        var wrpWspWom = wrp.Select(v => step1 + v * 100 * wsp * (1 - wom));

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
        var grp = mergedProfile.Values.Select(v => v / (Cd * (facilitiesAvailability / 100)) / gc / 1_000_000);
        var grpGspGom = grp.Select(v => step1 + v * gsp * (1 - gom));

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
        var orpOspOom = orp.Select(v => step1 + v * osp * (1 - oom)).ToArray();

        return new TimeSeries
        {
            Values = orpOspOom,
            StartYear = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)?.StartYear ?? 0
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
            Values = mergedOilAndGas.Values.Select(v => v * caseItem.Project.FlaredGasPerProducedVolume).ToArray(),
            StartYear = mergedOilAndGas.StartYear
        };
    }

    public static TimeSeries CalculateLosses(Case caseItem)
    {
        var lossesValues = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas)?.Values.Select(v => v * caseItem.Project.CO2RemovedFromGas).ToArray() ?? [];
        var additionalGasLossesValues = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)?.Values.Select(v => v * caseItem.Project.CO2RemovedFromGas).ToArray() ?? [];

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
