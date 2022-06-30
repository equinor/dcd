using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellTypeDtoAdapter
    {
        public static WellTypeDto Convert(WellType wellType)
        {
            var wellTypeDto = new WellTypeDto
            {
                Id = wellType.Id,
                // ProjectId = well.ProjectId,
                Category = (WellTypeCategoryDto)wellType.Category,
                Description = wellType.Description,
                DrillingDays = wellType.DrillingDays,
                Name = wellType.Name,
                WellCost = wellType.WellCost,
            };
            return wellTypeDto;
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
