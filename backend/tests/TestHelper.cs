using System.Linq;

using api.Models;

using Xunit;

namespace tests
{
    public static class TestHelper
    {
        public static void CompareYearValues<T>(TimeSeriesBase<T> expected, TimeSeriesBase<T> actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.YearValues.Count, actual.YearValues.Count);
                var yearValuePairsXY = expected.YearValues.OrderBy(v => v.Year).Zip(actual.YearValues.OrderBy(v => v.Year));
                foreach (var pair in yearValuePairsXY)
                {
                    Assert.Equal(pair.First.Year, pair.Second.Year);
                    Assert.Equal(pair.First.Value, pair.Second.Value);
                }
            }
        }

        public static void CompareDrainageStrategies(DrainageStrategy expected, DrainageStrategy actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.NGLYield, actual.NGLYield);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.GasInjectorCount, actual.GasInjectorCount);
                Assert.Equal(expected.WaterInjectorCount, actual.WaterInjectorCount);
                Assert.Equal(expected.ProducerCount, actual.ProducerCount);
                CompareVolumes(expected.ProductionProfileOil, actual.ProductionProfileOil);
                CompareVolumes(expected.ProductionProfileGas, actual.ProductionProfileGas);
                CompareVolumes(expected.ProductionProfileWater, actual.ProductionProfileWater);
                CompareVolumes(expected.ProductionProfileWaterInjection, actual.ProductionProfileWaterInjection);
                CompareVolumes(expected.NetSalesGas, actual.NetSalesGas);
                CompareMasses(expected.Co2Emissions, actual.Co2Emissions);
            }
        }

        public static void CompareVolumes(TimeSeriesVolume expected, TimeSeriesVolume actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                CompareYearValues(expected, actual);
            }
        }
        public static void CompareMasses(TimeSeriesMass expected, TimeSeriesMass actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                CompareYearValues(expected, actual);
            }
        }
        public static void CompareCosts<T>(TimeSeriesCost<T> expected, TimeSeriesCost<T> actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                CompareYearValues(expected, actual);
                Assert.Equal(expected.Currency, actual.Currency);
                Assert.Equal(expected.EPAVersion, actual.EPAVersion);
            }
        }

        public static void CompareSubstructures(Substructure expected, Substructure actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Maturity, actual.Maturity);
                CompareCosts(expected.CostProfile, actual.CostProfile);
                Assert.Equal(expected.DryWeight, actual.DryWeight);
            }
        }
    }
}
