using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.ProjectData.Dtos.AssetDtos;

public class CaseOverviewDto
{
    [Required] public required Guid CaseId { get; set; }
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required bool Archived { get; set; }
    [Required] public required bool ReferenceCase {get; set; }
    [Required] public required ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    [Required] public required ArtificialLift ArtificialLift { get; set; }
    [Required] public required int ProducerCount { get; set; }
    [Required] public required int GasInjectorCount { get; set; }
    [Required] public required int WaterInjectorCount { get; set; }
    [Required] public required double NPV { get; set; }
    public required double? NPVOverride { get; set; }
    [Required] public required double BreakEven { get; set; }
    public required double? BreakEvenOverride { get; set; }
    [Required] public required double FacilitiesAvailability { get; set; }
    [Required] public required double CapexFactorFeasibilityStudies { get; set; }
    [Required] public required double CapexFactorFEEDStudies { get; set; }
    public required string? Host { get; set; }

    [Required] public required DateTimeOffset DGADate { get; set; }
    [Required] public required DateTimeOffset DGBDate { get; set; }
    [Required] public required DateTimeOffset DGCDate { get; set; }
    [Required] public required DateTimeOffset APBODate { get; set; }
    [Required] public required DateTimeOffset BORDate { get; set; }
    [Required] public required DateTimeOffset VPBODate { get; set; }

    [Required] public required DateTimeOffset DG0Date { get; set; }
    [Required] public required DateTimeOffset DG1Date { get; set; }
    [Required] public required DateTimeOffset DG2Date { get; set; }
    [Required] public required DateTimeOffset DG3Date { get; set; }
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
