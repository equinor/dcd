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
            var discountedValue = values[i] * discountFactors[i + valuesStartYear + Math.Abs(discountYearInRelationToDg4Year)];
            accumulatedVolume += discountedValue;

        }

        return accumulatedVolume;
    }

    public static List<double> GetDiscountFactors(double discountRatePercentage, int numYears)
    {
        List<double> discountFactors = new List<double>();
        double discountRate = 1 + (discountRatePercentage / 100);

        for (int year = 0; year < numYears; year++)
        {
            double discountFactor = (year == 0) ? 1 : 1 / Math.Pow(discountRate, year);
            discountFactors.Add(discountFactor);
        }

        return discountFactors;
    }



    public static TimeSeries CalculateCashFlow(TimeSeries income, TimeSeries totalCost)
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

            cashFlowValues[yearIndex] = (incomeValue / 10) - costValue;
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
