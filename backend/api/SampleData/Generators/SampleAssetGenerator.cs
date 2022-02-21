using api.Models;
using api.SampleData.Builders;

namespace api.SampleData.Generators;

public static class SampleAssetGenerator
{
    public static ProjectsBuilder initializeAssets()
    {
        var projectBuilder = new ProjectsBuilder()
        .WithProject(new ProjectBuilder()
        {
            Name = "Skarven",
            CommonLibraryName = "P1 from common lib",
            CreateDate = DateTimeOffset.UtcNow,
            ProjectCategory = ProjectCategory.OffshoreWind,
            ProjectPhase = ProjectPhase.BusinessPlanning
        }
            .WithDrainageStrategy(new DrainageStrategyBuilder
            {
                Name = "SkarvenDrainStrat",
                Description = "Skarvens drainage strategy"
            }
                .WithProductionProfileGas(new ProductionProfileGas
                {
                    StartYear = 2030,
                    Values = new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }
                }
                )
                .WithProductionProfileOil(new ProductionProfileOil
                {
                    StartYear = 2030,
                    Values = new double[] { 1.3, 1.2, 1.0, 0.8, 0.6, 0.5, 0.4, 0.3, 0.2, 0.2, 0.1, 0.1, 0.1 }
                }
                )
                .WithNetSalesGas(new NetSalesGas
                {
                    StartYear = 2030,
                    Values = new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }
                }
                )
                .WithCo2Emissions(new Co2Emissions
                {
                    StartYear = 2029,
                    Values = new double[] { 0.01, 0.01, 0.01, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00 }
                }
                    )
                )
            .WithSubstructure(new SubstructureBuilder
            {
                Name = "SkarvenSub"
            }.WithCostProfile(new SubstructureCostProfile
            {
                Currency = Currency.NOK,
                StartYear = 2027,
                Values = new double[] { 391000000, 478000000, 474000000, 45000000 }
            }))
            .WithWellProject(new WellProjectBuilder
            {
                Name = "SkarvenWell"
            }.WithWellProjectCostProfile(new WellProjectCostProfile
            {
                Currency = Currency.NOK,
                StartYear = 2029,
                Values = new double[] { 1146000000 }
            }))
            .WithExploration(new ExplorationBuilder
            {
                Name = "SkarvenExpl"
            }.WithExplorationCostProfile(new ExplorationCostProfile
            {
                Currency = Currency.NOK,
                StartYear = 2023,
                Values = new double[] { 280000000 }

            }).WithGAndGAdminCost(new GAndGAdminCost
            {
                Currency = Currency.NOK,
                StartYear = 2022,
                Values = new double[] { 9000000, 9000000, 9000000 }
            }))
            )

        .WithProject(new ProjectBuilder()
        {
            Name = "P1",
            CommonLibraryName = "P1 from common lib",
            CreateDate = DateTimeOffset.UtcNow,
            ProjectCategory = ProjectCategory.OffshoreWind,
            ProjectPhase = ProjectPhase.BusinessPlanning
        }
            .WithDrainageStrategy(new DrainageStrategyBuilder
            {
                Name = "DrainStrat 1",
                Description = "Desc of drain strat 1",
                NGLYield = 0.3,
                ProducerCount = 2,
                GasInjectorCount = 3,
                WaterInjectorCount = 4,
            }
                .WithProductionProfileGas(new ProductionProfileGas
                {
                    StartYear = 2031,
                    Values = new double[] { 2.3, 3.3, 4.4 }
                }
                )
                .WithProductionProfileOil(new ProductionProfileOil
                {
                    StartYear = 2032,
                    Values = new double[] { 10.3, 13.3, 24.4, 1.2 }
                }
                )
                .WithProductionProfileWater(new ProductionProfileWater
                {
                    StartYear = 2033,
                    Values = new double[] { 12.34, 13.45, 14.56 }
                }
                )
                .WithProductionProfileWaterInjection(new ProductionProfileWaterInjection
                {
                    StartYear = 2030,
                    Values = new double[] { 7.89, 8.91, 9.01 }
                }
                )
                .WithFuelFlaringAndLosses(new FuelFlaringAndLosses
                {
                    StartYear = 2034,
                    Values = new double[] { 8.45, 4.78, 8, 74 }
                }
                )
                .WithNetSalesGas(new NetSalesGas
                {
                    StartYear = 2035,
                    Values = new double[] { 3.4, 8.9, 2.3 }
                }
                )
                .WithCo2Emissions(new Co2Emissions
                {
                    StartYear = 2030,
                    Values = new double[] { 33.4, 18.9, 62.3 }
                }
                )
            )
            .WithDrainageStrategy(new DrainageStrategyBuilder
            {
                Name = "DrainStrat 2",
                Description = "Desc of drain strategy 2",
                NGLYield = 0.3
            }
                .WithProductionProfileGas(new ProductionProfileGas
                {
                    StartYear = 2031,
                    Values = new double[] { 12.34, 13.45, 14.56 }
                }
                )
            )
            .WithWellProject(new WellProjectBuilder
            {
                RigMobDemob = 100.0,
                AnnualWellInterventionCost = 200.0,
                PluggingAndAbandonment = 300.0,
                ProducerCount = 2,
                GasInjectorCount = 3,
                WaterInjectorCount = 4,
                ArtificialLift = ArtificialLift.GasLift
            }
                .WithWellProjectCostProfile(new WellProjectCostProfile
                {
                    Currency = Currency.USD,
                    StartYear = 2030,
                    Values = new double[] { 33.4, 18.9, 62.3 }
                }
                )
                .WithDrillingSchedule(new DrillingSchedule
                {
                    StartYear = 2031,
                    Values = new int[] { 33, 3, 62 }
                }
                )
            )
            .WithSurf(new SurfBuilder
            {
                Name = "Surf 1",
                Maturity = Maturity.A,
                ProductionFlowline = ProductionFlowline.Default,
                InfieldPipelineSystemLength = 5.5,
                UmbilicalSystemLength = 1.1,
                RiserCount = 5,
                TemplateCount = 6,
                ArtificialLift = ArtificialLift.GasLift
            }
                .WithCostProfile(new SurfCostProfile
                {
                    Currency = Currency.USD,
                    StartYear = 2032,
                    Values = new double[] { 33.4, 18.9, 62.3 }
                }
                )
            )
            .WithSubstructure(new SubstructureBuilder
            {
                Name = "Substructure 1",
                Maturity = Maturity.B,
                DryWeight = 4.5,
            }
                .WithCostProfile(new SubstructureCostProfile
                {
                    Currency = Currency.USD,
                    StartYear = 2033,
                    Values = new double[] { 23.4, 28.9, 32.3 }
                }
                )
            )
            .WithTopside(new TopsideBuilder
            {
                Name = "Topside 1",
                Maturity = Maturity.C,
                OilCapacity = 50.0,
                GasCapacity = 75.0,
                DryWeight = 45.1,
                FacilitiesAvailability = 0.8,
                ArtificialLift = ArtificialLift.GasLift
            }
                .WithCostProfile(new TopsideCostProfile
                {
                    Currency = Currency.USD,
                    StartYear = 2034,
                    Values = new double[] { 123.4, 218.9, 312.3 }
                }
                )
            )
            .WithTransport(new TransportBuilder
            {
                Name = "Transport 1",
                Maturity = Maturity.D
            }
                .WithCostProfile(new TransportCostProfile
                {
                    Currency = Currency.USD,
                    StartYear = 2035,
                    Values = new double[] { 13.4, 18.9, 34.3 }
                }
                )
            )
            .WithExploration(new ExplorationBuilder
            {
                Name = "Exploration",
                WellType = WellType.Oil,
                RigMobDemob = 122.4
            }
                .WithExplorationCostProfile(new ExplorationCostProfile
                {
                    Currency = Currency.USD,
                    StartYear = 2036,
                    Values = new double[] { 11.4, 28.2, 34.3 }
                }
                )
                .WithExplorationDrillingSchedule(new ExplorationDrillingSchedule
                {
                    StartYear = 2037,
                    Values = new int[] { 13, 5, 5 }
                }
                )
                .WithGAndGAdminCost(new GAndGAdminCost
                {
                    Currency = Currency.NOK,
                    StartYear = 2038,
                    Values = new double[] { 31.4, 28.2, 34.3 }
                }

                )
            )
        )
        .WithProject(new ProjectBuilder
        {
            Name = "P2",
            CommonLibraryName = "P2 from common lib",
            CreateDate = DateTimeOffset.UtcNow
        }
            .WithDrainageStrategy(new DrainageStrategyBuilder
            {
                Name = "Drainage Strategy 1",
                Description = "Desc. of drainage strategy in P2",
                NGLYield = 0.56,
                ArtificialLift = ArtificialLift.GasLift
            }
                    .WithProductionProfileGas(new ProductionProfileGas
                    {
                        StartYear = 2030,
                        Values = new double[] { 12.34, 13.45, 14.56 }
                    }
                    ))
        );
        return projectBuilder;
    }
}
