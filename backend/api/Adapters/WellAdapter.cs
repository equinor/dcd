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
                Name = wellDto.Name,
                ProjectId = wellDto.ProjectId,
                WellType = wellDto.WellType,
                ExplorationWellType = wellDto.ExplorationWellType,
                WellInterventionCost = wellDto.WellInterventionCost,
                PlugingAndAbandonmentCost = wellDto.PlugingAndAbandonmentCost,
            };
            well.WellType = Convert(wellDto.WellType, well);
            return well;
        }

        public static void ConvertExisting(Well existing, WellDto wellDto)
        {
            existing.Id = wellDto.Id;
            existing.Name = wellDto.Name;
            existing.ProjectId = wellDto.ProjectId;
            existing.WellInterventionCost = wellDto.WellInterventionCost;
            existing.PlugingAndAbandonmentCost = wellDto.PlugingAndAbandonmentCost;
            existing.WellType = Convert(wellDto.WellType, existing);
            existing.ExplorationWellType = Convert(wellDto.ExplorationWellType, existing);
        }

        private static WellTypeNew Convert(WellTypeNew? wellType, Well well)
        {
            if (wellType == null)
            {
                return null!;
            }
            return new WellTypeNew
            {
                Name = wellType.Name,
                DrillingDays = wellType.DrillingDays,
                WellCost = wellType.WellCost,
            };
        }

        private static ExplorationWellType Convert(ExplorationWellType? explorationWellType, Well well)
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
