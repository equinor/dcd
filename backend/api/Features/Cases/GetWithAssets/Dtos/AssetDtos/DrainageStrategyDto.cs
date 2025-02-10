using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class DrainageStrategyDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required double NGLYield { get; set; }
    [Required] public required double GasShrinkageFactor { get; set; }
    [Required] public required int ProducerCount { get; set; }
    [Required] public required int GasInjectorCount { get; set; }
    [Required] public required int WaterInjectorCount { get; set; }
    [Required] public required ArtificialLift ArtificialLift { get; set; }
    [Required] public required GasSolution GasSolution { get; set; }
}
