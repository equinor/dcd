using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellProjectWellDtoAdapter
    {
        public static WellProjectWellDto Convert(WellProjectWell wellProjectWell)
        {
            var wellProjectWellDto = new WellProjectWellDto
            {
                WellProjectId = wellProjectWell.WellProjectId,
                WellId = wellProjectWell.WellId,
                Count = wellProjectWell.Count,
                DrillingSchedule = wellProjectWell.DrillingSchedule,
            };
            return wellProjectWellDto;
        }
    }
}
