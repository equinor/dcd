using System;
using System.Collections.Generic;
using System.Linq;

using api.Models;
using api.SampleData.Generators;
using api.Services;

using Xunit;


namespace tests
{
    [Collection("Database collection")]
    public class SurfServiceTest
    {
        readonly DatabaseFixture fixture;
        readonly IOrderedEnumerable<api.SampleData.Builders.ProjectBuilder> projectsFromSampleDataGenerator;

        public SurfServiceTest(DatabaseFixture fixture)
        {
            //arrange
            this.fixture = new DatabaseFixture();
            projectsFromSampleDataGenerator = SampleCaseGenerator.initializeCases(SampleAssetGenerator.initializeAssets()).Projects.OrderBy(p => p.Name);

        }

        [Fact]
        public void GetAllSurf()
        {
            var loggerFactory = new LoggerFactory();
            ProjectService projectService = new ProjectService(fixture.context, loggerFactory);
            SurfService surfService = new SurfService(fixture.context, projectService, loggerFactory);
            var project = projectsFromSampleDataGenerator.First();
            surfService.GetSurfs(project.Id);

        }
    }
}
