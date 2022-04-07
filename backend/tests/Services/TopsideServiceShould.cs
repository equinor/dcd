using System;
using System.Linq;

using api.Adapters;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;

namespace tests;

[Collection("Database collection")]
public class TopsideServiceShould : IDisposable
{
    private readonly DatabaseFixture fixture;

    public TopsideServiceShould(DatabaseFixture fixture)
    {
        this.fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public void GetTopsides()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var topsideService = new TopsideService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault();
        var expectedTopsides = fixture.context.Topsides.ToList().Where(o => o.Project.Id == project.Id);

        // Act
        var actualTopsides = topsideService.GetTopsides(project.Id);

        // Assert
        Assert.Equal(expectedTopsides.Count(), actualTopsides.Count());
        var topsidesExpectedAndActual = expectedTopsides.OrderBy(d => d.Name)
            .Zip(actualTopsides.OrderBy(d => d.Name));
        foreach (var topsidePair in topsidesExpectedAndActual)
        {
            TestHelper.CompareTopsides(topsidePair.First, topsidePair.Second);
        }
    }

    [Fact]
    public void CreateNewTopside()
    {
        // Arrange
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var testTopside = CreateTestTopside(project);
        ProjectService projectService = new ProjectService(fixture.context);
        TopsideService topsideService = new TopsideService(fixture.context, projectService);

        // Act
        var projectResult = topsideService.CreateTopside(TopsideDtoAdapter.Convert(testTopside), caseId);

        // Assert
        var retrievedTopside = projectResult.Topsides.FirstOrDefault(o => o.Name ==
                testTopside.Name);
        Assert.NotNull(retrievedTopside);
        TestHelper.CompareTopsides(testTopside, retrievedTopside);
        var case_ = fixture.context.Cases.FirstOrDefault(o => o.Id == caseId);
        Assert.Equal(retrievedTopside.Id, case_.TopsideLink);
    }

    [Fact]
    public void ThrowNotInDatabaseExceptionWhenCreatingTopsideWithBadProjectId()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var topsideService = new TopsideService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedTopside = CreateTestTopside(new Project { Id = new Guid() });

        // Act, assert
        Assert.Throws<NotFoundInDBException>(() => topsideService.CreateTopside(TopsideDtoAdapter.Convert(expectedTopside), caseId));
    }

    [Fact]
    public void ThrowNotFoundInDatabaseExceptionWhenCreatingTopsideWithBadCaseId()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var topsideService = new TopsideService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var expectedTopside = CreateTestTopside(project);

        // Act, assert
        Assert.Throws<NotFoundInDBException>(() => topsideService.CreateTopside(TopsideDtoAdapter.Convert(expectedTopside), new Guid()));
    }

    [Fact]
    public void DeleteTopside()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var topsideService = new TopsideService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault();
        var topsideToDelete = CreateTestTopside(project);
        fixture.context.Topsides.Add(topsideToDelete);
        fixture.context.Cases.Add(new Case
        {
            Project = project,
            TopsideLink = topsideToDelete.Id
        });
        fixture.context.SaveChanges();

        // Act
        var projectResult = topsideService.DeleteTopside(topsideToDelete.Id);

        // Assert
        var actualTopside = projectResult.Topsides.FirstOrDefault(o => o.Name == topsideToDelete.Name);
        Assert.Null(actualTopside);
        var casesWithTopsideLink = projectResult.Cases.Where(o => o.TopsideLink == topsideToDelete.Id);
        Assert.Empty(casesWithTopsideLink);
    }

    [Fact]
    public void ThrowArgumentExceptionIfTryingToDeleteNonExistentTopside()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var topsideService = new TopsideService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault();
        var topsideToDelete = CreateTestTopside(project);
        fixture.context.Topsides.Add(topsideToDelete);
        fixture.context.SaveChanges();

        // Act, assert
        Assert.Throws<ArgumentException>(() => topsideService.DeleteTopside(new Guid()));
    }

    [Fact]
    public void UpdateTopside()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var topsideService = new TopsideService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldTopside = CreateTestTopside(project);
        fixture.context.Topsides.Add(oldTopside);
        fixture.context.SaveChanges();
        var updatedTopside = CreateUpdatedTopside(project, oldTopside);

        // Act
        var projectResult = topsideService.UpdateTopside(TopsideDtoAdapter.Convert(updatedTopside));

        // Assert
        var actualTopside = projectResult.Topsides.FirstOrDefault(o => o.Name == updatedTopside.Name);
        Assert.NotNull(actualTopside);
        TestHelper.CompareTopsides(updatedTopside, actualTopside);
    }

    [Fact]
    public void ThrowArgumentExceptionIfTryingToUpdateNonExistentTopside()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var topsideService = new TopsideService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldTopside = CreateTestTopside(project);
        fixture.context.Topsides.Add(oldTopside);
        fixture.context.SaveChanges();
        var updatedTopside = CreateUpdatedTopside(project, oldTopside);

        //     // Act, assert
        //     Assert.Throws<ArgumentException>(() => topsideService.UpdateTopside(updatedTopside));
    }
    private static Topside CreateTestTopside(Project project)
    {
        return new TopsideBuilder
        {
            Name = "Test-topside-23",
            Project = project,
            ProjectId = project.Id,
            DryWeight = 0.3e6,
            OilCapacity = 50e6,
            GasCapacity = 0,
            FacilitiesAvailability = 0.2,
            ArtificialLift = ArtificialLift.GasLift,
            Maturity = Maturity.B
        }
                .WithCostProfile(new TopsideCostProfile()
                {
                    Currency = Currency.USD,
                    StartYear = 2030,
                    Values = new double[] { 13.4, 18.9, 34.3 }
                }
                );
    }

    private static Topside CreateUpdatedTopside(Project project, Topside oldTopside)
    {
        return new TopsideBuilder
        {
            Id = oldTopside.Id,
            Name = "Test-topside-34",
            Project = project,
            ProjectId = project.Id,
            DryWeight = 5.3e6,
            OilCapacity = 52e6,
            GasCapacity = 7,
            FacilitiesAvailability = 1.2,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            Maturity = Maturity.C
        }
            .WithCostProfile(new TopsideCostProfile()
            {
                Currency = Currency.NOK,
                StartYear = 2030,
                Values = new double[] { 13.4, 183.9, 34.3 }
            }
            );
    }

}
