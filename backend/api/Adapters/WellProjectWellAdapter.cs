using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellProjectWellAdapter
    {
        public static WellProjectWell Convert(WellProjectWellDto wellDto)
        {
            var well = new WellProjectWell
            {
                Count = wellDto.Count,
                DrillingSchedule = wellDto.DrillingSchedule,
                WellProjectId = wellDto.WellProjectId,
                WellId = wellDto.WellId,
            };
            return well;
        }

        public static void ConvertExisting(WellProjectWell existing, WellProjectWellDto wellDto)
        {
            existing.Count = wellDto.Count;
            existing.DrillingSchedule = wellDto.DrillingSchedule;
        }
    }
}
