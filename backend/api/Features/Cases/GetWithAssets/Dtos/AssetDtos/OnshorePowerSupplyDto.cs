using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class OnshorePowerSupplyDto
{
    [Required] public required Guid Id { get; set; }
    public required DateTime? LastChangedDate { get; set; }
    [Required] public required int CostYear { get; set; }
    [Required] public required Source Source { get; set; }
    public required DateTime? ProspVersion { get; set; }
}
