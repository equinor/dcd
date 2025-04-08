using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models.Enums;

using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Recalculation.Helpers;

public static class EconomicsHelper
{
    public static double CalculateSumOfDiscountedVolume(double[] values, double discountRatePercentage, int valuesStartYear, int discountYearInRelationToDg4Year)
    {
        var discountFactors = GetDiscountFactors(discountRatePercentage, values.Length + Math.Abs(valuesStartYear + Math.Abs(discountYearInRelationToDg4Year)));

        double accumulatedVolume = 0;

        for (var i = 0; i < values.Length; i++)
        {
            var discountFactorIndex = i + valuesStartYear + Math.Abs(discountYearInRelationToDg4Year);

            if (discountFactorIndex < 0 || discountFactorIndex >= discountFactors.Count)
            {
                continue;
            }

            var discountedValue = values[i] * discountFactors[discountFactorIndex];
            accumulatedVolume += discountedValue;
        }

        return accumulatedVolume;
    }

    public static TimeSeries CalculateDiscountedVolume(double[] values, double discountRatePercentage, int valuesStartYear, int discountYearInRelationToDg4Year)
    {
        var discountFactors = GetDiscountFactors(discountRatePercentage, values.Length + Math.Abs(valuesStartYear + Math.Abs(discountYearInRelationToDg4Year)));

        var discountedValues = new double[values.Length];

        for (var i = 0; i < values.Length; i++)
        {
            var discountFactorIndex = i + valuesStartYear + Math.Abs(discountYearInRelationToDg4Year);

            if (discountFactorIndex < 0 || discountFactorIndex >= discountFactors.Count)
            {
                discountedValues[i] = 0;

                continue;
            }

            discountedValues[i] = values[i] * discountFactors[discountFactorIndex];
        }

        return new TimeSeries
        {
            StartYear = valuesStartYear,
            Values = discountedValues
        };
    }

    public static List<double> GetDiscountFactors(double discountRatePercentage, int numYears)
    {
        var discountFactors = new List<double>();
        var discountRate = 1 + discountRatePercentage / 100;

        for (var year = 0; year < numYears; year++)
        {
            var discountFactor = year == 0 ? 1 : 1 / Math.Pow(discountRate, year);
            discountFactors.Add(discountFactor);
        }

        return discountFactors;
    }

    /// <summary>
    /// income - cost
    /// </summary>
    public static TimeSeries CalculateCashFlow(TimeSeries income, TimeSeries cost)
    {
        return TimeSeriesMerger.MergeTimeSeriesWithSubtraction(income, cost);
    }

    /// <summary>(gas production - flaring loss) * price</summary>
    /// <param name="totalGasProduction"> production in m3 </param>
    /// <param name="fuelFlaringAndLosses"> gas loss in m3</param>
    /// <param name="gasPriceNok"> nok price per m3 </param>
    /// <param name="usdToNok"> currency rate usd to nok </param>
    /// <param name='projectCurrency'> currency of the project </param>
    public static TimeSeries CalculateTotalGasIncome(TimeSeries totalGasProduction,TimeSeries fuelFlaringAndLosses, double gasPriceNok, double usdToNok, Currency projectCurrency)
    {
        var rate = projectCurrency == Currency.Nok ? 1.0 : 1 / usdToNok;
        var totalGasSales = TimeSeriesMerger.MergeTimeSeriesWithSubtraction(totalGasProduction, fuelFlaringAndLosses);

        return new TimeSeries(
            totalGasSales.StartYear,
            totalGasSales.Values
                .Select(value => value * gasPriceNok * rate)
                .ToArray()
            );
    }

    /// <summary>(oil production + condensate production) * price</summary>
    /// <param name="totalOilProduction"> production in m3 </param>
    /// <param name="totalCondensateProduction">condensate in m3</param>
    /// <param name="oilPriceUsd"> usd price per barrel </param>
    /// <param name="usdToNok"> currency rate usd to nok </param>
    /// <param name='projectCurrency'> currency of the project </param>
    public static TimeSeries CalculateTotalOilIncome(TimeSeries totalOilProduction, TimeSeries totalCondensateProduction,double oilPriceUsd, double usdToNok, Currency projectCurrency)
    {
        var rate = projectCurrency == Currency.Usd ? 1.0 : usdToNok;
        var totalOilSales = TimeSeriesMerger.MergeTimeSeries(totalOilProduction, totalCondensateProduction);
        return new TimeSeries(
            totalOilSales.StartYear,
            totalOilSales.Values
                .Select(value => value * BarrelsPerCubicMeter * oilPriceUsd * rate)
                .ToArray()
        );
    }

    /// <summary>production * price</summary>
    /// <param name="totalNglProduction"> production in ton </param>
    /// <param name="nglPriceUsd"> usd price per ton </param>
    /// <param name="usdToNok"> currency rate usd to nok </param>
    /// <param name='projectCurrency'> currency of the project </param>
    public static TimeSeries CalculateTotalNglIncome(TimeSeries totalNglProduction, double nglPriceUsd, double usdToNok, Currency projectCurrency)
    {
        return new TimeSeries(
            totalNglProduction.StartYear,
            totalNglProduction.Values
                .Select(value => value * nglPriceUsd)
                .ToArray()
        );
    }
}
