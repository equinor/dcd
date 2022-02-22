using System.Linq;

using api.Dtos;
using api.Models;

using Xunit;

namespace tests
{
    public static class TestHelper
    {
        public static void CompareProjects(Project expected, Project actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.CommonLibraryId, actual.CommonLibraryId);
            Assert.Equal(expected.CommonLibraryName, actual.CommonLibraryName);
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
                // CompareVolumes(expected.ProductionProfileOil, actual.ProductionProfileOil);
                // CompareVolumes(expected.ProductionProfileGas, actual.ProductionProfileGas);
                // CompareVolumes(expected.ProductionProfileWater, actual.ProductionProfileWater);
                // CompareVolumes(expected.ProductionProfileWaterInjection, actual.ProductionProfileWaterInjection);
                // CompareVolumes(expected.NetSalesGas, actual.NetSalesGas);
                // CompareMasses(expected.Co2Emissions, actual.Co2Emissions);
            }
        }

        public static void CompareDrainageStrategies(DrainageStrategy expected, DrainageStrategyDto actual)
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

        public static void CompareTransports(Transport expected, Transport actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.OilExportPipelineLength, actual.OilExportPipelineLength);
                Assert.Equal(expected.GasExportPipelineLength, actual.GasExportPipelineLength);
                Assert.Equal(expected.Maturity, actual.Maturity);
                // CompareCosts(expected.CostProfile, actual.CostProfile);
            }
        }

        public static void CompareTransports(Transport expected, TransportDto actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.OilExportPipelineLength, actual.OilExportPipelineLength);
                Assert.Equal(expected.GasExportPipelineLength, actual.GasExportPipelineLength);
                Assert.Equal(expected.Maturity, actual.Maturity);
                CompareCosts(expected.CostProfile, actual.CostProfile);
            }
        }

        public static void CompareExplorations(Exploration expected, Exploration actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.WellType, actual.WellType);

                //   TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
                Assert.Equal(expected.CostProfile.Exploration.Name,
                        actual.CostProfile.Exploration.Name);

                // TestHelper.CompareYearValues(expected.DrillingSchedule,
                //         actual.DrillingSchedule);
                if (expected.DrillingSchedule != null && actual.DrillingSchedule != null)
                {
                    Assert.Equal(expected.DrillingSchedule.Exploration.Name,
                            actual.DrillingSchedule.Exploration.Name);
                }
                // TestHelper.CompareCosts(expected.GAndGAdminCost,
                //         actual.GAndGAdminCost);
                Assert.Equal(expected.GAndGAdminCost.Exploration.Name,
                        actual.GAndGAdminCost.Exploration.Name);

                Assert.Equal(expected.RigMobDemob, actual.RigMobDemob);
            }
        }

        public static void CompareExplorations(Exploration expected, ExplorationDto actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.WellType, actual.WellType);

                TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
                TestHelper.CompareYearValues(expected.DrillingSchedule, actual.DrillingSchedule);
                TestHelper.CompareCosts(expected.GAndGAdminCost, actual.GAndGAdminCost);

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
                //   CompareCosts(expected.CostProfile, actual.CostProfile);
                Assert.Equal(expected.DryWeight, actual.DryWeight);
            }
        }

        public static void CompareSubstructures(Substructure expected, SubstructureDto actual)
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
                //TestHelper.CompareYearValues(expected.DrillingSchedule, actual.DrillingSchedule);
                //    TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
            }
        }

        public static void CompareWellProjects(WellProject expected, WellProjectDto actual)
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
                Assert.Null(expected);
                Assert.Null(actual);
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
                //      TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
            }
        }

        public static void CompareTopsides(Topside expected, TopsideDto actual)
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
                Assert.Null(expected);
                Assert.Null(actual);
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
                //     TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
            }
        }

        public static void CompareYearValues<T>(TimeSeries<T> expected, TimeSeriesDto<T> actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.Values.Length, actual.Values.Length);
                var yearValuePairsXY = expected.Values.OrderBy(v => v).Zip(actual.Values.OrderBy(v => v));
                foreach (var pair in yearValuePairsXY)
                {
                    Assert.Equal(pair.First, pair.Second);
                    Assert.Equal(pair.First, pair.Second);
                }
            }
        }

        public static void CompareVolumes(TimeSeriesVolume expected, TimeSeriesVolumeDto actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                // CompareYearValues(expected, actual);
            }
        }

        public static void CompareMasses(TimeSeriesMass expected, TimeSeriesMassDto actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                //  CompareYearValues(expected, actual);
            }
        }

        public static void CompareCosts(TimeSeriesCost expected, TimeSeriesCostDto actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Null(expected);
                Assert.Null(actual);
            }
            else
            {
                //   CompareYearValues(expected, actual);
                Assert.Equal(expected.Currency, actual.Currency);
                Assert.Equal(expected.EPAVersion, actual.EPAVersion);
            }
        }
    }
}
