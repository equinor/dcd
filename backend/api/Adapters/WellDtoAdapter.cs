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

        public static WellTypeDto? Convert(WellType? wellType)
        {
            if (wellType == null)
            {
                return null!;
            }
            return new WellTypeDto
            {
                Id = wellType.Id,
                Name = wellType.Name,
                DrillingDays = wellType.DrillingDays,
                WellCost = wellType.WellCost,
                Category = (WellTypeCategoryDto)wellType.Category
            };
        }

        public static ExplorationWellTypeDto? Convert(ExplorationWellType? explorationWellType)
        {
            if (explorationWellType == null)
            {
                return null!;
            }
            return new ExplorationWellTypeDto
            {
                Id = explorationWellType.Id,
                Name = explorationWellType.Name,
                DrillingDays = explorationWellType.DrillingDays,
                WellCost = explorationWellType.WellCost,
                Category = (ExplorationWellTypeCategoryDto)explorationWellType.Category
            };
        }
    }
}
