using api.Adapters;
using api.Exceptions;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;
namespace tests;

[Collection("Database collection")]
public class DrainageStrategyServiceShould : IDisposable
{
    private readonly DatabaseFixture fixture;

    public DrainageStrategyServiceShould()
    {
        fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    private static DrainageStrategy CreateTestDrainageStrategy(Project project)
    {
        return new DrainageStrategyBuilder
        {
            Name = "DrainStrat Test",
            Description = "Some description of the strategy",
            Project = project,
            ProjectId = project.Id,
            NGLYield = 0.5,
            WaterInjectorCount = 20,
            GasInjectorCount = 22,
            ProducerCount = 24,
            ArtificialLift = ArtificialLift.ElectricalSubmergedPumps,
        }
            .WithProductionProfileGas(new ProductionProfileGas
            {
                StartYear = 2030,
                Values = [2.3, 3.3, 4.4]
            }
            )
            .WithProductionProfileOil(new ProductionProfileOil
            {
                StartYear = 2030,
                Values = [10.3, 13.3, 24.4]
            }
            )
            .WithProductionProfileWater(new ProductionProfileWater
            {
                StartYear = 2030,
                Values = [12.34, 13.45, 14.56]
            }
            )
            .WithProductionProfileWaterInjection(new ProductionProfileWaterInjection
            {
                StartYear = 2030,
                Values = [7.89, 8.91, 9.01]
            }
            )
            .WithProductionProfileNGL(new ProductionProfileNGL
            {
                StartYear = 2030,
                Values = [2.34, 3.45, 4.56]
            }
            )
            .WithFuelFlaringAndLosses(new FuelFlaringAndLosses
            {
                StartYear = 2030,
                Values = [8.45, 4.78, 8, 74]
            }
            )
            .WithNetSalesGas(new NetSalesGas
            {
                StartYear = 2030,
                Values = [3.4, 8.9, 2.3]
            }
            )
            .WithCo2Emissions(new Co2Emissions
            {
                StartYear = 2030,
                Values = [33.4, 18.9, 62.3]
            }
            );
    }

    private static DrainageStrategy CreateUpdatedDrainageStrategy(Project project, DrainageStrategy oldDrainage)
    {
        return new DrainageStrategyBuilder
        {
            Id = oldDrainage.Id,
            Name = "Updated strategy",
            Description = "Updated description",
            Project = project,
            ProjectId = project.Id,
            NGLYield = 1.5,
            WaterInjectorCount = 23,
            GasInjectorCount = 23,
            ProducerCount = 21,
            ArtificialLift = ArtificialLift.GasLift,
        }
            .WithProductionProfileGas(new ProductionProfileGas
            {
                StartYear = 2130,
                Values = [2.3, 23.3, 4.4]
            }
            )
            .WithProductionProfileOil(new ProductionProfileOil
            {
                StartYear = 2230,
                Values = [10.23, 13.3, 24.4]
            }
            )
            .WithProductionProfileWater(new ProductionProfileWater
            {
                StartYear = 2230,
                Values = [12.34, 13.425, 14.56]
            }
            )
            .WithProductionProfileWaterInjection(new ProductionProfileWaterInjection
            {
                StartYear = 20230,
                Values = [7.89, 28.91, 9.01]
            }
            )
            .WithProductionProfileNGL(new ProductionProfileNGL
            {
                StartYear = 2030,
                Values = [2.34, 3.45, 4.56]
            }
            )
            .WithFuelFlaringAndLosses(new FuelFlaringAndLosses
            {
                StartYear = 20230,
                Values = [8.425, 4.78, 8, 74]
            }
            )
            .WithNetSalesGas(new NetSalesGas
            {
                StartYear = 1030,
                Values = [3.4, 8.9, 2.3]
            }
            )
            .WithCo2Emissions(new Co2Emissions
            {
                StartYear = 2034,
                Values = [33.4, 181.9, 62.3]
            }
            );
    }
}
