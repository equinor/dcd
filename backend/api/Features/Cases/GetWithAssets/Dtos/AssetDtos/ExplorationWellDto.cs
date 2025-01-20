using System.ComponentModel.DataAnnotations;

using api.Features.Assets.CaseAssets.DrillingSchedules.Dtos;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class ExplorationWellDto
{
    [Required]
    public DrillingScheduleDto DrillingSchedule { get; set; } = new();
    [Required]
    public Guid ExplorationId { get; set; } = Guid.Empty;
    [Required]
    public Guid WellId { get; set; } = Guid.Empty;
}
