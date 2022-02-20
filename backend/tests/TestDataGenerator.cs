using api.Dtos;
using api.Models;

namespace tests;

public static class TestDataGenerator
{
    public static ProjectDto SpreadSheetProject()
    {
        return new ProjectDto()
        {
            Name = "Skarven",
            CommonLibraryName = "The Skarven Project",
            CreateDate = DateTimeOffset.UtcNow,
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
            },
            ProductionProfileOil = new ProductionProfileOilDto
            {
                StartYear = 2030,
                Values = new double[] { 0.438e6, 0.436837105529155e6,
                0.391276883687471e6, 0.330789355892615e6, 0.279652600329056e6,
                0.236421080296768e6, 0.199872724741057e6, 0.168974382678855e6,
                0.142852618028251e6, 0.120769019267905e6, 0.102099325978384e6,
                0.0863157822132823e6,  0.0729722178642801e6,  0.0616914362992658e6,
                0.0521545517465929e6,  0.0440919750140482e6,  0.0237289404330128e6
                }
            },
            NetSalesGas = new NetSalesGasDto
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
            },
            Co2Emissions = new Co2EmissionsDto
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
                Values = new double[] { 349.95166651869e6, 427.288025880945e6,
                424.163092995196e6,  39.8227522796791e6
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
                Values = new double[] { 764e6 }
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
                Values = new double[] { 280e6 }

            },
            GAndGAdminCost = new GAndGAdminCostDto
            {
                Currency = Currency.NOK,
                StartYear = 2022,
                Values = new double[] { 9e6, 9e6, 9e6 }
            }
        };
    }
}
