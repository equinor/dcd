using System;
using System.Collections.Generic;
using System.Linq;

using api.Models;
using api.SampleData.Builders;
using api.SampleData.Generators;
using api.Services;

using Xunit;


namespace tests
{
    [Collection("Database collection")]
    public class ProjectServiceTest : IDisposable
    {
        DatabaseFixture fixture;

        public ProjectServiceTest()
        {
            fixture = new DatabaseFixture();
        }

        public void Dispose()
        {
            fixture.Dispose();
        }

        [Fact]
        public void GetAll()
        {
            var projectFromSampleDataGenerator = SampleCaseGenerator.initializeCases(SampleAssetGenerator.initializeAssets()).Projects.OrderBy(p => p.ProjectName);
            ProjectService projectService = new ProjectService(fixture.context);
            var projectsFromService = projectService.GetAll().OrderBy(p => p.ProjectName);
            var projectsExpectedActual = projectFromSampleDataGenerator.Zip(projectsFromService);
            Assert.Equal(projectFromSampleDataGenerator.Count(), projectsFromService.Count());
            foreach (var projectPair in projectsExpectedActual)
            {
                compareProjects(projectPair.First, projectPair.Second);
            }
        }

        [Fact]
        public void GetProject()
        {
            ProjectService projectService = new ProjectService(fixture.context);
            IEnumerable<Project> projectsFromGetAllService = projectService.GetAll();
            var projectsFromSampleDataGenerator = SampleCaseGenerator.initializeCases(SampleAssetGenerator.initializeAssets()).Projects;
            Assert.Equal(projectsFromSampleDataGenerator.Count(), projectsFromGetAllService.Count());
            foreach (var project in projectsFromGetAllService)
            {
                var projectFromGetProjectService = projectService.GetProject(project.Id);
                var projectFromSampleDataGenerator = projectsFromSampleDataGenerator.Find(p => p.ProjectName == project.ProjectName);
                compareProjects(projectFromSampleDataGenerator, projectFromGetProjectService);
            }
        }
        void compareProjects(Project expected, Project actual)
        {
            Assert.Equal(expected.ProjectName, actual.ProjectName);
            Assert.Equal(expected.ProjectPhase, actual.ProjectPhase);
            Assert.Equal(expected.ProjectCategory, actual.ProjectCategory);
            Assert.Equal(expected.Cases.Count(), actual.Cases.Count());
            var casesSourceAndTarget = expected.Cases.OrderBy(c => c.Name).Zip(actual.Cases.OrderBy(c => c.Name));
            foreach (var casePair in casesSourceAndTarget)
            {
                compareCases(casePair.First, casePair.Second);
            }
            Assert.Equal(expected.DrainageStrategies.Count(), actual.DrainageStrategies.Count());
            var drainageStrategiesExpectedAndActual = expected.DrainageStrategies.OrderBy(d => d.Name)
                .Zip(actual.DrainageStrategies.OrderBy(d => d.Name));
            foreach (var drainageStrategyPair in drainageStrategiesExpectedAndActual)
            {
                compareDrainageStrategies(drainageStrategyPair.First, drainageStrategyPair.Second);
            }
            Assert.Equal(expected.WellProjects.Count(), actual.WellProjects.Count());
            var wellProjectsExpectedAndActual = expected.WellProjects.OrderBy(d => d.Name)
                .Zip(actual.WellProjects.OrderBy(d => d.Name));
            foreach (var wellProjectPair in wellProjectsExpectedAndActual)
            {
                compareWellProjects(wellProjectPair.First, wellProjectPair.Second);
            }
            Assert.Equal(expected.Explorations.Count(), actual.Explorations.Count());
            var explorationsExpectedAndActual = expected.Explorations.OrderBy(d => d.Name)
                .Zip(actual.Explorations.OrderBy(d => d.Name));
            foreach (var explorationPair in explorationsExpectedAndActual)
            {
                compareExplorations(explorationPair.First, explorationPair.Second);
            }
            Assert.Equal(expected.Substructures.Count(), actual.Substructures.Count());
            var substructuresExpectedAndActual = expected.Substructures.OrderBy(d => d.Name)
              .Zip(actual.Substructures.OrderBy(d => d.Name));
            foreach (var substructurePair in substructuresExpectedAndActual)
            {
                compareSubstructures(substructurePair.First, substructurePair.Second);
            }
            Assert.Equal(expected.Surfs.Count(), actual.Surfs.Count());
            var surfsExpectedAndActual = expected.Surfs.OrderBy(d => d.Name)
             .Zip(actual.Surfs.OrderBy(d => d.Name));
            foreach (var surfPair in surfsExpectedAndActual)
            {
                compareSurfs(surfPair.First, surfPair.Second);
            }
            Assert.Equal(expected.Topsides.Count(), actual.Topsides.Count());
            var topsidesExpectedAndActual = expected.Topsides.OrderBy(d => d.Name)
             .Zip(actual.Topsides.OrderBy(d => d.Name));
            foreach (var topsidePair in topsidesExpectedAndActual)
            {
                compareTopsides(topsidePair.First, topsidePair.Second);
            }
            Assert.Equal(expected.Transports.Count(), actual.Transports.Count());
            var transportsExpectedAndActual = expected.Transports.OrderBy(d => d.Name)
            .Zip(actual.Transports.OrderBy(d => d.Name));
            foreach (var transportPair in transportsExpectedAndActual)
            {
                compareTransports(transportPair.First, transportPair.Second);
            }
        }

