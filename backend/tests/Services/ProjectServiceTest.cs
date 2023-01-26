using api.Models;
using api.SampleData.Generators;
using api.Services;

using Xunit;

namespace tests;

[Collection("Database collection")]
public class ProjectServiceTest : IDisposable
{
    readonly DatabaseFixture fixture;

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
        var loggerFactory = new LoggerFactory();
        var projectFromSampleDataGenerator = SampleCaseGenerator.initializeCases(SampleAssetGenerator.initializeAssets()).Projects.OrderBy(p => p.Name);
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var projectsFromService = projectService.GetAll().OrderBy(p => p.Name);
        var projectsExpectedActual = projectFromSampleDataGenerator.Zip(projectsFromService);
        Assert.Equal(projectFromSampleDataGenerator.Count(), projectsFromService.Count());
        foreach (var projectPair in projectsExpectedActual)
        {
            TestHelper.CompareProjects(projectPair.First, projectPair.Second);
        }
    }

    [Fact]
    public void GetProject()
    {
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        IEnumerable<Project> projectsFromGetAllService = projectService.GetAll();
        var projectsFromSampleDataGenerator = SampleCaseGenerator.initializeCases(SampleAssetGenerator.initializeAssets()).Projects;
        Assert.Equal(projectsFromSampleDataGenerator.Count, projectsFromGetAllService.Count());
        foreach (var project in projectsFromGetAllService)
        {
            var projectFromGetProjectService = projectService.GetProject(project.Id);
            var projectFromSampleDataGenerator = projectsFromSampleDataGenerator.Find(p => p.Name == project.Name);
            TestHelper.CompareProjects(projectFromSampleDataGenerator, projectFromGetProjectService);
        }
    }

    [Fact]
    public void GetDoesNotExist()
    {
        var loggerFactory = new LoggerFactory();
        ProjectService projectService = new ProjectService(fixture.context, loggerFactory);
        Assert.Throws<NotFoundInDBException>(() => projectService.GetProject(new Guid()));
    }
}
