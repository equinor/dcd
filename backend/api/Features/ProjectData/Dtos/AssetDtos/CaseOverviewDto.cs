using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.ProjectData.Dtos.AssetDtos;

public class CaseOverviewDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required string Name { get; set; } = null!;
    [Required] public required string Description { get; set; } = null!;
    [Required] public required bool Archived { get; set; }
    [Required] public required ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    [Required] public required int ProducerCount { get; set; }
    [Required] public required int GasInjectorCount { get; set; }
    [Required] public required int WaterInjectorCount { get; set; }
    [Required] public required double NPV { get; set; }
    [Required] public required double? NPVOverride { get; set; }
    [Required] public required double BreakEven { get; set; }
    [Required] public required double? BreakEvenOverride { get; set; }
    [Required] public required DateTimeOffset DG4Date { get; set; }
    [Required] public required DateTimeOffset CreateTime { get; set; }
    [Required] public required DateTimeOffset ModifyTime { get; set; }
    [Required] public required Guid SurfLink { get; set; }
    [Required] public required Guid SubstructureLink { get; set; }
    [Required] public required Guid TopsideLink { get; set; }
    [Required] public required Guid TransportLink { get; set; }
    public required string? SharepointFileId { get; set; }
    public required string? SharepointFileName { get; set; }
    public required string? SharepointFileUrl { get; set; }
}
