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

    public ExplorationServiceShould()
    {
        fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public async Task CreateNewExploration()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var explorationService = new ExplorationService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var testExploration = CreateTestExploration(project);
        await explorationService.CreateExploration(ExplorationDtoAdapter.Convert(testExploration), caseId);

        var explorations = fixture.context.Projects.FirstOrDefault(o =>
                o.Name == project.Name).Explorations;
        var retrievedExploration = explorations.FirstOrDefault(o => o.Name ==
                testExploration.Name);
        Assert.NotNull(retrievedExploration);
        TestHelper.CompareExplorations(testExploration, retrievedExploration);
    }

    [Fact]
    public async Task DeleteExploration()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var explorationService = new ExplorationService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var sourceCaseId = project.Cases.FirstOrDefault().Id;
        var ExplorationToDelete = CreateTestExploration(project);
        await explorationService.CreateExploration(ExplorationDtoAdapter.Convert(ExplorationToDelete), sourceCaseId);

        var deletedExplorationId = project.Explorations.First().Id;
        // Act
        var projectResult = await explorationService.DeleteExploration(deletedExplorationId);

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
    public async Task UpdateExploration()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var explorationService = new ExplorationService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var sourceCaseId = project.Cases.FirstOrDefault().Id;
        var oldExploration = CreateTestExploration(project);
        var oldId = (await explorationService.CreateExploration(ExplorationDtoAdapter.Convert(oldExploration), sourceCaseId)).Explorations.Last().Id;

        var updatedExploration = CreateUpdatedTestExploration(project, oldId);

        // Act
        var projectResult = await explorationService.UpdateExploration(ExplorationDtoAdapter.Convert(updatedExploration));


        // Assert
        var actualExploration = projectResult.Explorations.FirstOrDefault(o => o.Name == updatedExploration.Name);
        Assert.NotNull(actualExploration);
        TestHelper.CompareExplorations(updatedExploration, actualExploration);
    }

    private static Exploration CreateTestExploration(Project project)
    {
        return new ExplorationBuilder
        {
            Name = "Test-exploration-23",
            Project = project,
            ProjectId = project.Id,
            RigMobDemob = 32.7
        }
                .WithGAndGAdminCost(new GAndGAdminCost
                {
                    Currency = Currency.NOK,
                    StartYear = 2010,
                    Values = [314.4, 281.2, 34.3]
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
            RigMobDemob = 32.7
        }
                .WithGAndGAdminCost(new GAndGAdminCost
                {
                    Currency = Currency.USD,
                    StartYear = 2030,
                    Values = [31.4, 282.2, 34.3]
                }

                );
    }

}
