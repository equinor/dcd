using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.ProjectData.Dtos.AssetDtos;

public class WellOverviewDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required string? Name { get; set; }
    [Required] public required WellCategory WellCategory { get; set; }
    [Required] public required double WellCost { get; set; }
    [Required] public required double DrillingDays { get; set; }
}
