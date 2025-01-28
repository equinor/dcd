using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class ExplorationWellDto
{
    [Required] public TimeSeriesScheduleDto DrillingSchedule { get; set; } = new();
    [Required] public Guid ExplorationId { get; set; }
    [Required] public Guid WellId { get; set; }
}
