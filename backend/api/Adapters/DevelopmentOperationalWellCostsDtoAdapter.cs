using api.Dtos;
using api.Models;

namespace api.Adapters;
public static class DevelopmentOperationalWellCostsDtoAdapter
{
    public static DevelopmentOperationalWellCostsDto Convert(DevelopmentOperationalWellCosts? developmentOperationalWellCostsDto)
    {
        if (developmentOperationalWellCostsDto == null)
        {
            return null!;
        }
        return new DevelopmentOperationalWellCostsDto
        {
            Id = developmentOperationalWellCostsDto.Id,
            ProjectId = developmentOperationalWellCostsDto.ProjectId,
            RigUpgrading = developmentOperationalWellCostsDto.RigUpgrading,
            RigMobDemob = developmentOperationalWellCostsDto.RigMobDemob,
            AnnualWellInterventionCostPerWell = developmentOperationalWellCostsDto.AnnualWellInterventionCostPerWell,
            PluggingAndAbandonment = developmentOperationalWellCostsDto.PluggingAndAbandonment,
        };
    }
}
