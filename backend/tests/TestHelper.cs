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

        public static void CompareExplorations(Exploration expected, Exploration actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, null);
                Assert.Equal(actual, null);
            }
            else
            {
                Assert.Equal(expected.Project.Id, actual.Project.Id);
                Assert.Equal(expected.ProjectId, actual.ProjectId);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.WellType, actual.WellType);

                TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
                Assert.Equal(expected.CostProfile.Exploration.Name,
                        actual.CostProfile.Exploration.Name);

                TestHelper.CompareYearValues(expected.DrillingSchedule,
                        actual.DrillingSchedule);
                Assert.Equal(expected.DrillingSchedule.Exploration.Name,
                        actual.DrillingSchedule.Exploration.Name);

                TestHelper.CompareCosts(expected.GAndGAdminCost,
                        actual.GAndGAdminCost);
                Assert.Equal(expected.GAndGAdminCost.Exploration.Name,
                        actual.GAndGAdminCost.Exploration.Name);

                Assert.Equal(expected.RigMobDemob, actual.RigMobDemob);
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
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Maturity, actual.Maturity);
                CompareCosts(expected.CostProfile, actual.CostProfile);
                Assert.Equal(expected.DryWeight, actual.DryWeight);
            }
        }

        public static void CompareWellProjects(WellProject expected, WellProject actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.RigMobDemob, actual.RigMobDemob);
                Assert.Equal(expected.AnnualWellInterventionCost, actual.AnnualWellInterventionCost);
                Assert.Equal(expected.PluggingAndAbandonment, actual.PluggingAndAbandonment);
                Assert.Equal(expected.ProducerCount, actual.ProducerCount);
                Assert.Equal(expected.GasInjectorCount, actual.GasInjectorCount);
                Assert.Equal(expected.WaterInjectorCount, actual.WaterInjectorCount);
                Assert.Equal(expected.ArtificialLift, actual.ArtificialLift);
                TestHelper.CompareYearValues(expected.DrillingSchedule, actual.DrillingSchedule);
                TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
            }
        }

        public static void CompareTopsides(Topside expected, Topside actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Maturity, actual.Maturity);
                Assert.Equal(expected.DryWeight, actual.DryWeight);
                Assert.Equal(expected.GasCapacity, actual.GasCapacity);
                Assert.Equal(expected.OilCapacity, actual.OilCapacity);
                Assert.Equal(expected.FacilitiesAvailability, actual.FacilitiesAvailability);
                Assert.Equal(expected.ArtificialLift, actual.ArtificialLift);
                TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
            }
        }
    }
}
