using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellAdapter
    {
        public static Well Convert(WellDto wellDto)
        {
            var well = new Well
            {
                Id = wellDto.Id,
                ProjectId = wellDto.ProjectId,
                WellInterventionCost = wellDto.WellInterventionCost,
                PlugingAndAbandonmentCost = wellDto.PlugingAndAbandonmentCost,
            };
            well.WellTypes = Convert(wellDto.WellTypes);
            well.ExplorationWellTypes = Convert(wellDto.ExplorationWellTypes);
            return well;
        }

        public static void ConvertExisting(Well existing, WellDto wellDto)
        {
            existing.Id = wellDto.Id;
            existing.ProjectId = wellDto.ProjectId;
            existing.WellInterventionCost = wellDto.WellInterventionCost;
            existing.PlugingAndAbandonmentCost = wellDto.PlugingAndAbandonmentCost;
            existing.WellTypes = Convert(wellDto.WellTypes);
            existing.ExplorationWellTypes = Convert(wellDto.ExplorationWellTypes);
        }

        private static WellType Convert(WellTypeDto? wellType)
        {
            if (wellType == null)
            {
                return null!;
            }
            return new WellType
            {
                Id = wellType.Id,
                Description = wellType.Description,
                Category = (WellTypeCategory)wellType.Category,
                Name = wellType.Name,
                DrillingDays = wellType.DrillingDays,
                WellCost = wellType.WellCost,
            };
        }

        private static ExplorationWellType Convert(ExplorationWellTypeDto? explorationWellType)
        {
            if (explorationWellType == null)
            {
                return null!;
            }
            return new ExplorationWellType
            {
                Id = explorationWellType.Id,
                Description = explorationWellType.Description,
                Name = explorationWellType.Name,
                DrillingDays = explorationWellType.DrillingDays,
                WellCost = explorationWellType.WellCost,
                Category = (ExplorationWellTypeCategory)explorationWellType.Category,
            };
        }
    }
}
