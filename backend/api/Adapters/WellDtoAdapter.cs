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
                ProjectId = well.ProjectId,
                WellTypes = Convert(well.WellTypes),
                ExplorationWellTypes = Convert(well.ExplorationWellTypes),
                PlugingAndAbandonmentCost = well.PlugingAndAbandonmentCost,
                WellInterventionCost = well.WellInterventionCost,
            };
            return wellDto;
        }

        public static ICollection<WellTypeDto> Convert(ICollection<WellType> wellTypes)
        {
            if (wellTypes == null)
            {
                return null!;
            }
            var wellTypesList = new List<WellTypeDto>();
            foreach (var w in wellTypes)
            {
                wellTypesList.Add(new WellTypeDto
                {
                    Category = (WellTypeCategoryDto)w.Category,
                    Description = w.Description,
                    DrillingDays = w.DrillingDays,
                    Id = w.Id,
                    Name = w.Name,
                    WellCost = w.WellCost,
                });
            }
            return wellTypesList;
        }

        public static ICollection<ExplorationWellTypeDto> Convert(ICollection<ExplorationWellType> explorationWellTypes)
        {
            if (explorationWellTypes == null)
            {
                return null!;
            }
            var explorationWellTypeList = new List<ExplorationWellTypeDto>();
            foreach (var e in explorationWellTypes)
            {
                explorationWellTypeList.Add(new ExplorationWellTypeDto
                {
                    Category = (ExplorationWellTypeCategoryDto)e.Category,
                    Description = e.Description,
                    DrillingDays = e.DrillingDays,
                    Id = e.Id,
                    Name = e.Name,
                    WellCost = e.WellCost,
                });
            }
            return explorationWellTypeList;
        }
    }
}
