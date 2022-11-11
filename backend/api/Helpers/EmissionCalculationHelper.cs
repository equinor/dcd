using api.Models;

namespace api.Helpers;

public static class EmissionCalculationHelper
{
    private const int DaysInLeapYear = 366;
    private const int DaysInYear = 365;
    private const int ConversionFactorFromMtoG = 1000;

    public static TimeSeries<double> CalculateTotalFuelConsumptions(Case caseItem, Topside topside,
        DrainageStrategy drainageStrategy)
    {
        var totalUseOfPower = CalculateTotalUseOfPower(caseItem, topside, drainageStrategy);
        var productionEfficiency = caseItem.FacilitiesAvailability;
        var fuelGasConsumptionMax = topside.FuelConsumption;

        var valuesList = new List<double>();
        for (var i = totalUseOfPower.StartYear; i < totalUseOfPower.StartYear + totalUseOfPower.Values.Length; i++)
        {
            var year = caseItem.DG4Date.Year + i;
            var calendarDays = DateTime.IsLeapYear(year) ? DaysInLeapYear : DaysInYear;

            var fuelGasConsumption = productionEfficiency * fuelGasConsumptionMax * calendarDays / ConversionFactorFromMtoG;

            valuesList.Add(fuelGasConsumption);
        }

        var newTimeSeries = new TimeSeries<double>
        {
            StartYear = totalUseOfPower.StartYear,
            Values = valuesList.ToArray(),
        };


        return TimeSeriesCost.MergeCostProfiles(totalUseOfPower, newTimeSeries);
    }

    private static TimeSeries<double> CalculateTotalUseOfPower(Case caseItem, Topside topside, DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerOil = CalculateTotalUseOfPowerOil(caseItem, topside, drainageStrategy);
        var totalUseOfPowerGas = CalculateTotalUseOfPowerGas(caseItem, topside, drainageStrategy);
        var totalUseOfPowerWaterInjection = CalculateTotalUseOfPowerWaterInjection(caseItem, topside, drainageStrategy);

        var totalUseOfPower = TimeSeriesCost.MergeCostProfiles(
            TimeSeriesCost.MergeCostProfiles(totalUseOfPowerOil, totalUseOfPowerGas), totalUseOfPowerWaterInjection);

        return totalUseOfPower;
    }

    private static TimeSeriesVolume CalculateTotalUseOfPowerWaterInjection(Case caseItem, Topside topside,
        DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerWaterInjection = new TimeSeriesVolume();
        var co2ShareWaterInjectionProfile = topside.CO2ShareWaterInjectionProfile;
        var co2OnMaxWaterInjection = topside.CO2OnMaxWaterInjectionProfile;


        if (drainageStrategy.ProductionProfileWaterInjection == null)
        {
            return totalUseOfPowerWaterInjection;
        }

        var waterInjectionRatesProfile = new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileWaterInjection.StartYear,
            Values = drainageStrategy.ProductionProfileWaterInjection.Values,
        };

        var index = 0;
        for (var i = waterInjectionRatesProfile.StartYear; i < waterInjectionRatesProfile.StartYear + waterInjectionRatesProfile.Values.Length; i++, index++)
        {
            var year = caseItem.DG4Date.Year + i;
            var calendarDays = DateTime.IsLeapYear(year) ? DaysInLeapYear : DaysInYear;

            waterInjectionRatesProfile.Values[index] = waterInjectionRatesProfile.Values[index] / topside.WaterInjectionCapacity / calendarDays;
        }

        totalUseOfPowerWaterInjection.StartYear = drainageStrategy.ProductionProfileWaterInjection.StartYear;
        totalUseOfPowerWaterInjection.Values = waterInjectionRatesProfile.Values.Select(waterInjectionValue =>
            co2ShareWaterInjectionProfile * co2OnMaxWaterInjection +
            waterInjectionValue * co2ShareWaterInjectionProfile * (1 - co2OnMaxWaterInjection)).ToArray();

