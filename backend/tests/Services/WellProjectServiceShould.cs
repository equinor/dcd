using System;
using System.Linq;

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
    private readonly IServiceProvider _serviceProvider;

    public WellProjectServiceShould()
    {
        fixture = new DatabaseFixture();
        var serviceCollection = new ServiceCollection();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public void GetWellProjects()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory, _serviceProvider);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var expectedWellProjects = fixture.context.WellProjects.ToList().Where(o => o.Project.Id == project.Id);

        // Act
        var actualWellProjects = wellProjectService.GetWellProjects(project.Id);

        // Assert
        Assert.Equal(expectedWellProjects.Count(), actualWellProjects.Count());
        var wellProjectsExpectedAndActual = expectedWellProjects.OrderBy(d => d.Name)
            .Zip(actualWellProjects.OrderBy(d => d.Name));
        foreach (var wellProjectPair in wellProjectsExpectedAndActual)
        {
            TestHelper.CompareWellProjects(wellProjectPair.First, wellProjectPair.Second);
        }
    }

    [Fact]
    public void CreateNewWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory, _serviceProvider);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedWellProject = CreateTestWellProject(project);

        // Act
        var projectResult = wellProjectService.CreateWellProject(expectedWellProject, caseId);

        // Assert
        var actualWellProject = projectResult.WellProjects.FirstOrDefault(o => o.Name == expectedWellProject.Name);
        Assert.NotNull(actualWellProject);
        TestHelper.CompareWellProjects(expectedWellProject, actualWellProject);
        var case_ = fixture.context.Cases.FirstOrDefault(o => o.Id == caseId);
        Assert.Equal(actualWellProject.Id, case_.WellProjectLink);
    }

    [Fact]
    public void ThrowNotInDatabaseExceptionWhenCreatingWellProjectWithBadProjectId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory, _serviceProvider);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedWellProject = CreateTestWellProject(new Project { Id = new Guid() });

        // Act, assert
        Assert.Throws<NotFoundInDBException>(() => wellProjectService.CreateWellProject(expectedWellProject, caseId));
    }

    [Fact]
    public void ThrowNotFoundInDatabaseExceptionWhenCreatingWellProjectWithBadCaseId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory, _serviceProvider);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var expectedWellProject = CreateTestWellProject(project);

        // Act, assert
        Assert.Throws<NotFoundInDBException>(() => wellProjectService.CreateWellProject(expectedWellProject, new Guid()));
    }

    [Fact]
    public void DeleteWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory, _serviceProvider);
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
        var projectResult = wellProjectService.DeleteWellProject(wellProjectToDelete.Id);

        // Assert
        var actualWellProject = projectResult.WellProjects.FirstOrDefault(o => o.Name == wellProjectToDelete.Name);
        Assert.Null(actualWellProject);
        var casesWithWellProjectLink = projectResult.Cases.Where(o => o.WellProjectLink == wellProjectToDelete.Id);
        Assert.Empty(casesWithWellProjectLink);
    }

    [Fact]
    public void ThrowArgumentExceptionIfTryingToDeleteNonExistentWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory, _serviceProvider);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var wellProjectToDelete = CreateTestWellProject(project);
        fixture.context.WellProjects.Add(wellProjectToDelete);
        fixture.context.SaveChanges();

        // Act
        wellProjectService.DeleteWellProject(wellProjectToDelete.Id);

        // Act, assert
        Assert.Throws<ArgumentException>(() => wellProjectService.DeleteWellProject(wellProjectToDelete.Id));
    }

    [Fact]
    public void UpdateWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory, _serviceProvider);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldWellProject = CreateTestWellProject(project);
        fixture.context.WellProjects.Add(oldWellProject);
        fixture.context.SaveChanges();
        var updatedWellProject = CreateUpdatedWellProject(project);
        updatedWellProject.Id = oldWellProject.Id;

        // Act
        var projectResult = wellProjectService.UpdateWellProject(updatedWellProject);

        // Assert
        var actualWellProject = projectResult.WellProjects.FirstOrDefault(o => o.Name == updatedWellProject.Name);
        Assert.NotNull(actualWellProject);
        TestHelper.CompareWellProjects(updatedWellProject, actualWellProject);
    }

    [Fact]
    public void ThrowArgumentExceptionIfTryingToUpdateNonExistentWellProject()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory, _serviceProvider);
        var wellProjectService = new WellProjectService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldWellProject = CreateTestWellProject(project);
        fixture.context.WellProjects.Add(oldWellProject);
        fixture.context.SaveChanges();
        var updatedWellProject = CreateUpdatedWellProject(project);

        // Act, assert
        Assert.Throws<ArgumentException>(() => wellProjectService.UpdateWellProject(updatedWellProject));
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
