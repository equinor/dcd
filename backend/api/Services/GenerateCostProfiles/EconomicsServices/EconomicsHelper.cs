using api.Models;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace EconomicsServices
{
    public static class EconomicsHelper
    {

        public static double CalculateDiscountedVolume(double[] values, double discountRate, int startIndex)
        {
            double accumulatedVolume = 0;
            for (int i = 0; i < values.Length; i++)
            {
                accumulatedVolume += values[i] / Math.Pow(1 + (discountRate / 100), startIndex + i + 1);
            }
            return accumulatedVolume;
        }


        public static TimeSeries<double> CalculateCashFlow(TimeSeries<double> income, TimeSeries<double> totalCost)
        {
            var startYear = Math.Min(income.StartYear, totalCost.StartYear);
            var endYear = Math.Max(
                income.StartYear + income.Values.Length - 1,
                totalCost.StartYear + totalCost.Values.Length - 1
            );

            var incomeValues = new double[endYear - startYear + 1];
            var costValues = new double[endYear - startYear + 1];

            for (int i = 0; i < income.Values.Length; i++)
            {
                int yearIndex = income.StartYear + i - startYear;
                incomeValues[yearIndex] = income.Values[i];
            }

            for (int i = 0; i < totalCost.Values.Length; i++)
            {
                int yearIndex = totalCost.StartYear + i - startYear;
                costValues[yearIndex] = totalCost.Values[i];
            }

            var cashFlowValues = new double[incomeValues.Length];
            for (int i = 0; i < cashFlowValues.Length; i++)
            {
                cashFlowValues[i] = incomeValues[i] - costValues[i];
            }

            return new TimeSeries<double>
            {
                StartYear = startYear,
                Values = cashFlowValues
            };
        }
    }
}