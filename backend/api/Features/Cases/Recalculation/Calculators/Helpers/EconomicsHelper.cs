using api.Features.TimeSeriesCalculators;
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
            accumulatedVolume += values[i] / Math.Pow(discountFactor, startIndex + i + 1);
        }

        return accumulatedVolume;
    }

    public static TimeSeriesCost CalculateCashFlow(TimeSeriesCost income, TimeSeriesCost totalCost)
    {
        int startYear = Math.Min(income.StartYear, totalCost.StartYear);
        int endYear = Math.Max(income.StartYear + income.Values.Length - 1, totalCost.StartYear + totalCost.Values.Length - 1);

        int numberOfYears = endYear - startYear + 1;
        var cashFlowValues = new double[numberOfYears];

        for (int yearIndex = 0; yearIndex < numberOfYears; yearIndex++)
        {
            int currentYear = startYear + yearIndex;
            int incomeIndex = currentYear - income.StartYear;
            int costIndex = currentYear - totalCost.StartYear;

            double incomeValue = (incomeIndex >= 0 && incomeIndex < income.Values.Length) ? income.Values[incomeIndex] : 0;
            double costValue = (costIndex >= 0 && costIndex < totalCost.Values.Length) ? totalCost.Values[costIndex] : 0;

            cashFlowValues[yearIndex] = incomeValue - costValue;
        }

        return new TimeSeriesCost
        {
            StartYear = startYear,
            Values = cashFlowValues
        };
    }

    public static TimeSeriesCost MergeProductionAndAdditionalProduction(TimeSeriesProfile? t1, TimeSeriesProfile? t2)
    {
        return CostProfileMerger.MergeCostProfiles(
            new TimeSeriesCost
            {
                StartYear = t1?.StartYear ?? 0,
                Values = t1?.Values ?? []
            },
            new TimeSeriesCost
            {
                StartYear = t2?.StartYear ?? 0,
                Values = t2?.Values ?? []
            }
        );
    }
}
