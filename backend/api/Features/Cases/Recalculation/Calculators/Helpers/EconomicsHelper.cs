using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.Helpers;

public static class EconomicsHelper
{
    public static double CalculateDiscountedVolume(double[] values, double discountRatePercentage, int valuesStartYear, int discountYearInRelationToDg4Year)
    {
        var discountFactors = GetDiscountFactors(discountRatePercentage, values.Length + Math.Abs(valuesStartYear + Math.Abs(discountYearInRelationToDg4Year)));

        double accumulatedVolume = 0;

        for (int i = 0; i < values.Length; i++)
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

    private static List<double> GetDiscountFactors(double discountRatePercentage, int numYears)
    {
        var discountFactors = new List<double>();
        var discountRate = 1 + (discountRatePercentage / 100);

        for (var year = 0; year < numYears; year++)
        {
            var discountFactor = year == 0 ? 1 : 1 / Math.Pow(discountRate, year);
            discountFactors.Add(discountFactor);
        }

        return discountFactors;
    }

    public static TimeSeries CalculateCashFlow(TimeSeries income, TimeSeries totalCost)
    {
        var startYear = Math.Min(income.StartYear, totalCost.StartYear);
        var endYear = Math.Max(income.StartYear + income.Values.Length - 1, totalCost.StartYear + totalCost.Values.Length - 1);

        var numberOfYears = endYear - startYear + 1;
        var cashFlowValues = new double[numberOfYears];

        for (var yearIndex = 0; yearIndex < numberOfYears; yearIndex++)
        {
            var currentYear = startYear + yearIndex;
            var incomeIndex = currentYear - income.StartYear;
            var costIndex = currentYear - totalCost.StartYear;

            var incomeValue = (incomeIndex >= 0 && incomeIndex < income.Values.Length) ? income.Values[incomeIndex] : 0;
            var costValue = (costIndex >= 0 && costIndex < totalCost.Values.Length) ? totalCost.Values[costIndex] : 0;

            cashFlowValues[yearIndex] = incomeValue - costValue;
        }

        return new TimeSeries
        {
            StartYear = startYear,
            Values = cashFlowValues
        };
    }

    public static TimeSeries MergeProductionAndAdditionalProduction(TimeSeriesProfile? t1, TimeSeriesProfile? t2)
    {
        return TimeSeriesMerger.MergeTimeSeries(
            new TimeSeries(t1),
            new TimeSeries(t2)
        );
    }
}
