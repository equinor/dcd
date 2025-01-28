using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class WellProjectWellsMapper
{
    public static List<WellProjectWellDto> MapToDtos(IEnumerable<WellProjectWell> entities)
    {
        return entities
            .Where(x => x.DrillingScheduleId.HasValue)
            .Select(x => new WellProjectWellDto
            {
                WellProjectId = x.WellProjectId,
                WellId = x.WellId,
                DrillingSchedule = new TimeSeriesScheduleDto
                {
                    Id = x.DrillingSchedule!.Id,
                    StartYear = x.DrillingSchedule.StartYear,
                    Values = x.DrillingSchedule.Values.ToArray()
                }
            }).ToList();
    }
}
