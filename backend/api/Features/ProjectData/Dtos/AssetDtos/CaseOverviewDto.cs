using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.ProjectData.Dtos.AssetDtos;

public class CaseOverviewDto
{
    [Required] public required Guid CaseId { get; set; }
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required bool Archived { get; set; }
    [Required] public required bool ReferenceCase { get; set; }
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
    [Required] public required double AverageCo2Intensity { get; set; }

    [Required] public required DateTime DGADate { get; set; }
    [Required] public required DateTime DGBDate { get; set; }
    [Required] public required DateTime DGCDate { get; set; }
    [Required] public required DateTime APBODate { get; set; }
    [Required] public required DateTime BORDate { get; set; }
    [Required] public required DateTime VPBODate { get; set; }

    [Required] public required DateTime DG0Date { get; set; }
    [Required] public required DateTime DG1Date { get; set; }
    [Required] public required DateTime DG2Date { get; set; }
    [Required] public required DateTime DG3Date { get; set; }
    [Required] public required DateTime DG4Date { get; set; }
    [Required] public required DateTime CreatedUtc { get; set; }
    [Required] public required DateTime UpdatedUtc { get; set; }
    [Required] public required Guid SurfId { get; set; }
    [Required] public required Guid SubstructureId { get; set; }
    [Required] public required Guid TopsideId { get; set; }
    [Required] public required Guid TransportId { get; set; }
    [Required] public required Guid OnshorePowerSupplyId { get; set; }
    public required string? SharepointFileId { get; set; }
    public required string? SharepointFileName { get; set; }
    public required string? SharepointFileUrl { get; set; }
}
