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
                wellProjectWell.DrillingSchedule = Convert(wellProjectWellDto.DrillingSchedule);
            }
            return wellProjectWell;
        }

        public static void ConvertExisting(WellProjectWell existing, WellProjectWellDto wellProjectWellDto)
        {
            existing.Count = wellProjectWellDto.Count;
            if (wellProjectWellDto.DrillingSchedule != null)
            {
                if (existing.DrillingSchedule != null)
                {
                    existing.DrillingSchedule = ConvertExisting(wellProjectWellDto.DrillingSchedule, existing);
                }
                else
                {
                    existing.DrillingSchedule = Convert(wellProjectWellDto.DrillingSchedule);
                }
            }
        }

        private static DrillingSchedule? ConvertExisting(DrillingScheduleDto? drillingScheduleDto, WellProjectWell wellProjectWell)
        {
            if (drillingScheduleDto == null) { return null; }

            var existing = wellProjectWell.DrillingSchedule;

            existing!.Id = drillingScheduleDto.Id;
            existing.StartYear = drillingScheduleDto.StartYear;
            existing.Values = drillingScheduleDto.Values;
            return existing;
        }

        private static DrillingSchedule? Convert(DrillingScheduleDto? drillingScheduleDto)
        {
            if (drillingScheduleDto == null) { return null; }
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
