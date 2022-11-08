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
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var explorationService = new ExplorationService(fixture.context, projectService, loggerFactory);
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
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var explorationService = new ExplorationService(fixture.context, projectService, loggerFactory);
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
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var explorationService = new ExplorationService(fixture.context, projectService, loggerFactory);
        var project = fixture?.context?.Projects?.FirstOrDefault();
        var sourceCaseId = project?.Cases?.FirstOrDefault()?.Id ?? Guid.Empty;
        var explorationToDelete = CreateTestExploration(project);
        if (sourceCaseId == Guid.Empty)
        {
            return;
        }

        explorationService.CreateExploration(ExplorationDtoAdapter.Convert(explorationToDelete), sourceCaseId)
            .GetAwaiter();

        var deletedExplorationId = project.Explorations.First().Id;
        // Act
        var projectResult = explorationService.DeleteExploration(deletedExplorationId).Result;

        // Assert
        if (projectResult.DrainageStrategies != null)
        {
            var actualExploration =
                projectResult.DrainageStrategies.FirstOrDefault(o => o.Name == explorationToDelete.Name);
            // No case links to deleted exploration
            foreach (var c in fixture.context.Cases!)
            {
                Assert.NotEqual(c.ExplorationLink, deletedExplorationId);
            }

            Assert.Null(actualExploration);
        }
    }

    [Fact]
    public void UpdateExploration()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var explorationService = new ExplorationService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var sourceCaseId = project.Cases.FirstOrDefault().Id;
        var oldExploration = CreateTestExploration(project);
        var oldId = explorationService.CreateExploration(ExplorationDtoAdapter.Convert(oldExploration), sourceCaseId)
            .GetAwaiter().GetResult()
            .Explorations.Last().Id;

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
            RigMobDemob = 32.7,
        }
            .WithExplorationCostProfile(new ExplorationCostProfile
            {
                Currency = Currency.USD,
                StartYear = 2230,
                Values = new[] { 131.4, 28.2, 334.3 },
            }
            )
            .WithGAndGAdminCost(new GAndGAdminCost
            {
                Currency = Currency.NOK,
                StartYear = 2010,
                Values = new[] { 314.4, 281.2, 34.3 },
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
            RigMobDemob = 32.7,
        }
            .WithExplorationCostProfile(new ExplorationCostProfile
            {
                Currency = Currency.NOK,
                StartYear = 2010,
                Values = new[] { 11.4, 28.2, 34.3 },
            }
            )
            .WithGAndGAdminCost(new GAndGAdminCost
            {
                Currency = Currency.USD,
                StartYear = 2030,
                Values = new[] { 31.4, 282.2, 34.3 },
            }
            );
    }
}
