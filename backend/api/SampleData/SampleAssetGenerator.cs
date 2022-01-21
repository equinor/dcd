
using api.Models;

namespace api.SampleData;

public static class SampleAssetGenerator
{
    public static ProjectsBuilder initializeAssets()
    {
        var projectBuilder = new ProjectsBuilder() { }
        .WithProject(new ProjectBuilder()
        {
            ProjectName = "P1",
            CreateDate = DateTimeOffset.UtcNow,
            ProjectCategory = ProjectCategory.OffshoreWind,
            ProjectPhase = ProjectPhase.DG3
        }
            .WithDrainageStrategy(new DrainageStrategyBuilder()
            {
                Name = "DrainStrat 1",
                NGLYield = 0.3
            }
                .WithProductionProfileGas(new ProductionProfileGasBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 2.3)
                    .WithYearValue(2031, 3.3)
                    .WithYearValue(2032, 4.4)
                )
                .WithProductionProfileOil(new ProductionProfileOilBuilder()
                {
                    Unit = VolumeUnit.BBL
                }
                    .WithYearValue(2030, 10.3)
                    .WithYearValue(2031, 13.3)
                    .WithYearValue(2032, 24.4)
                )
                .WithProductionProfileWater(new ProductionProfileWaterBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 12.34)
                    .WithYearValue(2031, 13.45)
                    .WithYearValue(2032, 14.56)
                )
                .WithProductionProfileWaterInjection(new ProductionProfileWaterInjectionBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 7.89)
                    .WithYearValue(2031, 8.91)
                    .WithYearValue(2032, 9.01)
                )
                .WithFuelFlaringAndLosses(new FuelFlaringAndLossesBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 7.89)
                    .WithYearValue(2031, 8.91)
                    .WithYearValue(2032, 9.01)
                )
                .WithNetSalesGas(new NetSalesGasBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 11.23)
                    .WithYearValue(2031, 11.45)
                    .WithYearValue(2032, 11.56)
                    )
                .WithCo2Emissions(new Co2EmissionsBuilder()
                {
                    Unit = MassUnit.TON
                }
                    .WithYearValue(2030, 21.23)
                    .WithYearValue(2031, 22.45)
                    .WithYearValue(2032, 23.56)
                )
            )
            .WithDrainageStrategy(new DrainageStrategyBuilder()
            {
                Name = "DrainStrat 2",
                NGLYield = 0.3
            }
                .WithProductionProfileGas(new ProductionProfileGasBuilder()
                {

                }
                    .WithYearValue(2035, 5.3)
                    .WithYearValue(2036, 6.3)
                    .WithYearValue(2037, 7.4)
                )
            )
            .WithWellProject(new WellProjectBuilder()
            {
                WellType = WellType.Oil,
                RigMobDemob = 100.0,
                AnnualWellInterventionCost = 200.0,
                PluggingAndAbandonment = 300.0
            }
                .WithWellProjectCostProfile(new WellProjectCostProfileBuilder()
                {
                    Currency = Currency.USD
                }
                    .WithYearValue(2035, 55.3)
                    .WithYearValue(2036, 66.3)
                    .WithYearValue(2037, 75.4)
                )
                .WithDrillingSchedule(new DrillingScheduleBuilder()
                    .WithYearValue(2035, 10)
                    .WithYearValue(2036, 12)
                    .WithYearValue(2037, 20)
                    )
            )
        )
        .WithProject(new ProjectBuilder()
        {
            ProjectName = "P2",
            CreateDate = DateTimeOffset.UtcNow
        }
            .WithDrainageStrategy(new DrainageStrategyBuilder()
            {
                Name = "Drainage Strategy 1",
                NGLYield = 0.56
            }
                    .WithProductionProfileGas(new ProductionProfileGasBuilder()
                    {

                    }
                        .WithYearValue(2040, 10.45)
                        .WithYearValue(2041, 13.23)
                        .WithYearValue(2042, 34.21)
                    ))
        );
        return projectBuilder;
    }
}
