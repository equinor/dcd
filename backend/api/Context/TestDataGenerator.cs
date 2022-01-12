
using api.Models;

public static class TestDataGenerator
{

    public static ProjectsBuilder initialize()
    {
        return new ProjectsBuilder() { }
        .WithProject(new ProjectBuilder()
        {
            ProjectName = "P1",
            CreateDate = DateTimeOffset.UtcNow
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

                    }
                        .WithYearValue(2030, 2.3)
                        .WithYearValue(2031, 3.3)
                        .WithYearValue(2032, 4.4)
                    )
                    .WithProductionProfileOil(new ProductionProfileOilBuilder()
                    {

                    }
                        .WithYearValue(2030, 10.3)
                        .WithYearValue(2031, 13.3)
                        .WithYearValue(2032, 24.4)
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
