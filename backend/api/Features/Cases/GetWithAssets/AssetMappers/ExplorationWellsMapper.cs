using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class ExplorationWellsMapper
{
    public static List<ExplorationWellDto> MapToDtos(IEnumerable<ExplorationWell> entities)
    {
        return entities
            .Where(x => x.DrillingScheduleId.HasValue)
            .Select(x => new ExplorationWellDto
            {
                ExplorationId = x.ExplorationId,
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
