using System;
using System.Collections.Generic;
using System.Linq;

using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Xunit;


namespace tests;

[Collection("Database collection")]
public class ExplorationAdapterShould : IDisposable
{

    private readonly DatabaseFixture fixture;

    public ExplorationAdapterShould(DatabaseFixture fixture)
    {
        this.fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public void ConvertExplorationDtoToDataModel()
    {
        var testProjectId = fixture.context.Projects.FirstOrDefault().Id;
        ProjectService projectService = new ProjectService(fixture.context);

        var explorationDto = CreateTestExplorationDto(testProjectId);

        var exploration = ExplorationAdapter.Convert(explorationDto);

        Assert.Equal(explorationDto.ProjectId, exploration.ProjectId);
        Assert.Equal(explorationDto.Name, exploration.Name);
        Assert.Equal(explorationDto.WellType, exploration.WellType);
        Assert.Equal(explorationDto.RigMobDemob, exploration.RigMobDemob);

        TestHelper.CompareYearValues(explorationDto.DrillingSchedule,
                exploration.DrillingSchedule);
        Assert.Equal(exploration.Name,
                exploration.DrillingSchedule.Exploration.Name);

        // TestHelper.CompareCosts(explorationDto.CostProfile,
        //         exploration.CostProfile);
        Assert.Equal(exploration.Name,
                exploration.CostProfile.Exploration.Name);

        // TestHelper.CompareCosts(explorationDto.GAndGAdminCost,
        //         exploration.GAndGAdminCost);
        Assert.Equal(exploration.Name,
                exploration.GAndGAdminCost.Exploration.Name);
    }

    private ExplorationDto CreateTestExplorationDto(Guid projectId)
    {
        return new ExplorationDto
        {
            ProjectId = projectId,
            Name = "test exploration",
            WellType = WellType.Gas,
            CostProfile = new ExplorationCostProfileDto
            {
                Currency = Currency.USD,
                EPAVersion = "GT 1",
                StartYear = 2010,
                Values = new double[] { 3.4564, 18.9, 62.3 }
            },
            DrillingSchedule = new ExplorationDrillingScheduleDto
            {
                StartYear = 2010,
                Values = new int[] { 3, 18, 62 }
            },
            GAndGAdminCost = new GAndGAdminCostDto
            {
                Currency = Currency.USD,
                EPAVersion = "GT 1",
                StartYear = 2010,
                Values = new double[] { 3.4564, 18.9, 62.3 }
            },
            RigMobDemob = 2.3e6
        };
    }
}
