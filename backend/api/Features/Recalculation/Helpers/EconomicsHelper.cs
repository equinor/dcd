using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

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

    public static TimeSeries CalculateWithDiscountFactor(double discountRatePercentage, int npvYear, int dg4Year, TimeSeries timeSeries)
    {
        var discountYear = npvYear - dg4Year;
        var offset = timeSeries.StartYear - discountYear;
        var discountFactors = GetDiscountFactors(discountRatePercentage, timeSeries.Values.Length + offset);
        if(offset < 0)
        {
            discountFactors.InsertRange(0, Enumerable.Repeat(1.0, offset));
        }

        var discountFactorsSeries = new TimeSeries(
                discountYear,
                discountFactors.ToArray()
            );
        return TimeSeriesMerger.MergeTimeSeriesWithMultiplication(discountFactorsSeries, timeSeries);
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
    /// <param name="netSalesGas"> net gas sales in m3 </param>
    /// <param name="gasPriceNok"> nok price per m3 </param>
    /// <param name="usdToNok"> currency rate usd to nok </param>
    /// <param name='projectCurrency'> currency of the project </param>
    public static TimeSeries CalculateTotalGasIncome(TimeSeries netSalesGas, double gasPriceNok, double usdToNok, Currency projectCurrency)
    {
        var rate = projectCurrency == Currency.Nok ? 1.0 : 1 / usdToNok;

        return new TimeSeries(
            netSalesGas.StartYear,
            netSalesGas.Values
                .Select(value => value * gasPriceNok * rate)
                .ToArray()
            );
    }

    /// <summary>(gas production - flaring loss)</summary>
    /// <param name="totalGasProduction"> production in m3 </param>
    /// <param name="fuelFlaringAndLosses"> gas loss in m3</param>
    /// <param name="strategy"> case drainage strategy </param>
    public static TimeSeries CalculateTotalGasSales(TimeSeries totalGasProduction, TimeSeries fuelFlaringAndLosses, DrainageStrategy strategy)
    {
        var gasSolution = strategy.GasSolution;
        var nglYield = strategy.NglYield;
        var condensateYield = strategy.CondensateYield;
        var gasShrinkageFactor = strategy.GasShrinkageFactor;

        if (gasSolution == GasSolution.Injection)
        {
            return new TimeSeries();
        }

        if (nglYield + condensateYield <= 0)
        {
            return TimeSeriesMerger.MergeTimeSeriesWithSubtraction(totalGasProduction, fuelFlaringAndLosses);
        }

        var gasAdjustedForShrinkageFactor = new TimeSeries
        {
            StartYear = totalGasProduction.StartYear,
            Values = totalGasProduction.Values.Select(value => value * (gasShrinkageFactor / 100)).ToArray()
        };

        return TimeSeriesMerger.MergeTimeSeriesWithSubtraction(gasAdjustedForShrinkageFactor, fuelFlaringAndLosses);
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
