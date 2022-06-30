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
                WellTypes = Convert(wellDto.WellTypes),
                ExplorationWellTypes = Convert(wellDto.ExplorationWellTypes)
            };
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

        private static ICollection<WellType> Convert(ICollection<WellTypeDto> wellTypes)
        {
            if (wellTypes == null)
            {
                return null!;
            }
            var wellTypeList = new List<WellType>();
            foreach (var w in wellTypes)
            {
                wellTypeList.Add(new WellType
                {
                    Category = (WellTypeCategory)w.Category,
                    Description = w.Description,
                    DrillingDays = w.DrillingDays,
                    Id = w.Id,
                    Name = w.Name,
                    WellCost = w.WellCost,
                });
            }
            return wellTypeList;
        }

        private static ICollection<ExplorationWellType> Convert(ICollection<ExplorationWellTypeDto> explorationWellTypes)
        {
            if (explorationWellTypes == null)
            {
                return null!;
            }

            var explorationWellTypeList = new List<ExplorationWellType>();
            foreach (var e in explorationWellTypes)
            {
                explorationWellTypeList.Add(new ExplorationWellType
                {
                    Category = (ExplorationWellTypeCategory)e.Category,
                    Description = e.Description,
                    DrillingDays = e.DrillingDays,
                    Id = e.Id,
                    Name = e.Name,
                    WellCost = e.WellCost
                });
            }
            return explorationWellTypeList;
        }
    }
}
