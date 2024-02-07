using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class WellProjectWellDtoAdapter
{
    public static WellProjectWellDto Convert(WellProjectWell wellProjectWell)
    {
        var wellProjectWellDto = new WellProjectWellDto
        {
            WellProjectId = wellProjectWell.WellProjectId,
            WellId = wellProjectWell.WellId,
        };

        if (wellProjectWell.DrillingSchedule != null)
        {
            wellProjectWellDto.DrillingSchedule = Convert(wellProjectWell.DrillingSchedule) ?? new DrillingScheduleDto();
        }
        return wellProjectWellDto;
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
