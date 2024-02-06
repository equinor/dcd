using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class DevelopmentOperationalWellCostsAdapter
{
    public static DevelopmentOperationalWellCosts Convert(DevelopmentOperationalWellCostsDto? developmentOperationalWellCostsDto)
    {
        if (developmentOperationalWellCostsDto == null)
        {
            return null!;
        }
        return new DevelopmentOperationalWellCosts
        {
            Id = developmentOperationalWellCostsDto.Id,
            ProjectId = developmentOperationalWellCostsDto.ProjectId,
            RigUpgrading = developmentOperationalWellCostsDto.RigUpgrading,
            RigMobDemob = developmentOperationalWellCostsDto.RigMobDemob,
            AnnualWellInterventionCostPerWell = developmentOperationalWellCostsDto.AnnualWellInterventionCostPerWell,
            PluggingAndAbandonment = developmentOperationalWellCostsDto.PluggingAndAbandonment,
        };
    }

    public static void ConvertExisting(DevelopmentOperationalWellCosts existing, DevelopmentOperationalWellCostsDto explorationOperationalWellCostsDto)
    {
        existing.RigUpgrading = explorationOperationalWellCostsDto.RigUpgrading;
        existing.RigMobDemob = explorationOperationalWellCostsDto.RigMobDemob;
        existing.AnnualWellInterventionCostPerWell = explorationOperationalWellCostsDto.AnnualWellInterventionCostPerWell;
        existing.PluggingAndAbandonment = explorationOperationalWellCostsDto.PluggingAndAbandonment;
    }
}
