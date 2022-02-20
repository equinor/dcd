using api.Models;
using api.SampleData.Builders;

namespace api.SampleData.Generators;

public static class SampleAssetGenerator
{
    public static ProjectsBuilder initializeAssets()
    {
        var projectBuilder = new ProjectsBuilder()
        .WithProject(spreadSheetProjectAssembledWithCase2Assets()
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
            CommonLibraryId = new Guid(),
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
            CommonLibraryId = new Guid(),
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

    public static ProjectBuilder spreadSheetProjectAssembledWithCase2Assets()
    {
        return spreadSheetProject()
            .WithDrainageStrategy(case2DrainageStrategy())
            .WithSubstructure(case2Substructure())
            .WithWellProject(case2WellProject())
            .WithExploration(case2Exploration());
    }

    public static ProjectBuilder spreadSheetProject()
    {
        return new ProjectBuilder()
        {
            Name = "Skarven",
            CommonLibraryId = new Guid(),
            CommonLibraryName = "Skarven",
            Description = "Project from example spread sheet",
            CreateDate = DateTimeOffset.UtcNow,
            ProjectCategory = ProjectCategory.OffshoreWind,
            ProjectPhase = ProjectPhase.BusinessPlanning
        };
    }
    public static CaseBuilder case2Case()
    {
        return new CaseBuilder
        {
            Name = "Case 2",
            CreateTime = DateTimeOffset.UtcNow,
            Description = "case 2 from example spreadsheet",
            ModifyTime = DateTimeOffset.UtcNow
        };
    }
    public static DrainageStrategyBuilder case2DrainageStrategy()
    {
        return new DrainageStrategyBuilder
        {
            Name = "SkarvenDrainStratCase2",
            Description = "Skarvens drainage strategy"
        }
            .WithProductionProfileGas(new ProductionProfileGas
            {
                StartYear = 2030,
                Values = new double[] { 0.0460776e6, 0.0459552635016671e6,
                0.0411623281639219e6,  0.0347990402399031e6,
                0.0294194535546169e6, 0.02487149764722e6,
                0.0210266106427589e6,  0.017776105057815e6,
                0.0150280954165715e6,  0.0127049008269832e6,
                0.010740849092926e6, 0.00908042028883707e6,
                0.00767667731932247e6, 0.00648993909868211e6,
                0.00548665884374195e6, 0.00463847577147806e6,
                0.00249628453355253e6,
                }
            }
            )
            .WithProductionProfileOil(new ProductionProfileOil
            {
                StartYear = 2030,
                Values = new double[] { 0.438e6, 0.436837105529155e6,
                0.391276883687471e6, 0.330789355892615e6, 0.279652600329056e6,
                0.236421080296768e6, 0.199872724741057e6, 0.168974382678855e6,
                0.142852618028251e6, 0.120769019267905e6, 0.102099325978384e6,
                0.0863157822132823e6,  0.0729722178642801e6,  0.0616914362992658e6,
                0.0521545517465929e6,  0.0440919750140482e6,  0.0237289404330128e6
                }
            }
            )
            .WithNetSalesGas(new NetSalesGas
            {
                StartYear = 2030,
                Values = new double[] { 0.045534094613436e6,
                0.0454132011274522e6,  0.0406768005522276e6,
                0.0343885703843194e6, 0.0290724382700382e6,
                0.0245781274859447e6,  0.0207785926004667e6,
                0.0175664281464513e6,  0.0148508324773406e6,
                0.0125550409810879e6, 0.0106141560937621e6,
                0.00897331277153478e6, 0.00758612756252165e6,
                0.00641338743673343e6, 0.0054219413099335e6,
                0.00458376292690185e6, 0.00246683976884294e6
                }
            }
            )
            .WithCo2Emissions(new Co2Emissions
            {
                StartYear = 2029,
                Values = new double[] { 0.009e6, 0.00202727509188372e6,
                0.00202189265582155e6, 0.00181101799161985e6,
                0.0015310525613273e6, 0.00129436701147856e6,
                0.0010942707015568e6, 0.000925107297749942e6,
                0.000782094879386459e6, 0.00066119076333117e6,
                0.000558977225189606e6, 0.000472564886881495e6,
                0.000399511039537549e6, 0.000337750592867049e6,
                0.000285537699068565e6, 0.000241396401105546e6,
                0.000204078910269268e6, 0.000109828972366755e6
                }
            }
            );
    }
    public static SubstructureBuilder case2Substructure()
    {
        return new SubstructureBuilder
        {
            Name = "SkarvenSubCase2",
        }.WithCostProfile(new SubstructureCostProfile
        {
            Currency = Currency.NOK,
            StartYear = 2027,
            Values = new double[] { 349.95166651869e6, 427.288025880945e6,
            424.163092995196e6,  39.8227522796791e6
            }
        });
    }
    public static WellProjectBuilder case2WellProject()
    {
        return new WellProjectBuilder
        {
            Name = "SkarvenWellCase2",
            AnnualWellInterventionCost = 85e6
        }.WithWellProjectCostProfile(new WellProjectCostProfile
        {
            Currency = Currency.NOK,
            StartYear = 2029,
            Values = new double[] { 764e6 }
        });
    }
    public static ExplorationBuilder case2Exploration()
    {
        return new ExplorationBuilder
        {
            Name = "SkarvenExplCase2"
        }.WithExplorationCostProfile(new ExplorationCostProfile
        {
            Currency = Currency.NOK,
            StartYear = 2023,
            Values = new double[] { 280e6 }

        }).WithGAndGAdminCost(new GAndGAdminCost
        {
            Currency = Currency.NOK,
            StartYear = 2022,
            Values = new double[] { 9e6, 9e6, 9e6 }
        });
    }
}
