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
                Id = costProfile.Id,
                Currency = costProfile.Currency,
                EPAVersion = costProfile.EPAVersion,
                StartYear = costProfile.StartYear,
                Values = costProfile.Values,
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
                Id = drillingSchedule.Id,
                StartYear = drillingSchedule.StartYear,
                Values = drillingSchedule.Values,
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
                Id = gAndGAdminCost.Id,
                Currency = gAndGAdminCost.Currency,
                EPAVersion = gAndGAdminCost.EPAVersion,
                StartYear = gAndGAdminCost.StartYear,
                Values = gAndGAdminCost.Values,
            };
        }
    }
}
