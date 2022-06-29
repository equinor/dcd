using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class ExplorationDtoAdapter
    {
        public static ExplorationDto Convert(Exploration exploration)
        {
            var explorationDto = new ExplorationDto
            {
                Id = exploration.Id,
                ProjectId = exploration.ProjectId,
                Name = exploration.Name,
                RigMobDemob = exploration.RigMobDemob,
                Currency = exploration.Currency,
                CostProfile = Convert(exploration.CostProfile),
                DrillingSchedule = Convert(exploration.DrillingSchedule),
                GAndGAdminCost = Convert(exploration.GAndGAdminCost),
                ExplorationWellTypes = exploration.ExplorationWellTypes
            };
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
