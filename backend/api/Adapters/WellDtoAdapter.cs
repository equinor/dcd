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
                PlugingAndAbandonmentCost = well.PlugingAndAbandonmentCost,
                WellInterventionCost = well.WellInterventionCost,
            };
            return wellDto;
        }
    }
}
