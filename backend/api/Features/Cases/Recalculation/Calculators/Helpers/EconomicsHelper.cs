using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.Helpers;

public static class EconomicsHelper
{
    public static double CalculateDiscountedVolume(double[] values, double discountRate, int startIndex)
    {
        double accumulatedVolume = 0;
        double discountFactor = 1 + (discountRate / 100);

        for (int i = 0; i < values.Length; i++)
        {
            accumulatedVolume += values[i] / Math.Pow(discountFactor, startIndex + i);
        }

        return accumulatedVolume;
    }

    public static TimeSeries CalculateCashFlow(TimeSeries income, TimeSeries total)
    {
        int startYear = Math.Min(income.StartYear, total.StartYear);
        int endYear = Math.Max(income.StartYear + income.Values.Length - 1, total.StartYear + total.Values.Length - 1);

        int numberOfYears = endYear - startYear + 1;
        var cashFlowValues = new double[numberOfYears];

        for (int yearIndex = 0; yearIndex < numberOfYears; yearIndex++)
        {
            int currentYear = startYear + yearIndex;
            int incomeIndex = currentYear - income.StartYear;
            int costIndex = currentYear - total.StartYear;

            double incomeValue = (incomeIndex >= 0 && incomeIndex < income.Values.Length) ? income.Values[incomeIndex] : 0;
            double costValue = (costIndex >= 0 && costIndex < total.Values.Length) ? total.Values[costIndex] : 0;

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
