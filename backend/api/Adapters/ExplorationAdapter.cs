using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class ExplorationAdapter
    {

        public static Exploration Convert(ExplorationDto explorationDto)
        {
            var exploration = new Exploration();
            exploration.ProjectId = explorationDto.ProjectId;
            exploration.Name = explorationDto.Name;
            exploration.RigMobDemob = explorationDto.RigMobDemob;
            exploration.WellType = explorationDto.WellType;
            exploration.CostProfile = Convert(explorationDto.CostProfile, exploration);
            exploration.DrillingSchedule = Convert(explorationDto.DrillingSchedule, exploration);
            exploration.GAndGAdminCost = Convert(explorationDto.GAndGAdminCost, exploration);
            return exploration;
        }

        private static ExplorationCostProfile Convert(ExplorationCostProfileDto? costProfileDto, Exploration exploration)
        {
            if (costProfileDto == null)
            {
                return null!;
            }
            return new ExplorationCostProfile
            {
                Currency = costProfileDto.Currency,
                EPAVersion = costProfileDto.EPAVersion,
                Exploration = exploration,
                YearValues = costProfileDto.YearValues,
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
                YearValues = drillingScheduleDto.YearValues,
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
                Currency = gAndGAdminCostDto.Currency,
                EPAVersion = gAndGAdminCostDto.EPAVersion,
                Exploration = exploration,
                YearValues = gAndGAdminCostDto.YearValues,
            };
        }

    }
}
