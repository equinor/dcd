using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class ExplorationAdapter
    {

        public static Exploration Convert(ExplorationDto explorationDto)
        {
            var exploration = new Exploration
            {
                Id = explorationDto.Id,
                ProjectId = explorationDto.ProjectId,
                Name = explorationDto.Name,
                RigMobDemob = explorationDto.RigMobDemob,
                Currency = explorationDto.Currency,
            };
            exploration.CostProfile = Convert(explorationDto.CostProfile, exploration);
            exploration.GAndGAdminCost = Convert(explorationDto.GAndGAdminCost, exploration);
            return exploration;
        }

        public static void ConvertExisting(Exploration existing, ExplorationDto explorationDto)
        {
            existing.Id = explorationDto.Id;
            existing.ProjectId = explorationDto.ProjectId;
            existing.Name = explorationDto.Name;
            existing.RigMobDemob = explorationDto.RigMobDemob;
            existing.Currency = explorationDto.Currency;
            existing.CostProfile = Convert(explorationDto.CostProfile, existing);
            existing.GAndGAdminCost = Convert(explorationDto.GAndGAdminCost, existing);
        }

        private static ExplorationCostProfile Convert(ExplorationCostProfileDto? costProfileDto, Exploration exploration)
        {
            if (costProfileDto == null)
            {
                return null!;
            }
            return new ExplorationCostProfile
            {
                Id = costProfileDto.Id,
                Currency = costProfileDto.Currency,
                EPAVersion = costProfileDto.EPAVersion,
                Exploration = exploration,
                StartYear = costProfileDto.StartYear,
                Values = costProfileDto.Values,
            };
        }

        private static GAndGAdminCost Convert(GAndGAdminCostDto? gAndGAdminCostDto, Exploration exploration)
        {
            if (gAndGAdminCostDto == null)
            {
                return null!;
            }
            return new GAndGAdminCost
            {
                Id = gAndGAdminCostDto.Id,
                Currency = gAndGAdminCostDto.Currency,
                EPAVersion = gAndGAdminCostDto.EPAVersion,
                Exploration = exploration,
                Values = gAndGAdminCostDto.Values,
                StartYear = gAndGAdminCostDto.StartYear
            };
        }
    }
}
