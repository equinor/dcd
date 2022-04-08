using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class ExplorationAdapter
    {

        public static Exploration Convert(ExplorationDto explorationDto)
        {
            var exploration = new Exploration();
            exploration.Id = explorationDto.Id;
            exploration.ProjectId = explorationDto.ProjectId;
            exploration.Name = explorationDto.Name;
            exploration.RigMobDemob = explorationDto.RigMobDemob;
            exploration.WellType = explorationDto.WellType;
            exploration.CostProfile = Convert(explorationDto.CostProfile, exploration);
            exploration.DrillingSchedule = Convert(explorationDto.DrillingSchedule, exploration);
            exploration.GAndGAdminCost = Convert(explorationDto.GAndGAdminCost, exploration);
            return exploration;
        }

        public static void ConvertExisting(Exploration existing, ExplorationDto explorationDto)
        {
            existing.Id = explorationDto.Id;
            existing.ProjectId = explorationDto.ProjectId;
            existing.Name = explorationDto.Name;
            existing.RigMobDemob = explorationDto.RigMobDemob;
            existing.WellType = explorationDto.WellType;
            existing.CostProfile = Convert(explorationDto.CostProfile, existing);
            existing.DrillingSchedule = Convert(explorationDto.DrillingSchedule, existing);
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
        private static ExplorationDrillingSchedule Convert(ExplorationDrillingScheduleDto? drillingScheduleDto, Exploration exploration)
        {
            if (drillingScheduleDto == null)
            {
                return null!;
            }
            return new ExplorationDrillingSchedule
            {
                Exploration = exploration,
                Id = drillingScheduleDto.Id,
                StartYear = drillingScheduleDto.StartYear,
                Values = drillingScheduleDto.Values
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
