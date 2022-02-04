using System;
using System.Linq;

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
        explorationService.CreateExploration(testExploration, caseId);

        var explorations = fixture.context.Projects.FirstOrDefault(o =>
                o.Name == project.Name).Explorations;
        var retrievedExploration = explorations.FirstOrDefault(o => o.Name ==
                testExploration.Name);
        Assert.NotNull(retrievedExploration);
        TestHelper.CompareExplorations(testExploration, retrievedExploration);
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
                .WithExplorationCostProfile(new ExplorationCostBuilder()
                    .WithYearValue(2023, 44)
                    .WithYearValue(2024, 45.7)
                )
                .WithExplorationDrillingSchedule(new ExplorationDrillingScheduleBuilder()
                    .WithYearValue(2023, 4)
                    .WithYearValue(2024, 3)
                )
                .WithGAndGAdminCost(new WithGAndGAdminCostBuilder()
                    .WithYearValue(2023, 60.7)
                    .WithYearValue(2024, 67.4)
                );
    }

}
