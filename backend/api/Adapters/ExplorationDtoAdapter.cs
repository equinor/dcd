using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class ExplorationDtoAdapter
    {
        public static ExplorationDto Convert(Exploration exploration)
        {
            var explorationDto = new ExplorationDto();
            explorationDto.Id = exploration.Id;
            explorationDto.ProjectId = exploration.ProjectId;
            explorationDto.Name = exploration.Name;
            explorationDto.RigMobDemob = exploration.RigMobDemob;
            explorationDto.WellType = exploration.WellType;
            explorationDto.CostProfile = Convert(exploration.CostProfile);
            explorationDto.DrillingSchedule = Convert(exploration.DrillingSchedule);
            explorationDto.GAndGAdminCost = Convert(exploration.GAndGAdminCost);
            return explorationDto;
        }

        private static ExplorationCostProfileDto Convert(ExplorationCostProfile? costProfile)
        {
            if (costProfile == null)
            {
                return null!;
            }
            return new ExplorationCostProfileDto
            {
                Currency = costProfile.Currency,
                EPAVersion = costProfile.EPAVersion,
                YearValues = costProfile.YearValues,
            };
        }

        private static ExplorationDrillingScheduleDto Convert(ExplorationDrillingSchedule? drillingSchedule)
        {
            if (drillingSchedule == null)
            {
                return null!;
            }
            return new ExplorationDrillingScheduleDto
            {
                YearValues = drillingSchedule.YearValues,
            };
        }
        private static GAndGAdminCostDto Convert(GAndGAdminCost? gAndGAdminCost)
        {
            if (gAndGAdminCost == null)
            {
                return null!;
            }
            return new GAndGAdminCostDto
            {
                Currency = gAndGAdminCost.Currency,
                EPAVersion = gAndGAdminCost.EPAVersion,
                YearValues = gAndGAdminCost.YearValues,
            };
        }
    }
}
