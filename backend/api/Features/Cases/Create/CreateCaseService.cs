using api.Context;
using api.Context.Extensions;
using api.Features.Profiles;
using api.Models;

namespace api.Features.Cases.Create;

public class CreateCaseService(DcdDbContext context)
{
    public async Task CreateCase(Guid projectId, CreateCaseDto createCaseDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var createdCase = new Case
        {
            ProjectId = projectPk,
            Name = createCaseDto.Name,
            Description = createCaseDto.Description,
            ProductionStrategyOverview = createCaseDto.ProductionStrategyOverview,
            ProducerCount = createCaseDto.ProducerCount,
            GasInjectorCount = createCaseDto.GasInjectorCount,
            WaterInjectorCount = createCaseDto.WaterInjectorCount,
            DG4Date = createCaseDto.DG4Date == DateTime.MinValue ? new DateTime(2030, 1, 1) : createCaseDto.DG4Date,
            CapexFactorFeasibilityStudies = 0.015,
            CapexFactorFEEDStudies = 0.015,
            DrainageStrategy = CreateDrainageStrategy(projectPk),
            Topside = CreateTopside(projectPk),
            Surf = CreateSurf(projectPk),
            Substructure = CreateSubstructure(projectPk),
            Transport = CreateTransport(projectPk),
            Exploration = CreateExploration(projectPk),
            WellProject = CreateWellProject(projectPk),
            OnshorePowerSupply = CreateOnshorePowerSupply(projectPk),
            TimeSeriesProfiles = new List<TimeSeriesProfile>
            {
                new()
                {
                    ProfileType = ProfileTypes.TopsideCostProfileOverride,
                    Override = true
                }
            }
        };

        context.Cases.Add(createdCase);

        await context.SaveChangesAsync();
    }

    private static DrainageStrategy CreateDrainageStrategy(Guid projectPk)
    {
        return new DrainageStrategy
        {
            Name = "Drainage Strategy",
            Description = "Drainage Strategy",
            ProjectId = projectPk
        };
    }

    private static Topside CreateTopside(Guid projectPk)
    {
        return new Topside
        {
            Name = "Topside",
            ProjectId = projectPk
        };
    }

    private static Surf CreateSurf(Guid projectPk)
    {
        return new Surf
        {
            Name = "Surf",
            ProjectId = projectPk,
            CostProfileOverride = new SurfCostProfileOverride
            {
                Override = true
            }
        };
    }

    private static Substructure CreateSubstructure(Guid projectPk)
    {
        return new Substructure
        {
            Name = "Substructure",
            ProjectId = projectPk,
            CostProfileOverride = new SubstructureCostProfileOverride
            {
                Override = true
            }
        };
    }

    private static Transport CreateTransport(Guid projectPk)
    {
        return new Transport
        {
            Name = "Transport",
            ProjectId = projectPk,
            CostProfileOverride = new TransportCostProfileOverride
            {
                Override = true
            }
        };
    }

    private static OnshorePowerSupply CreateOnshorePowerSupply(Guid projectPk)
    {
        return new OnshorePowerSupply
        {
            Name = "OnshorePowerSupply",
            ProjectId = projectPk,
            CostProfileOverride = new OnshorePowerSupplyCostProfileOverride
            {
                Override = true
            }
        };
    }

    private static Exploration CreateExploration(Guid projectPk)
    {
        return new Exploration
        {
            Name = "Exploration",
            ProjectId = projectPk
        };
    }

    private static WellProject CreateWellProject(Guid projectPk)
    {
        return new WellProject
        {
            Name = "Well Project",
            ProjectId = projectPk
        };
    }
}
