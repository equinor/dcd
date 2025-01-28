using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class WellProjectWellDto
{
    [Required] public TimeSeriesScheduleDto DrillingSchedule { get; set; } = new();
    [Required] public Guid WellProjectId { get; set; }
    [Required] public Guid WellId { get; set; }
}
