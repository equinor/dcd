using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class ExplorationWellDtoAdapter
{
    public static ExplorationWellDto Convert(ExplorationWell explorationWell)
    {
        var explorationWellDto = new ExplorationWellDto
        {
            ExplorationId = explorationWell.ExplorationId,
            WellId = explorationWell.WellId,
        };

        if (explorationWell.DrillingSchedule != null)
        {
            explorationWellDto.DrillingSchedule = Convert(explorationWell.DrillingSchedule);
        }
        return explorationWellDto;
    }

    private static DrillingScheduleDto? Convert(DrillingSchedule? drillingSchedule)
    {
        if (drillingSchedule == null)
        {
            return null!;
        }
        var drillingScheduleDto = new DrillingScheduleDto
        {
            Id = drillingSchedule.Id,
            StartYear = drillingSchedule.StartYear,
            Values = drillingSchedule.Values
        };
        return drillingScheduleDto;
    }
}
