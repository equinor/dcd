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
                    StartYear = 8,
                    Values = new[] { 0.1382328e9,   0.129461160089552e9,
                    0.101858662653302e9,   0.0793845923999293e9,
                    0.061869195474839e9,   0.0482183914155514e9,
                    0.0375794974034997e9,  0.0292879663473024e9,
                    0.0228258766622245e9,  0.0177895808544964e9,
                    0.0138644921140046e9,  0.0108054339869784e9,
                    0.00735035059831965e9 }
                }
                )
                .WithProductionProfileOil(new ProductionProfileOil
                {
                    StartYear = 8,
                    Values = new[] { 1.314e6,   1.23061939248623e6,
                    0.968238238149264e6,   0.754606391634305e6,
                    0.588110223144857e6,   0.458349728284721e6,
                    0.357219557067497e6,   0.278402721932525e6,
                    0.216976013899468e6,   0.169102479605479e6,
                    0.131791750133118e6,   0.102713250826803e6,
                    0.069870252835732 }
                }
                )
                .WithNetSalesGas(new NetSalesGas
                {
                    StartYear = 8,
                    Values = new[] { 0.136602283840308e9,
                    0.127934109247939e9,   0.100657195306473e9,
                    0.0784482165127401e9,  0.0611394213328925e9,
                    0.0476496344606362e9,  0.0371362308431066e9,
                    0.0289425020116759e9,  0.022556635492568e9,
                    0.017579744990234e9,   0.013700954383178e9,
                    0.0106779791808235e9,  0.00726365000750502e9 }
                }
                )
                .WithCo2Emissions(new Co2Emissions
                {
                    StartYear = 7,
                    Values = new[] { 0.0135e6,  0.00608182527565116e6,
                    0.00569589963921555e6, 0.00448147320367438e6,
                    0.00349268205921571e6, 0.00272205754946061e6,
                    0.00212146344183398e6, 0.00165338427026637e6,
                    0.00128858197188685e6, 0.00100426956281874e6,
                    0.000782687773698545e6, 0.000609995735983182e6,
                    0.000475406426957946e6, 0.000323393203738581e6 }
                }
                )
                )
            .WithSubstructure(new SubstructureBuilder
            {
                Name = "SkarvenSub"
            }.WithCostProfile(new SubstructureCostProfile
            {
                Currency = Currency.NOK,
                StartYear = 5,
                Values = new[] { 391.073152662903e6, 477.737937405167e6,
                474.497638477598e6, 44.5750914242851e6 }
            }))
            .WithWellProject(new WellProjectBuilder
            {
                Name = "SkarvenWell"
            }.WithWellProjectCostProfile(new WellProjectCostProfile
            {
                Currency = Currency.NOK,
                StartYear = 7,
                Values = new[] { 1146e6 }
            }))
            .WithExploration(new ExplorationBuilder
            {
                Name = "SkarvenExpl"
            }.WithExplorationCostProfile(new ExplorationCostProfile
            {
                Currency = Currency.NOK,
                StartYear = 1,
                Values = new[] { 280e6 }

            }).WithGAndGAdminCost(new GAndGAdminCost
            {
                Currency = Currency.NOK,
                StartYear = 0,
                Values = new[] { 8.5e6, 8.5e6, 8.5e6 }
            }))
            )

        .WithProject(new ProjectBuilder()
        {
            Name = "P1",
            CommonLibraryId = new Guid(),
            CommonLibraryName = "P1 from common lib",
            CreateDate = DateTimeOffset.UtcNow.Date,
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
                    StartYear = 9,
                    Values = new[] { 2.3, 3.3, 4.4 }
                }
                )
                .WithProductionProfileOil(new ProductionProfileOil
                {
                    StartYear = 11,
                    Values = new[] { 10.3, 13.3, 24.4, 1.2 }
                }
                )
                .WithProductionProfileWater(new ProductionProfileWater
                {
                    StartYear = 12,
                    Values = new[] { 12.34, 13.45, 14.56 }
                }
                )
                .WithProductionProfileWaterInjection(new ProductionProfileWaterInjection
                {
                    StartYear = 8,
                    Values = new[] { 7.89, 8.91, 9.01 }
                }
                )
                .WithFuelFlaringAndLosses(new FuelFlaringAndLosses
                {
                    StartYear = 12,
                    Values = new[] { 8.45, 4.78, 8, 74 }
                }
                )
                .WithNetSalesGas(new NetSalesGas
                {
                    StartYear = 13,
                    Values = new[] { 3.4, 8.9, 2.3 }
                }
                )
                .WithCo2Emissions(new Co2Emissions
                {
                    StartYear = 8,
                    Values = new[] { 33.4, 18.9, 62.3 }
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
                    StartYear = 9,
                    Values = new[] { 12.34, 13.45, 14.56 }
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
                    StartYear = 8,
                    Values = new[] { 33.4, 18.9, 62.3 }
                }
                )
                .WithDrillingSchedule(new DrillingSchedule
                {
                    StartYear = 9,
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
                    StartYear = 10,
                    Values = new[] { 33.4, 18.9, 62.3 }
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
                    StartYear = 11,
                    Values = new[] { 23.4, 28.9, 32.3 }
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
                    StartYear = 12,
                    Values = new[] { 123.4, 218.9, 312.3 }
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
                    StartYear = 13,
                    Values = new[] { 13.4, 18.9, 34.3 }
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
                    StartYear = 14,
                    Values = new[] { 11.4, 28.2, 34.3 }
                }
                )
                .WithExplorationDrillingSchedule(new ExplorationDrillingSchedule
                {
                    StartYear = 15,
                    Values = new int[] { 13, 5, 5 }
                }
                )
                .WithGAndGAdminCost(new GAndGAdminCost
                {
                    Currency = Currency.NOK,
                    StartYear = 16,
                    Values = new[] { 31.4, 28.2, 34.3 }
                }

                )
            )
        )
        .WithProject(new ProjectBuilder
        {
            Name = "P2",
            CommonLibraryId = new Guid(),
            CommonLibraryName = "P2 from common lib",
            CreateDate = DateTimeOffset.UtcNow.Date
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
                        StartYear = 8,
                        Values = new[] { 12.34, 13.45, 14.56 }
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
        return new ProjectBuilder
        {
            Name = "Skarven",
            CommonLibraryId = new Guid(),
            CommonLibraryName = "Skarven",
            Description = "Project from example spread sheet",
            CreateDate = DateTimeOffset.UtcNow.Date,
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
            ModifyTime = DateTimeOffset.UtcNow,
            DG4Date = DateTimeOffset.Now
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
                StartYear = 8,
                Values = new[] { 0.0460776e6, 0.0459552635016671e6,
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
                StartYear = 8,
                Values = new[] { 0.438e6, 0.436837105529155e6,
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
                StartYear = 8,
                Values = new[] { 0.045534094613436e6,
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
                StartYear = 7,
                Values = new[] { 0.009e6, 0.00202727509188372e6,
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
            StartYear = 5,
            Values = new[] { 349.95166651869e6, 427.288025880945e6,
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
            StartYear = 7,
            Values = new[] { 764e6 }
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
            StartYear = 1,
            Values = new[] { 280e6 }

        }).WithGAndGAdminCost(new GAndGAdminCost
        {
            Currency = Currency.NOK,
            StartYear = 0,
            Values = new[] { 9e6, 9e6, 9e6 }
        });
    }
}
