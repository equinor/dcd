using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.ProjectData.Dtos.AssetDtos;

public class OnshorePowerSupplyOverviewDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required Source Source { get; set; }
}
