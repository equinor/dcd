using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class ExplorationOperationalWellCostsDtoAdapter
{
    public static ExplorationOperationalWellCostsDto Convert(ExplorationOperationalWellCosts? explorationOperationalWellCostsDto)
    {
        if (explorationOperationalWellCostsDto == null)
        {
            return null!;
        }
        return new ExplorationOperationalWellCostsDto
        {
            Id = explorationOperationalWellCostsDto.Id,
            ProjectId = explorationOperationalWellCostsDto.ProjectId,
            RigUpgrading = explorationOperationalWellCostsDto.ExplorationRigUpgrading,
            ExplorationRigMobDemob = explorationOperationalWellCostsDto.ExplorationRigMobDemob,
            ExplorationProjectDrillingCosts = explorationOperationalWellCostsDto.ExplorationProjectDrillingCosts,
            AppraisalRigMobDemob = explorationOperationalWellCostsDto.AppraisalRigMobDemob,
            AppraisalProjectDrillingCosts = explorationOperationalWellCostsDto.AppraisalProjectDrillingCosts,
        };
    }
}