        void compareCases(Case expected, Case actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.ReferenceCase, actual.ReferenceCase);
        }

        void compareDrainageStrategies(DrainageStrategy expected, DrainageStrategy actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                Assert.Equal(expected.NGLYield, actual.NGLYield);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.ArtificialLift, actual.ArtificialLift);
                compareVolumes(expected.ProductionProfileOil, actual.ProductionProfileOil);
                compareVolumes(expected.ProductionProfileGas, actual.ProductionProfileGas);
                compareVolumes(expected.ProductionProfileWater, actual.ProductionProfileWater);
                compareVolumes(expected.ProductionProfileWaterInjection, actual.ProductionProfileWaterInjection);
                compareVolumes(expected.NetSalesGas, actual.NetSalesGas);
                compareMasses(expected.Co2Emissions, actual.Co2Emissions);
            }
        }

        void compareWellProjects(WellProject expected, WellProject actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
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

        void compareExplorations(Exploration expected, Exploration actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.WellType, actual.WellType);
                Assert.Equal(expected.RigMobDemob, actual.RigMobDemob);
                TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
                TestHelper.CompareYearValues(expected.DrillingSchedule, actual.DrillingSchedule);
                TestHelper.CompareCosts(expected.GAndGAdminCost, actual.GAndGAdminCost);
            }
        }

        void compareSubstructures(Substructure expected, Substructure actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Maturity, actual.Maturity);
                TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
                Assert.Equal(expected.DryWeight, actual.DryWeight);
            }
        }

        void compareSurfs(Surf expected, Surf actual)
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

        void compareTopsides(Topside expected, Topside actual)
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

        void compareTransports(Transport expected, Transport actual)
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

        void compareVolumes(TimeSeriesVolume expected, TimeSeriesVolume actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                TestHelper.CompareYearValues(expected, actual);

            }
        }
        void compareMasses(TimeSeriesMass expected, TimeSeriesMass actual)
        {
            if (expected == null || actual == null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                TestHelper.CompareYearValues(expected, actual);
            }
        }
        [Fact]
        public void GetDoesNotExist()
        {
            ProjectService projectService = new ProjectService(fixture.context);
            Assert.Throws<NotFoundInDBException>(() => projectService.GetProject(new Guid()));
        }
    }
}
