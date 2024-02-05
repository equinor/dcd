using api.Adapters;
using api.Dtos;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;

namespace tests;

[Collection("Database collection")]
public class WellProjectServiceShould : IDisposable
{
    private readonly DatabaseFixture fixture;

    public WellProjectServiceShould()
    {
        fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public async Task CreateNewWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedWellProject = CreateTestWellProject(project);

        // Act
        var projectResult = await wellProjectService.CreateWellProject(expectedWellProject, caseId);

        // Assert
        var actualWellProject = projectResult.WellProjects.FirstOrDefault(o => o.Name == expectedWellProject.Name);
        Assert.NotNull(actualWellProject);
        TestHelper.CompareWellProjects(expectedWellProject, actualWellProject);
        var case_ = fixture.context.Cases.FirstOrDefault(o => o.Id == caseId);
        Assert.Equal(actualWellProject.Id, case_.WellProjectLink);
    }

    [Fact]
    public async Task ThrowNotInDatabaseExceptionWhenCreatingWellProjectWithBadProjectId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedWellProject = CreateTestWellProject(new Project { Id = new Guid() });

        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await wellProjectService.CreateWellProject(expectedWellProject, caseId));
    }

    [Fact]
    public async Task ThrowNotFoundInDatabaseExceptionWhenCreatingWellProjectWithBadCaseId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var expectedWellProject = CreateTestWellProject(project);

        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await wellProjectService.CreateWellProject(expectedWellProject, new Guid()));
    }

    [Fact]
    public async Task DeleteWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var wellProjectToDelete = CreateTestWellProject(project);
        fixture.context.WellProjects.Add(wellProjectToDelete);
        fixture.context.Cases.Add(new Case
        {
            Project = project,
            WellProjectLink = wellProjectToDelete.Id
        });
        fixture.context.SaveChanges();

        // Act
        var projectResult = await wellProjectService.DeleteWellProject(wellProjectToDelete.Id);

        // Assert
        var actualWellProject = projectResult.WellProjects.FirstOrDefault(o => o.Name == wellProjectToDelete.Name);
        Assert.Null(actualWellProject);
        var casesWithWellProjectLink = projectResult.Cases.Where(o => o.WellProjectLink == wellProjectToDelete.Id);
        Assert.Empty(casesWithWellProjectLink);
    }

    [Fact]
    public async Task ThrowArgumentExceptionIfTryingToDeleteNonExistentWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var wellProjectToDelete = CreateTestWellProject(project);
        fixture.context.WellProjects.Add(wellProjectToDelete);
        fixture.context.SaveChanges();

        // Act
        await wellProjectService.DeleteWellProject(wellProjectToDelete.Id);

        // Act, assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await wellProjectService.DeleteWellProject(wellProjectToDelete.Id));
    }

    [Fact]
    public async Task UpdateWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldWellProject = CreateTestWellProject(project);
        fixture.context.WellProjects.Add(oldWellProject);
        fixture.context.SaveChanges();
        var updatedWellProject = CreateUpdatedWellProject(project);
        updatedWellProject.Id = oldWellProject.Id;

        // Act
        var projectResult = await wellProjectService.UpdateWellProject(updatedWellProject);

        // Assert
        var actualWellProject = projectResult.WellProjects.FirstOrDefault(o => o.Name == updatedWellProject.Name);
        Assert.NotNull(actualWellProject);
        TestHelper.CompareWellProjects(updatedWellProject, actualWellProject);
    }

    [Fact]
    public async Task ThrowArgumentExceptionIfTryingToUpdateNonExistentWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldWellProject = CreateTestWellProject(project);
        fixture.context.WellProjects.Add(oldWellProject);
        fixture.context.SaveChanges();
        var updatedWellProject = CreateUpdatedWellProject(project);

        // Act, assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await wellProjectService.UpdateWellProject(updatedWellProject));
    }

    private static WellProject CreateTestWellProject(Project project)
    {
        return new WellProjectBuilder
        {
            Name = "DrainStrat Test",
            ArtificialLift = ArtificialLift.ElectricalSubmergedPumps,
            Project = project,
            ProjectId = project.Id,
        };
    }

    private static WellProjectDto CreateUpdatedWellProject(Project project)
    {
        return WellProjectDtoAdapter.Convert(new WellProjectBuilder
        {
            Name = "updated name",
            ArtificialLift = ArtificialLift.GasLift,
            Project = project,
            ProjectId = project.Id,
        }
        );
    }
}
