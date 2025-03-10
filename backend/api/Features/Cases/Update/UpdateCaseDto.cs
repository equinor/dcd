using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Cases.Update;

public class UpdateCaseDto
{
    [Required] public required string Name { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required bool Archived { get; set; }
    [Required] public required ArtificialLift ArtificialLift { get; set; }
    [Required] public required ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    [Required] public required int ProducerCount { get; set; }
    [Required] public required int GasInjectorCount { get; set; }
    [Required] public required int WaterInjectorCount { get; set; }
    [Required] public required double FacilitiesAvailability { get; set; }
    [Required] public required double CapexFactorFeasibilityStudies { get; set; }
    [Required] public required double CapexFactorFeedStudies { get; set; }
    [Required] public required double Npv { get; set; }
    public required double? NpvOverride { get; set; }
    [Required] public required double BreakEven { get; set; }
    public required double? BreakEvenOverride { get; set; }
    public required string? Host { get; set; }
    [Required] public required double AverageCo2Intensity { get; set; }
    [Required] public required double DiscountedCashflow { get; set; }
    public required DateTime? DGADate { get; set; }
    public required DateTime? DGBDate { get; set; }
    public required DateTime? DGCDate { get; set; }
    public required DateTime? APBODate { get; set; }
    public required DateTime? BORDate { get; set; }
    public required DateTime? VPBODate { get; set; }
    public required DateTime? DG0Date { get; set; }
    public required DateTime? DG1Date { get; set; }
    public required DateTime? DG2Date { get; set; }
    public required DateTime? DG3Date { get; set; }
    [Required] public required DateTime DG4Date { get; set; }
    public required string? SharepointFileId { get; set; }
    public required string? SharepointFileName { get; set; }
    public required string? SharepointFileUrl { get; set; }
}
