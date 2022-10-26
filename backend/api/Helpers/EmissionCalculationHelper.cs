using api.Models;

namespace api.Helpers;

public static class EmissionCalculationHelper
{
    private const double Co2EmissionRemovedFromGas = 0;
    private const double FlaredGasPerProducedVolume = 1.321;
    private const double Co2EmissionFromFuelGas = 2.34;
    private const double Co2EmissionFromFlaredGas = 3.74;
    private const double Co2Vented = 1.96;
    private const int AverageDevelopmentWellDrillingDays = 50;
    private const int DailyEmissionFromDrillingRig = 100;
    private const int DaysInLeapYear = 366;
    private const int DaysInYear = 365;

    public static TimeSeries<double> CalculateTotalFuelConsumptions(Case caseItem, Topside topside,
        DrainageStrategy drainageStrategy)
    {
        var totalUseOfPower = CalculateTotalUseOfPower(topside, drainageStrategy);
        var productionEfficiency = caseItem.FacilitiesAvailability;
        var fuelGasConsumptionMax = topside.FuelConsumption;

        var valuesList = new List<double>();
        for (var i = totalUseOfPower.StartYear; i < totalUseOfPower.StartYear + totalUseOfPower.Values.Length; i++)
        {
            var year = caseItem.DG4Date.Year + i;
            var calendarDays = DateTime.IsLeapYear(year) ? DaysInLeapYear : DaysInYear;

            var fuelGasConsumption = productionEfficiency * fuelGasConsumptionMax * calendarDays / 1000;

            valuesList.Add(fuelGasConsumption);
        }

        var newTimeSeries = new TimeSeries<double>
        {
            StartYear = totalUseOfPower.StartYear,
            Values = valuesList.ToArray(),
        };


        return TimeSeriesCost.MergeCostProfiles(totalUseOfPower, newTimeSeries);
    }

    private static TimeSeries<double> CalculateTotalUseOfPower(Topside topside, DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerOil = CalculateTotalUseOfPowerOil(topside, drainageStrategy);
        var totalUseOfPowerGas = CalculateTotalUseOfPowerGas(topside, drainageStrategy);
        var totalUseOfPowerWaterInjection = CalculateTotalUseOfPowerWaterInjection(topside, drainageStrategy);

        var totalUseOfPower = TimeSeriesCost.MergeCostProfiles(
            TimeSeriesCost.MergeCostProfiles(totalUseOfPowerOil, totalUseOfPowerGas), totalUseOfPowerWaterInjection);

        return totalUseOfPower;
    }

    private static TimeSeriesVolume CalculateTotalUseOfPowerWaterInjection(Topside topside,
        DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerWaterInjection = new TimeSeriesVolume();
        var co2ShareWaterInjectionProfile = topside.CO2ShareWaterInjectionProfile;
        var co2OnMaxWaterInjection = topside.CO2OnMaxWaterInjectionProfile;


        if (drainageStrategy.ProductionProfileWaterInjection == null)
        {
            return totalUseOfPowerWaterInjection;
        }

        var waterInjectionRates = drainageStrategy.ProductionProfileWaterInjection.Values;

        totalUseOfPowerWaterInjection.StartYear = drainageStrategy.ProductionProfileWaterInjection.StartYear;
        totalUseOfPowerWaterInjection.Values = waterInjectionRates.Select(waterInjectionValue =>
            co2ShareWaterInjectionProfile * co2OnMaxWaterInjection +
            waterInjectionValue * co2ShareWaterInjectionProfile * (1 - co2OnMaxWaterInjection)).ToArray();

        return totalUseOfPowerWaterInjection;
    }

    private static TimeSeriesVolume CalculateTotalUseOfPowerGas(Topside topside, DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerGas = new TimeSeriesVolume();
        var co2ShareGasProfile = topside.CO2ShareGasProfile;
        var co2OnMaxGas = topside.CO2OnMaxGasProfile;

        if (drainageStrategy.ProductionProfileGas == null)
        {
            return totalUseOfPowerGas;
        }

        var gasRates = drainageStrategy.ProductionProfileGas.Values;

        totalUseOfPowerGas.StartYear = drainageStrategy.ProductionProfileGas.StartYear;
        totalUseOfPowerGas.Values = gasRates.Select(gasValue =>
            co2ShareGasProfile * co2OnMaxGas + gasValue * co2ShareGasProfile * (1 - co2OnMaxGas)).ToArray();

        return totalUseOfPowerGas;
    }

    private static TimeSeriesVolume CalculateTotalUseOfPowerOil(Topside topside, DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerOil = new TimeSeriesVolume();
        var co2ShareOilProfile = topside.CO2ShareOilProfile;
        var co2OnMaxOil = topside.CO2OnMaxOilProfile;

        if (drainageStrategy.ProductionProfileOil == null)
        {
            return totalUseOfPowerOil;
        }

        var oilRates = drainageStrategy.ProductionProfileOil.Values;

        totalUseOfPowerOil.StartYear = drainageStrategy.ProductionProfileOil.StartYear;
        totalUseOfPowerOil.Values = oilRates.Select(oilValue =>
            co2ShareOilProfile * co2OnMaxOil + oilValue * co2ShareOilProfile * (1 - co2OnMaxOil)).ToArray();

        return totalUseOfPowerOil;
    }

    public static TimeSeries<double> CalculateFlaring(DrainageStrategy drainageStrategy)
    {
        var oilValues = new List<double>();
        var gasValues = new List<double>();
        var oilRatesInFlaring = CalculatedOilRatesInFlaring(drainageStrategy, oilValues);
        var gasRatesInFlaring = CalculatedGasRatesInFlaring(drainageStrategy, gasValues);
        return TimeSeriesCost.MergeCostProfiles(oilRatesInFlaring, gasRatesInFlaring);
    }

    private static TimeSeriesVolume CalculatedOilRatesInFlaring(DrainageStrategy drainageStrategy,
        List<double> oilValues)
    {
        if (drainageStrategy.ProductionProfileOil == null)
        {
            return new TimeSeriesVolume()!;
        }

        var oilRates = drainageStrategy.ProductionProfileOil.Values;
        oilValues.AddRange(oilRates.Select(oilValue => oilValue * FlaredGasPerProducedVolume / 1000));

        return new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileOil.StartYear,
            Values = oilValues.ToArray(),
        };
    }

    private static TimeSeriesVolume CalculatedGasRatesInFlaring(DrainageStrategy drainageStrategy,
        List<double> gasValues)
    {
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return new TimeSeriesVolume();
        }

        var gasRates = drainageStrategy.ProductionProfileGas.Values;
        gasValues.AddRange(gasRates.Select(gasValue => gasValue * FlaredGasPerProducedVolume / 1000));

        return new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileGas.StartYear,
            Values = gasValues.ToArray(),
        };
    }

    public static TimeSeries<double> CalculateLosses(DrainageStrategy drainageStrategy)
    {
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return new TimeSeries<double>();
        }

        var gasRates = drainageStrategy.ProductionProfileGas.Values;

        return new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileGas.StartYear,
            Values = gasRates.Select(gasValue => gasValue * Co2EmissionRemovedFromGas).ToArray(),
        };
    }
}
