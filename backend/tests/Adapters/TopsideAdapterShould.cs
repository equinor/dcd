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
public class TopsideAdapterShould : IDisposable
{

    private readonly DatabaseFixture fixture;

    public TopsideAdapterShould(DatabaseFixture fixture)
    {
        this.fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public void ConvertTopsideDtoToDataModel()
    {
        var testProjectId = fixture.context.Projects.FirstOrDefault().Id;
        ProjectService projectService = new ProjectService(fixture.context);
        var topsideAdapter = new TopsideAdapter();
        var topsideDto = CreateTestTopsideDto(testProjectId);

        var topside = topsideAdapter.Convert(topsideDto);

        Assert.Equal(topsideDto.Name, topside.Name);
        Assert.Equal(topsideDto.ProjectId, topside.ProjectId);
        TestHelper.CompareCosts(topsideDto.CostProfile, topside.CostProfile);
        Assert.Equal(topsideDto.Name, topside.CostProfile.Topside.Name);
        Assert.Equal(topsideDto.DryWeight, topside.DryWeight);
        Assert.Equal(topsideDto.OilCapacity, topside.OilCapacity);
        Assert.Equal(topsideDto.GasCapacity, topside.GasCapacity);
        Assert.Equal(topsideDto.FacilitiesAvailability,
                topside.FacilitiesAvailability);
        Assert.Equal(topsideDto.ArtificialLift, topside.ArtificialLift);
        Assert.Equal(topsideDto.Maturity, topside.Maturity);
    }

    public void FailConvertingWithEmptyProjectId()
    {
        ProjectService projectService = new ProjectService(fixture.context);
        var topsideAdapter = new TopsideAdapter();
        var topsideDto = CreateTestTopsideDto(System.Guid.Empty);
        Assert.ThrowsAny<Exception>(() =>
        {
            topsideAdapter.Convert(topsideDto);
        });
    }

    private TopsideDto CreateTestTopsideDto(Guid projectId)
    {
        return new TopsideDto
        {
            Name = "test topside",
            ProjectId = projectId,
            CostProfile = new TopsideCostProfileDto
            {
                Currency = Currency.USD,
                EPAVersion = "GT 1",
                YearValues = new List<YearValue<double>> {
                    new YearValue<double> (2023, 1.2e6),
                    new YearValue<double> (2024, 1.3e6),
                    new YearValue<double> (2025, 1.1e6),
                },
            },
            DryWeight = 0.6e12,
            OilCapacity = 10,
            GasCapacity = 34,
            FacilitiesAvailability = 0.000000001,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            Maturity = Maturity.A
        };
    }
}
