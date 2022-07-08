using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellTypeAdapter
    {
        public static WellType Convert(WellTypeDto wellTypeDto)
        {
            var wellType = new WellType
            {
                Id = wellTypeDto.Id,
                // ProjectId = wellTypeDto.ProjectId,
                Category = (WellTypeCategory)wellTypeDto.Category,
                Description = wellTypeDto.Description,
                DrillingDays = wellTypeDto.DrillingDays,
                Name = wellTypeDto.Name,
                WellCost = wellTypeDto.WellCost
            };
            return wellType;
        }

        public static void ConvertExisting(WellType existing, WellTypeDto wellDto)
        {
            existing.Id = wellDto.Id;
            existing.Category = (WellTypeCategory)wellDto.Category;
            existing.Description = wellDto.Description;
            existing.DrillingDays = wellDto.DrillingDays;
            existing.Name = wellDto.Name;
            // existing.ProjectId = wellDto.ProjectId;
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
    }
}
