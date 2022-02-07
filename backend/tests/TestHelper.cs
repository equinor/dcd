using System.Linq;

using api.Models;

using Xunit;

namespace tests
{
    public static class TestHelper
    {
        public static void CompareProjects(Project expected, Project actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.ProjectPhase, actual.ProjectPhase);
            Assert.Equal(expected.ProjectCategory, actual.ProjectCategory);
            Assert.Equal(expected.Cases.Count(), actual.Cases.Count());
            var casesSourceAndTarget = expected.Cases.OrderBy(c => c.Name).Zip(actual.Cases.OrderBy(c => c.Name));
            foreach (var casePair in casesSourceAndTarget)
            {
                TestHelper.CompareCases(casePair.First, casePair.Second);
            }
            Assert.Equal(expected.DrainageStrategies.Count(), actual.DrainageStrategies.Count());
            var drainageStrategiesExpectedAndActual = expected.DrainageStrategies.OrderBy(d => d.Name)
                .Zip(actual.DrainageStrategies.OrderBy(d => d.Name));
            foreach (var drainageStrategyPair in drainageStrategiesExpectedAndActual)
            {
                TestHelper.CompareDrainageStrategies(drainageStrategyPair.First, drainageStrategyPair.Second);
            }
            Assert.Equal(expected.WellProjects.Count(), actual.WellProjects.Count());
            var wellProjectsExpectedAndActual = expected.WellProjects.OrderBy(d => d.Name)
                .Zip(actual.WellProjects.OrderBy(d => d.Name));
            foreach (var wellProjectPair in wellProjectsExpectedAndActual)
            {
                TestHelper.CompareWellProjects(wellProjectPair.First, wellProjectPair.Second);
            }
            Assert.Equal(expected.Explorations.Count(), actual.Explorations.Count());
            var explorationsExpectedAndActual = expected.Explorations.OrderBy(d => d.Name)
                .Zip(actual.Explorations.OrderBy(d => d.Name));
            foreach (var explorationPair in explorationsExpectedAndActual)
            {
                TestHelper.CompareExplorations(explorationPair.First, explorationPair.Second);
            }
            Assert.Equal(expected.Substructures.Count(), actual.Substructures.Count());
            var substructuresExpectedAndActual = expected.Substructures.OrderBy(d => d.Name)
              .Zip(actual.Substructures.OrderBy(d => d.Name));
            foreach (var substructurePair in substructuresExpectedAndActual)
            {
                TestHelper.CompareSubstructures(substructurePair.First, substructurePair.Second);
            }
            Assert.Equal(expected.Surfs.Count(), actual.Surfs.Count());
            var surfsExpectedAndActual = expected.Surfs.OrderBy(d => d.Name)
             .Zip(actual.Surfs.OrderBy(d => d.Name));
            foreach (var surfPair in surfsExpectedAndActual)
            {
                TestHelper.CompareSurfs(surfPair.First, surfPair.Second);
            }
            Assert.Equal(expected.Topsides.Count(), actual.Topsides.Count());
            var topsidesExpectedAndActual = expected.Topsides.OrderBy(d => d.Name)
             .Zip(actual.Topsides.OrderBy(d => d.Name));
            foreach (var topsidePair in topsidesExpectedAndActual)
            {
                TestHelper.CompareTopsides(topsidePair.First, topsidePair.Second);
            }
            Assert.Equal(expected.Transports.Count(), actual.Transports.Count());
            var transportsExpectedAndActual = expected.Transports.OrderBy(d => d.Name)
            .Zip(actual.Transports.OrderBy(d => d.Name));
            foreach (var transportPair in transportsExpectedAndActual)
            {
                TestHelper.CompareTransports(transportPair.First, transportPair.Second);
            }
        }

        public static void CompareCases(Case expected, Case actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.ReferenceCase, actual.ReferenceCase);
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

         public static void CompareSurfs(Surf expected, Surf actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Maturity, actual.Maturity);
                Assert.Equal(expected.RiserCount, actual.RiserCount);
                Assert.Equal(expected.TemplateCount, actual.TemplateCount);
                Assert.Equal(expected.ArtificialLift, actual.ArtificialLift);
                Assert.Equal(expected.InfieldPipelineSystemLength, actual.InfieldPipelineSystemLength);
                Assert.Equal(expected.UmbilicalSystemLength, actual.UmbilicalSystemLength);
                Assert.Equal(expected.ProductionFlowline, actual.ProductionFlowline);
                TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
            }
        }
         public static void CompareTransports(Transport expected, Transport actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Maturity, actual.Maturity);
                Assert.Equal(expected.OilExportPipelineLength, actual.OilExportPipelineLength);
                Assert.Equal(expected.GasExportPipelineLength, actual.GasExportPipelineLength);
                TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
            }
        }

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
    }
}
