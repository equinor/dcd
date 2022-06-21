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
                WellType = Convert(well.WellType),
                PlugingAndAbandonmentCost = well.PlugingAndAbandonmentCost,
                WellInterventionCost = well.WellInterventionCost,
            };
            return wellDto;
        }

        public static WellTypeNew? Convert(WellTypeNew? wellType)
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
    }
}