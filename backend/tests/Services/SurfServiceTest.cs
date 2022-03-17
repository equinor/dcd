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
            ProjectService projectService = new ProjectService(fixture.context);
            SurfService surfService = new SurfService(fixture.context, projectService);
            var project = projectsFromSampleDataGenerator.First();
            surfService.GetSurfs(project.Id);

        }
    }
}
