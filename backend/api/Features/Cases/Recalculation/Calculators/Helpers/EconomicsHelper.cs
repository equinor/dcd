using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.Helpers;

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

    public static TimeSeries CalculateCashFlow(TimeSeries income, TimeSeries totalCost)
    {
        return TimeSeriesMerger.MergeTimeSeriesWithSubtraction(income, totalCost);
    }
}
