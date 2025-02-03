using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class DevelopmentWellDto
{
    [Required] public required TimeSeriesScheduleDto DrillingSchedule { get; set; }
    [Required] public required Guid WellProjectId { get; set; }
    [Required] public required Guid WellId { get; set; }
}
