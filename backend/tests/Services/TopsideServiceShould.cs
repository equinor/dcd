using System;
using System.Linq;

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
    public void CreateNewTopside()
    {
        var project = fixture.context.Projects.FirstOrDefault();
        var testTopside = CreateTestTopside(project);
        ProjectService projectService = new ProjectService(fixture.context);
        TopsideService topsideService = new
            TopsideService(fixture.context, projectService);

        topsideService.CreateTopside(testTopside);

        var topsides = fixture.context.Projects.FirstOrDefault(o =>
                o.ProjectName == project.ProjectName).Topsides;
        var retrievedTopside = topsides.FirstOrDefault(o => o.Name ==
                testTopside.Name);
        Assert.NotNull(retrievedTopside);
        compareTopsides(testTopside, retrievedTopside);
    }

    void compareTopsides(Topside expected, Topside actual)
    {
        if (expected == null || actual == null)
        {
            Assert.Equal(expected, null);
            Assert.Equal(actual, null);
        }
        else
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Project.Id, actual.Project.Id);
            Assert.Equal(expected.ProjectId, actual.ProjectId);
            TestHelper.CompareCosts(expected.CostProfile, actual.CostProfile);
            Assert.Equal(expected.CostProfile.Topside.Name,
                    actual.CostProfile.Topside.Name);
            Assert.Equal(expected.DryWeight, actual.DryWeight);
            Assert.Equal(expected.OilCapacity, actual.OilCapacity);
            Assert.Equal(expected.GasCapacity, actual.GasCapacity);
            Assert.Equal(expected.FacilitiesAvailability,
                    actual.FacilitiesAvailability);
            Assert.Equal(expected.ArtificialLift, actual.ArtificialLift);
            Assert.Equal(expected.Maturity, actual.Maturity);
        }
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
                .WithCostProfile(new TopsideCostProfileBuilder()
                    .WithYearValue(2023, 44)
                    .WithYearValue(2024, 45.7)
                    .WithYearValue(2025, 42.3)
                );
    }

}
