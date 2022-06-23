using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellDtoAdapter
    {
        public static WellDto Convert(Well well)
        {
            var wellDto = new WellDto
            {
                Id = well.Id,
                Name = well.Name,
                ProjectId = well.ProjectId,
                WellType = Convert(well.WellType),
                ExplorationWellType = Convert(well.ExplorationWellType),
                PlugingAndAbandonmentCost = well.PlugingAndAbandonmentCost,
                WellInterventionCost = well.WellInterventionCost,
            };
            return wellDto;
        }

        public static WellType? Convert(WellType? wellType)
        {
            if (wellType == null)
            {
                return null!;
            }
            return new WellType
            {
                Name = wellType.Name,
                DrillingDays = wellType.DrillingDays,
                WellCost = wellType.WellCost,
            };
        }

        public static ExplorationWellType? Convert(ExplorationWellType? explorationWellType)
        {
            if (explorationWellType == null)
            {
                return null!;
            }
            return new ExplorationWellType
            {
                Name = explorationWellType.Name,
                DrillingDays = explorationWellType.DrillingDays,
                WellCost = explorationWellType.WellCost,
            };
        }
    }
}
