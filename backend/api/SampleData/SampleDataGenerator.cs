
using api.Models;

namespace api.SampleData;

public static class SampleDataGenerator
{

    public static ProjectsBuilder initialize()
    {
        return new ProjectsBuilder() { }
        .WithProject(new ProjectBuilder()
        {
            ProjectName = "P1",
            CreateDate = DateTimeOffset.UtcNow,
            ProjectCategory = ProjectCategory.OffshoreWind,
            ProjectPhase = ProjectPhase.DG3
        }
            .WithCase(new CaseBuilder()
            {
                Name = "Case 1 in P1",
                Description = "Description Case 1 in P1",
                CreateTime = DateTimeOffset.UtcNow,
                ModifyTime = DateTimeOffset.UtcNow,
                ReferenceCase = true,
                ProducerCount = 2,
                GasInjectorCount = 3,
                WaterInjectorCount = 4,
                RiserCount = 5,
                TemplateCount = 6,
                FacilitiesAvailability = 0.8,
                DG4Date = DateTimeOffset.Now.AddYears(5),
                ArtificialLift = ArtificialLift.GasLift
            }
                .WithCessationCost(new CessationCostBuilder()
                {
                    Currency = api.Models.Currency.NOK
                }
                    .WithYearValue(2030, 1000)
                    .WithYearValue(2031, 1100)
                    .WithYearValue(2032, 1200)
                )
                .WithDrainageStrategy(new DrainageStrategyBuilder()
                {
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
                    ))
            .WithCase(new CaseBuilder()
            {
                Name = "Case 2 in P1",
                Description = "Description 2 in Case 2 in P1"
            }
                .WithDrainageStrategy(new DrainageStrategyBuilder()
                {
                    NGLYield = 0.3
                }
                    .WithProductionProfileGas(new ProductionProfileGasBuilder()
                    {

                    }
                        .WithYearValue(2035, 5.3)
                        .WithYearValue(2036, 6.3)
                        .WithYearValue(2037, 7.4)
                    )))
            .WithCase(new CaseBuilder()
            {
                Name = "Case 3 in P1",
                Description = "Description 2 in Case 1 in P1"
            })
            .WithSurf(new SurfBuilder()
            {
                Maturity = Maturity.B
            }))
        .WithProject(new ProjectBuilder()
        {
            ProjectName = "P2",
            CreateDate = DateTimeOffset.UtcNow
        }
            .WithCase(new CaseBuilder()
            {
                Name = "Case 1 in P2",
                Description = "Description Case 1 P2"
            }
                .WithDrainageStrategy(new DrainageStrategyBuilder()
                {
                    NGLYield = 0.56
                }
                    .WithProductionProfileGas(new ProductionProfileGasBuilder()
                    {

                    }
                        .WithYearValue(2040, 10.45)
                        .WithYearValue(2041, 13.23)
                        .WithYearValue(2042, 34.21)
                    ))));
    }
}
