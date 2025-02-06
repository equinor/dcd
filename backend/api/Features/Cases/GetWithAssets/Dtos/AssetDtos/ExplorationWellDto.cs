using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class ExplorationWellDto
{
    [Required] public required TimeSeriesScheduleDto DrillingSchedule { get; set; }
    [Required] public required Guid ExplorationId { get; set; }
    [Required] public required Guid WellId { get; set; }
    [Required] public DateTime UpdatedUtc { get; set; }
}
