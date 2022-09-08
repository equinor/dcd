using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class ExplorationOperationalWellCostsAdapter
    {
        public static ExplorationOperationalWellCosts Convert(ExplorationOperationalWellCostsDto? explorationOperationalWellCostsDto)
        {
            if (explorationOperationalWellCostsDto == null)
            {
                return null!;
            }
            return new ExplorationOperationalWellCosts
            {
                Id = explorationOperationalWellCostsDto.Id,
                ProjectId = explorationOperationalWellCostsDto.ProjectId,
                ExplorationRigUpgrading = explorationOperationalWellCostsDto.RigUpgrading,
                ExplorationRigMobDemob = explorationOperationalWellCostsDto.ExplorationRigMobDemob,
                ExplorationProjectDrillingCosts = explorationOperationalWellCostsDto.ExplorationProjectDrillingCosts,
                AppraisalRigMobDemob = explorationOperationalWellCostsDto.AppraisalRigMobDemob,
                AppraisalProjectDrillingCosts = explorationOperationalWellCostsDto.AppraisalProjectDrillingCosts,
            };
        }
    }
}
