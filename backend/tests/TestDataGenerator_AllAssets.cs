using api.Dtos;
using api.Models;

namespace tests;

public static class TestDataGenerator_AllAssets
{
    public static ProjectDto OneProject()
    {
        return new ProjectDto
        {
            Name = "All Assets",
            CommonLibraryName = "The All Assets Project",
            Country = "Norway",
            Description = "Just a project that has all assets",
            ProjectCategory = ProjectCategory.Co2Capture,
            ProjectPhase = ProjectPhase.ConceptPlanning,
            CreateDate = DateTimeOffset.UtcNow.Date
        };
    }
    public static CaseDto CaseA()
    {
        return new CaseDto
        {
            Name = "Case A",
            CreateTime = DateTimeOffset.UtcNow,
            Description = "Case A",
            ModifyTime = DateTimeOffset.UtcNow,
            DG4Date = DateTime.Parse("Jun 01 2029")
        };
    }

    public static CaseDto CaseB()
    {
        return new CaseDto
        {
            Name = "Case B",
            CreateTime = DateTimeOffset.UtcNow,
            Description = "Case B",
            ModifyTime = DateTimeOffset.UtcNow,
            DG4Date = DateTime.Parse("Mar 01 2027")
        };
    }
    public static DrainageStrategyDto Case2DrainageStrategy(Guid projectId)
    {
        return new DrainageStrategyDto
        {
            Name = "A drainage strategy",
            Description = "All asset project drainage strategy",
            ProjectId = projectId,
            ProductionProfileGas = new ProductionProfileGasDto
            {
                StartYear = -3,
                Values = new[] { 1e6, 2e6, 1.5e6, 1e6 }
            },
            ProductionProfileOil = new ProductionProfileOilDto
            {
                StartYear = -4,
                Values = new[] { 1e6, 1.1e6, 1.2e6, 1.3e6 }
            },
            NetSalesGas = new NetSalesGasDto
            {
                StartYear = -4,
                Values = new[] { 5e6, 6e6, 5e6 }
            },
            Co2Emissions = new Co2EmissionsDto
            {
                StartYear = -4,
                Values = new[] { 7e6, 8e6, 6e6, 9e6 }
            }
        };
    }

    public static DrainageStrategyDto UpdatedDrainageStrategy()
    {
        return new DrainageStrategyDto
        {
            Name = "A drainage strategy",
            Description = "All asset project drainage strategy",
            ProductionProfileGas = new ProductionProfileGasDto
            {
                StartYear = -1,
                Values = new[] { 6e6, 8e6, 1.5e6, 1e6 }
            },
            ProductionProfileOil = new ProductionProfileOilDto
            {
                StartYear = -14,
                Values = new[] { 1e6, 1.1e6, 14e6, 1.3e6 }
            },
            NetSalesGas = new NetSalesGasDto
            {
                StartYear = -5,
                Values = new[] { 5e6, 7e6, 5e6 }
            },
            Co2Emissions = new Co2EmissionsDto
            {
                StartYear = -7,
                Values = new[] { 9e6, 8e6, 6e6, 19e6 }
            }
        };
    }

    public static TransportDto Case2Transport(Guid ProjectId)
    {
        return new TransportDto
        {
            Name = "A topside strategy",
            ProjectId = ProjectId,
            CostProfile = new TransportCostProfileDto
            {
                Currency = Currency.NOK,
                StartYear = -7,
                Values = new[] { 30e6, 31e6, 32e6, 31e6 }
            }
        };
    }
    public static TopsideDto Case2Topside(Guid ProjectId)
    {
        return new TopsideDto
        {
            Name = "A topside strategy",
            ProjectId = ProjectId,
            CostProfile = new TopsideCostProfileDto
            {
                Currency = Currency.NOK,
                StartYear = -9,
                Values = new[] { 20e6, 21e6, 22e6, 21e6 }
            }
        };
    }
    public static SurfDto Case2Surf(Guid ProjectId)
    {
        return new SurfDto
        {
            Name = "A surf strategy",
            ProjectId = ProjectId,
            CostProfile = new SurfCostProfileDto
            {
                Currency = Currency.NOK,
                StartYear = -8,
                Values = new[] { 10e6, 11e6, 12e6, 11e6 }
            }
        };
    }
    public static SubstructureDto Case2Substructure()
    {
        return new SubstructureDto
        {
            Name = "A substructure strategy",
            CostProfile = new SubstructureCostProfileDto
            {
                Currency = Currency.NOK,
                StartYear = -8,
                Values = new[] { 10e6, 11e6, 12e6, 11e6 }
            }
        };
    }
    public static WellProjectDto Case2WellProject()
    {
        return new WellProjectDto
        {
            Name = "A well strategy",
            AnnualWellInterventionCost = 85e6,
            CostProfile = new WellProjectCostProfileDto
            {
                Currency = Currency.NOK,
                StartYear = -9,
                Values = new[] { 70e6, 75e6, 80e6 }
            }
        };
    }
    public static ExplorationDto Case2Exploration(Guid ProjectId)
    {
        return new ExplorationDto
        {
            Name = "An exploration strategy",
            ProjectId = ProjectId,
            CostProfile = new ExplorationCostProfileDto
            {
                Currency = Currency.NOK,
                StartYear = -9,
                Values = new[] { 280e6 }

            },
            GAndGAdminCost = new GAndGAdminCostDto
            {
                Currency = Currency.NOK,
                StartYear = -10,
                Values = new[] { 9e6, 9e6, 9e6 }
            }
        };
    }
}
