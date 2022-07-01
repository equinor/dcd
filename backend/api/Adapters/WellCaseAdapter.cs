using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellCaseAdapter
    {
        public static WellCase Convert(WellCaseDto wellDto)
        {
            var well = new WellCase
            {
                Count = wellDto.Count,
                DrillingSchedule = wellDto.DrillingSchedule,
                CaseId = wellDto.CaseId,
                WellId = wellDto.WellId,
            };
            return well;
        }

        public static void ConvertExisting(WellCase existing, WellCaseDto wellDto)
        {
            existing.Count = wellDto.Count;
            existing.DrillingSchedule = wellDto.DrillingSchedule;
        }
    }
}
