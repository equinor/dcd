using api.Dtos;
using api.Models;

namespace tests;

public static class TestDataGenerator
{
    public static ProjectDto SpreadSheetProject()
    {
        return new ProjectDto
        {
            Name = "Skarven",
            CommonLibraryName = "The Skarven Project",
            Country = "Norway",
            Description = "Skarven Project from Spreadsheet",
            ProjectCategory = ProjectCategory.OffshoreWind,
            ProjectPhase = ProjectPhase.BusinessPlanning
        };
    }

    public static CaseDto Case2Case()
    {
        return new CaseDto
        {
            Name = "Case 2",
            CreateTime = DateTimeOffset.UtcNow,
            Description = "case 2 from example spreadsheet",
            ModifyTime = DateTimeOffset.UtcNow
        };
    }

    public static DrainageStrategyDto Case2DrainageStrategy()
    {
        return new DrainageStrategyDto
        {
            Name = "SkarvenDrainStratCase2",
            Description = "Skarvens drainage strategy",
            ProductionProfileGas = new ProductionProfileGasDto
            {
                StartYear = 2030,
                Values = new[]
                {
                    22.0460776e9, 23.0459552635016671e9,
                    33.0411623281639219e9, 55.0347990402399031e9,
                    44.0294194535546169e9, 32.02487149764722e9,
                    55.0210266106427589e9, 52.017776105057815e9,
                    66.0150280954165715e9, 25.0127049008269832e9,
                    77.010740849092926e9, 64.00908042028883707e9,
                    88.00767667731932247e9, 75.00648993909868211e9,
                    99.00548665884374195e9, 92.00463847577147806e9,
                    100.00249628453355253e9
                }
            },
            ProductionProfileOil = new ProductionProfileOilDto
            {
                StartYear = 2030,
                Values = new[]
                {
                    25.438e6, 65.436837105529155e6,
                    1.391276883687471e6, 5.330789355892615e6, 25.279652600329056e6,
                    2.236421080296768e6, 7.199872724741057e6, 654.168974382678855e6,
                    3.142852618028251e6, 8.120769019267905e6, 754.102099325978384e6,
                    5.0863157822132823e6, 9.0729722178642801e6, 751.0616914362992658e6,
                    4.0521545517465929e6, 564.0440919750140482e6, 715.0237289404330128e6
                }
            },
            NetSalesGas = new NetSalesGasDto
            {
                StartYear = 2030,
                Values = new[]
                {
                    654.045534094613436e9,
                    54.0454132011274522e9, 852.0406768005522276e9,
                    56.0343885703843194e9, 15.0290724382700382e9,
                    67.0245781274859447e9, 16.0207785926004667e9,
                    243.0175664281464513e9, 21.0148508324773406e9,
                    54.0125550409810879e9, 22.0106141560937621e9,
                    12.00897331277153478e9, 23.00758612756252165e9,
                    13.00641338743673343e9, 24.0054219413099335e9,
                    14.00458376292690185e9, 25.00246683976884294e9
                }
            },
            Co2Emissions = new Co2EmissionsDto
            {
                StartYear = 2029,
                Values = new[]
                {
                    85.009e6, 7521.00202727509188372e6,
                    232.00202189265582155e6, 85.00181101799161985e6,
                    21.0015310525613273e6, 21.00129436701147856e6,
                    22.0010942707015568e6, 44.000925107297749942e6,
                    23.000782094879386459e6, 71.00066119076333117e6,
                    24.000558977225189606e6, 72.000472564886881495e6,
                    25.000399511039537549e6, 74.000337750592867049e6,
                    26.000285537699068565e6, 45.000241396401105546e6,
                    27.000204078910269268e6, 35.000109828972366755e6
                }
            }
        };
    }

    public static SubstructureDto Case2Substructure()
    {
        return new SubstructureDto
        {
            Name = "SkarvenSubCase2",
            CostProfile = new SubstructureCostProfileDto
            {
                Currency = Currency.NOK,
                StartYear = 2027,
                Values = new[]
                {
                    349.95166651869e6, 427.288025880945e6,
                    424.163092995196e6, 39.8227522796791e6
                }
            }
        };
    }

    public static WellProjectDto Case2WellProject()
    {
        return new WellProjectDto
        {
            Name = "SkarvenWellCase2",
            AnnualWellInterventionCost = 85e6,
            CostProfile = new WellProjectCostProfileDto
            {
                Currency = Currency.NOK,
                StartYear = 2029,
                Values = new[] { 764e6 }
            }
        };
    }

    public static ExplorationDto Case2Exploration()
    {
        return new ExplorationDto
        {
            Name = "SkarvenExplCase2",
            CostProfile = new ExplorationCostProfileDto
            {
                Currency = Currency.NOK,
                StartYear = 2023,
                Values = new[] { 280e6 }
            },
            GAndGAdminCost = new GAndGAdminCostDto
            {
                Currency = Currency.NOK,
                StartYear = 2022,
                Values = new[] { 9e6, 9e6, 9e6 }
            }
        };
    }
}
