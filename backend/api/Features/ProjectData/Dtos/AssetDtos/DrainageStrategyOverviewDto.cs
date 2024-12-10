using System.ComponentModel.DataAnnotations;

namespace api.Features.ProjectData.Dtos.AssetDtos;

public class DrainageStrategyOverviewDto
{
    [Required] public required Guid Id { get; set; }
}
