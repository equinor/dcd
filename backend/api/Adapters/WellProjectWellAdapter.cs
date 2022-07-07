using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellProjectWellAdapter
    {
        public static WellProjectWell Convert(WellProjectWellDto wellProjectWellDto)
        {
            var wellProjectWell = new WellProjectWell
            {
                Count = wellProjectWellDto.Count,
                WellProjectId = wellProjectWellDto.WellProjectId,
                WellId = wellProjectWellDto.WellId,
            };
            if (wellProjectWellDto.DrillingSchedule != null)
            {
                wellProjectWell.DrillingSchedule = Convert(wellProjectWellDto.DrillingSchedule, wellProjectWell);
            }
            return wellProjectWell;
        }

        public static void ConvertExisting(WellProjectWell existing, WellProjectWellDto wellProjectWellDto)
        {
            existing.Count = wellProjectWellDto.Count;
            if (wellProjectWellDto.DrillingSchedule != null)
            {
                existing.DrillingSchedule = Convert(wellProjectWellDto.DrillingSchedule, existing);
            }
        }

        private static DrillingSchedule? Convert(DrillingScheduleDto? drillingScheduleDto, WellProjectWell wellProject)
        {
            if (drillingScheduleDto == null) return null;
            var drillingSchedule = new DrillingSchedule
            {
                Id = drillingScheduleDto.Id,
                StartYear = drillingScheduleDto.StartYear,
                Values = drillingScheduleDto.Values
            };
            return drillingSchedule;
        }
    }
}
