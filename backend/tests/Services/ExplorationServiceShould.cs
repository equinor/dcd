using System;
using System.Linq;

using api.Adapters;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;


namespace tests;

[Collection("Database collection")]
public class ExplorationServiceShould : IDisposable
{
    private readonly DatabaseFixture fixture;

    public ExplorationServiceShould(DatabaseFixture fixture)
    {
        this.fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public void GetExplorations()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var explorationService = new ExplorationService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault();
        var expectedExplorations = fixture.context.Explorations.ToList().Where(o => o.Project.Id == project.Id);

        // Act
        var actualExplorations = explorationService.GetExplorations(project.Id);

        // Assert
        Assert.Equal(expectedExplorations.Count(), actualExplorations.Count());
        var explorationsExpectedAndActual = expectedExplorations.OrderBy(d => d.Name)
            .Zip(actualExplorations.OrderBy(d => d.Name));
        foreach (var explorationPair in explorationsExpectedAndActual)
        {
            TestHelper.CompareExplorations(explorationPair.First, explorationPair.Second);
        }
    }

    [Fact]
    public void CreateNewExploration()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var explorationService = new ExplorationService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var testExploration = CreateTestExploration(project);
        explorationService.CreateExploration(ExplorationDtoAdapter.Convert(testExploration), caseId);

        var explorations = fixture.context.Projects.FirstOrDefault(o =>
                o.Name == project.Name).Explorations;
        var retrievedExploration = explorations.FirstOrDefault(o => o.Name ==
                testExploration.Name);
        Assert.NotNull(retrievedExploration);
        TestHelper.CompareExplorations(testExploration, retrievedExploration);
    }

    [Fact]
    public void DeleteExploration()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var explorationService = new ExplorationService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault();
        var sourceCaseId = project.Cases.FirstOrDefault().Id;
        var ExplorationToDelete = CreateTestExploration(project);
        explorationService.CreateExploration(ExplorationDtoAdapter.Convert(ExplorationToDelete), sourceCaseId);

        var deletedExplorationId = project.Explorations.First().Id;
        // Act
        var projectResult = explorationService.DeleteExploration(deletedExplorationId);

        // Assert
        var actualExploration = projectResult.DrainageStrategies.FirstOrDefault(o => o.Name == ExplorationToDelete.Name);
        // No case links to deleted exploration
        foreach (Case c in fixture.context.Cases!)
        {
            Assert.NotEqual(c.ExplorationLink, deletedExplorationId);
        }
        Assert.Null(actualExploration);
    }

    [Fact]
    public void UpdateExploration()
    {
        // Arrange
        var projectService = new ProjectService(fixture.context);
        var explorationService = new ExplorationService(fixture.context, projectService);
        var project = fixture.context.Projects.FirstOrDefault();
        var sourceCaseId = project.Cases.FirstOrDefault().Id;
        var oldExploration = CreateTestExploration(project);
        var oldId = explorationService.CreateExploration(ExplorationDtoAdapter.Convert(oldExploration), sourceCaseId).Explorations.Last().Id;

        var updatedExploration = CreateUpdatedTestExploration(project, oldId);

        // Act
        var projectResult = explorationService.UpdateExploration(ExplorationDtoAdapter.Convert(updatedExploration));


        //     // Assert
        //     var actualExploration = projectResult.Explorations.FirstOrDefault(o => o.Name == updatedExploration.Name);
        //     Assert.NotNull(actualExploration);
        //     TestHelper.CompareExplorations(updatedExploration, actualExploration);
    }

    private static Exploration CreateTestExploration(Project project)
    {
        return new ExplorationBuilder
        {
            Name = "Test-exploration-23",
            Project = project,
            ProjectId = project.Id,
            WellType = WellType.Gas,
            RigMobDemob = 32.7
        }
              .WithExplorationCostProfile(new ExplorationCostProfile()
              {
                  Currency = Currency.USD,
                  StartYear = 2230,
                  Values = new double[] { 131.4, 28.2, 334.3 }
              }
                )
                .WithExplorationDrillingSchedule(new ExplorationDrillingSchedule()
                {
                    StartYear = 2050,
                    Values = new int[] { 143, 5, 45 }
                }
                )
                .WithGAndGAdminCost(new GAndGAdminCost()
                {
                    Currency = Currency.NOK,
                    StartYear = 2010,
                    Values = new double[] { 314.4, 281.2, 34.3 }
                }

                );
    }

    private static Exploration CreateUpdatedTestExploration(Project project, Guid oldExplorationId)
    {
        return new ExplorationBuilder
        {
            Id = oldExplorationId,
            Name = "Test-exploration-23",
            Project = project,
            ProjectId = project.Id,
            WellType = WellType.Gas,
            RigMobDemob = 32.7
        }
                 .WithExplorationCostProfile(new ExplorationCostProfile()
                 {
                     Currency = Currency.NOK,
                     StartYear = 2010,
                     Values = new double[] { 11.4, 28.2, 34.3 }
                 }
                )
                .WithExplorationDrillingSchedule(new ExplorationDrillingSchedule()
                {
                    StartYear = 2030,
                    Values = new int[] { 123, 5, 5 }
                }
                )
                .WithGAndGAdminCost(new GAndGAdminCost()
                {
                    Currency = Currency.USD,
                    StartYear = 2030,
                    Values = new double[] { 31.4, 282.2, 34.3 }
                }

                );
    }

}
