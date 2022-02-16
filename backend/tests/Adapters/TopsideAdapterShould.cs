using System;
using System.Collections.Generic;

using api.Adapters;
using api.Dtos;
using api.Models;

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
        // Arrange
        var testProjectId = new Guid();
        var topsideAdapter = new TopsideAdapter();
        var topsideDto = CreateTestTopsideDto(testProjectId);

        // Act
        var topside = topsideAdapter.Convert(topsideDto);

        // Assert
        Assert.Equal(topsideDto.Name, topside.Name);
        Assert.Equal(topsideDto.ProjectId, topside.ProjectId);
        //    TestHelper.CompareCosts(topsideDto.CostProfile, topside.CostProfile);
        Assert.Equal(topsideDto.Name, topside.CostProfile.Topside.Name);
        Assert.Equal(topsideDto.DryWeight, topside.DryWeight);
        Assert.Equal(topsideDto.OilCapacity, topside.OilCapacity);
        Assert.Equal(topsideDto.GasCapacity, topside.GasCapacity);
        Assert.Equal(topsideDto.FacilitiesAvailability,
                topside.FacilitiesAvailability);
        Assert.Equal(topsideDto.ArtificialLift, topside.ArtificialLift);
        Assert.Equal(topsideDto.Maturity, topside.Maturity);
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
                StartYear = 2010,
                Values = new double[] { 3.4564, 18.9, 62.3 }
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
