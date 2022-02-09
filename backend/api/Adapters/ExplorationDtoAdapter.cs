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
    }
}
