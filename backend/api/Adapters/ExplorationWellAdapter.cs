using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class ExplorationWellAdapter
    {
        public static ExplorationWell Convert(ExplorationWellDto explorationWellDto)
        {
            var explorationWell = new ExplorationWell
            {
                Count = explorationWellDto.Count,
                ExplorationId = explorationWellDto.ExplorationId,
                WellId = explorationWellDto.WellId,
            };
            if (explorationWellDto.DrillingSchedule != null)
            {
                explorationWell.DrillingSchedule = Convert(explorationWellDto.DrillingSchedule);
            }
            return explorationWell;
        }

        public static void ConvertExisting(ExplorationWell existing, ExplorationWellDto explorationWellDto)
        {
            existing.Count = explorationWellDto.Count;
            if (explorationWellDto.DrillingSchedule != null)
            {
                if (existing.DrillingSchedule != null)
                {
                    existing.DrillingSchedule = ConvertExisting(explorationWellDto.DrillingSchedule, existing);
                }
                else
                {
                    existing.DrillingSchedule = Convert(explorationWellDto.DrillingSchedule);
                }
            }
        }

        private static DrillingSchedule? ConvertExisting(DrillingScheduleDto? drillingScheduleDto, ExplorationWell explorationWell)
        {
            if (drillingScheduleDto == null) { return null; }

            var existing = explorationWell.DrillingSchedule;

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
