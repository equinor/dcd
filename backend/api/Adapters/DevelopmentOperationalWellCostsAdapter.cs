using api.Dtos;
using api.Models;

namespace api.Adapters
{
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
                RigUpgrading = developmentOperationalWellCostsDto.RigUpgrading,
                RigMobDemob = developmentOperationalWellCostsDto.RigMobDemob,
                AnnualWellInterventionCostPerWell = developmentOperationalWellCostsDto.AnnualWellInterventionCostPerWell,
                PluggingAndAbandonment = developmentOperationalWellCostsDto.PluggingAndAbandonment,
            };
        }
    }
}
