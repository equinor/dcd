using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class ExplorationDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required double RigMobDemob { get; set; }
    [Required] public required Currency Currency { get; set; }
}