        return totalUseOfPowerWaterInjection;
    }

    private static TimeSeriesVolume CalculateTotalUseOfPowerGas(Case caseItem, Topside topside, DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerGas = new TimeSeriesVolume();
        var co2ShareGasProfile = topside.CO2ShareGasProfile;
        var co2OnMaxGas = topside.CO2OnMaxGasProfile;

        if (drainageStrategy.ProductionProfileGas == null)
        {
            return totalUseOfPowerGas;
        }

        var gasRatesProfile = new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileGas.StartYear,
            Values = drainageStrategy.ProductionProfileGas.Values,
        };

        var index = 0;
        for (var i = gasRatesProfile.StartYear; i < gasRatesProfile.StartYear + gasRatesProfile.Values.Length; i++, index++)
        {
            var year = caseItem.DG4Date.Year + i;
            var calendarDays = DateTime.IsLeapYear(year) ? DaysInLeapYear : DaysInYear;

            const int million = 1000000;
            gasRatesProfile.Values[index] = gasRatesProfile.Values[index] / topside.GasCapacity * calendarDays / million;
        }

        totalUseOfPowerGas.StartYear = drainageStrategy.ProductionProfileGas.StartYear;
        totalUseOfPowerGas.Values = gasRatesProfile.Values.Select(gasRateProfile =>
            co2ShareGasProfile * co2OnMaxGas + gasRateProfile * co2ShareGasProfile * (1 - co2OnMaxGas)).ToArray();

        return totalUseOfPowerGas;
    }

    private static TimeSeriesVolume CalculateTotalUseOfPowerOil(Case caseItem, Topside topside, DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerOil = new TimeSeriesVolume();
        var co2ShareOilProfile = topside.CO2ShareOilProfile;
        var co2OnMaxOil = topside.CO2OnMaxOilProfile;

        if (drainageStrategy.ProductionProfileOil == null)
        {
            return totalUseOfPowerOil;
        }

        var oilRatesProfile = new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileOil.StartYear,
            Values = drainageStrategy.ProductionProfileOil.Values
        };

        var index = 0;
        for (var i = oilRatesProfile.StartYear; i < oilRatesProfile.StartYear + oilRatesProfile.Values.Length; i++, index++)
        {
            var year = caseItem.DG4Date.Year + i;
            var calendarDays = DateTime.IsLeapYear(year) ? DaysInLeapYear : DaysInYear;

            oilRatesProfile.Values[index] = oilRatesProfile.Values[index] / topside.OilCapacity / calendarDays;
        }

        totalUseOfPowerOil.StartYear = drainageStrategy.ProductionProfileOil.StartYear;
        totalUseOfPowerOil.Values = oilRatesProfile.Values.Select(oilRateProfile =>
            co2ShareOilProfile * co2OnMaxOil + oilRateProfile * co2ShareOilProfile * (1 - co2OnMaxOil)).ToArray();

        return totalUseOfPowerOil;
    }

    public static TimeSeries<double> CalculateFlaring(Project project, DrainageStrategy drainageStrategy)
    {
        var oilValues = new List<double>();
        var gasValues = new List<double>();
        var oilRatesInFlaring = CalculatedOilRatesInFlaring(project, drainageStrategy, oilValues);
        var gasRatesInFlaring = CalculatedGasRatesInFlaring(project, drainageStrategy, gasValues);
        return TimeSeriesCost.MergeCostProfiles(oilRatesInFlaring, gasRatesInFlaring);
    }

    private static TimeSeriesVolume CalculatedOilRatesInFlaring(Project project, DrainageStrategy drainageStrategy,
        List<double> oilValues)
    {
        if (drainageStrategy.ProductionProfileOil == null)
        {
            return new TimeSeriesVolume()!;
        }

        var oilRates = drainageStrategy.ProductionProfileOil.Values;
        oilValues.AddRange(oilRates.Select(oilValue => oilValue * project.FlaredGasPerProducedVolume / ConversionFactorFromMtoG));

        return new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileOil.StartYear,
            Values = oilValues.ToArray(),
        };
    }

    private static TimeSeriesVolume CalculatedGasRatesInFlaring(Project project, DrainageStrategy drainageStrategy,
        List<double> gasValues)
    {
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return new TimeSeriesVolume();
        }

        var gasRates = drainageStrategy.ProductionProfileGas.Values;
        gasValues.AddRange(gasRates.Select(gasValue => gasValue * project.FlaredGasPerProducedVolume / ConversionFactorFromMtoG));

        return new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileGas.StartYear,
            Values = gasValues.ToArray(),
        };
    }

    public static TimeSeries<double> CalculateLosses(Project project, DrainageStrategy drainageStrategy)
    {
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return new TimeSeries<double>();
        }

        var gasRates = drainageStrategy.ProductionProfileGas.Values;

        return new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileGas.StartYear,
            Values = gasRates.Select(gasValue => gasValue * project.CO2RemovedFromGas).ToArray(),
        };
    }
}
