using System;
using System.Collections.Generic;
using System.Linq;

using api.Models;
using api.Services;

using Xunit;


namespace tests
{
    [Collection("Database collection")]
    public class ProjectServiceTest
    {
        DatabaseFixture fixture;

        public ProjectServiceTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public void GetAll()
        {
            var projectFromTestDataGenerator = TestDataGenerator.initialize().Projects.OrderBy(p => p.ProjectName);
            ProjectService projectService = new ProjectService(fixture.context);
            var projectsFromService = projectService.GetAll().OrderBy(p => p.ProjectName);
            var projectsSourceAndTarget = projectFromTestDataGenerator.Zip(projectsFromService);
            Assert.Equal(projectFromTestDataGenerator.Count(), projectsFromService.Count());
            foreach (var projectPair in projectsSourceAndTarget)
            {
                compareProjects(projectPair.First, projectPair.Second);
            }
        }

        [Fact]
        public void GetProject()
        {
            ProjectService projectService = new ProjectService(fixture.context);
            IEnumerable<Project> projectsFromGetAllService = projectService.GetAll();
            var projectsFromTestDataGenerator = TestDataGenerator.initialize().Projects;
            foreach (var project in projectsFromGetAllService)
            {
                var projectFromGetProjectService = projectService.GetProject(project.Id);
                var projectFromTestDataGenerator = projectsFromTestDataGenerator.Find(p => p.ProjectName == project.ProjectName);
                compareProjects(projectFromTestDataGenerator, projectFromGetProjectService);
            }
        }
        void compareProjects(Project x, Project y)
        {
            Assert.Equal(x.ProjectName, y.ProjectName);
            Assert.Equal(x.ProjectPhase, y.ProjectPhase);
            Assert.Equal(x.Cases.Count(), y.Cases.Count());
            var casesSourceAndTarget = x.Cases.OrderBy(c => c.Name).Zip(y.Cases.OrderBy(c => c.Name));
            foreach (var casePair in casesSourceAndTarget)
            {
                compareCases(casePair.First, casePair.Second);
            }
        }

        void compareCases(Case x, Case y)
        {
            Assert.Equal(x.Name, y.Name);
            Assert.Equal(x.Description, y.Description);
            Assert.Equal(x.ReferenceCase, y.ReferenceCase);
            Assert.Equal(x.ProducerCount, y.ProducerCount);
            Assert.Equal(x.GasInjectorCount, y.GasInjectorCount);
            Assert.Equal(x.WaterInjectorCount, y.WaterInjectorCount);
            Assert.Equal(x.RiserCount, y.RiserCount);
            Assert.Equal(x.TemplateCount, y.TemplateCount);
            Assert.Equal(x.FacilitiesAvailability, y.FacilitiesAvailability);
            Assert.Equal(x.ArtificialLift, y.ArtificialLift);
            compareCosts(x.CessationCost, y.CessationCost);
            compareDrainageStrategies(x.DrainageStrategy, y.DrainageStrategy);
        }

        void compareCosts<T>(TimeSeriesCost<T> x, TimeSeriesCost<T> y)
        {
            if (x == null || y == null)
            {
                Assert.Equal(x, null);
                Assert.Equal(y, null);
            }
            else
            {
                compareYearValues(x, y);
                Assert.Equal(x.Currency, y.Currency);
            }
        }
        void compareDrainageStrategies(DrainageStrategy x, DrainageStrategy y)
        {
            if (x == null || y == null)
            {
                Assert.Equal(x, null);
                Assert.Equal(y, null);
            }
            else
            {
                Assert.Equal(x.NGLYield, y.NGLYield);
                compareYearValues(x.ProductionProfileOil, y.ProductionProfileOil);
                compareYearValues(x.ProductionProfileGas, y.ProductionProfileGas);
            }
        }
        void compareYearValues<T>(TimeSeriesBase<T> x, TimeSeriesBase<T> y)
        {
            if (x == null || y == null)
            {
                Assert.Equal(x, null);
                Assert.Equal(y, null);
            }
            else
            {
                var yearValuePairsXY = x.YearValues.OrderBy(v => v.Year).Zip(y.YearValues.OrderBy(v => v.Year));
                foreach (var pair in yearValuePairsXY)
                {
                    Assert.Equal(pair.First.Year, pair.Second.Year);
                    Assert.Equal(pair.First.Value, pair.Second.Value);
                }
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
