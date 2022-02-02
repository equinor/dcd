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
    public void CreateNewExploration()
    {
        var project = fixture.context.Projects.FirstOrDefault();
        var testExploration = CreateTestExploration(project);
        ProjectService projectService = new ProjectService(fixture.context);
        ExplorationService explorationService = new
            ExplorationService(fixture.context, projectService);

        explorationService.CreateExploration(testExploration);

        var explorations = fixture.context.Projects.FirstOrDefault(o =>
                o.ProjectName == project.ProjectName).Explorations;
        var retrievedExploration = explorations.FirstOrDefault(o => o.Name ==
                testExploration.Name);
        Assert.NotNull(retrievedExploration);
        compareExplorations(testExploration, retrievedExploration);
    }

    void compareExplorations(Exploration expected, Exploration actual)
    {
        if (expected == null || actual == null)
        {
            Assert.Equal(expected, null);
            Assert.Equal(actual, null);
        }
        else
        {
            Assert.Equal(expected.Project.Id, actual.Project.Id);
            Assert.Equal(expected.ProjectId, actual.ProjectId);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.WellType, actual.WellType);

            TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
            Assert.Equal(expected.CostProfile.Exploration.Name,
                    actual.CostProfile.Exploration.Name);

            TestHelper.CompareYearValues(expected.DrillingSchedule,
                    actual.DrillingSchedule);
            Assert.Equal(expected.DrillingSchedule.Exploration.Name,
                    actual.DrillingSchedule.Exploration.Name);

            TestHelper.CompareCosts(expected.GAndGAdminCost,
                    actual.GAndGAdminCost);
            Assert.Equal(expected.GAndGAdminCost.Exploration.Name,
                    actual.GAndGAdminCost.Exploration.Name);

            Assert.Equal(expected.RigMobDemob, actual.RigMobDemob);
        }
    }

    private static Exploration CreateTestExploration(Project project)
    {
        return new ExplorationBuilder
        {
            Name = "Test-exploration-23",
            Project = project,
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
