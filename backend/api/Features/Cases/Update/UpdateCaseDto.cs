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
    [Required] public required double InitialYearsWithoutWellInterventionCost { get; set; }
    [Required] public required double FinalYearsWithoutWellInterventionCost { get; set; }
    [Required] public required double Npv { get; set; }
    public required double? NpvOverride { get; set; }
    [Required] public required double BreakEven { get; set; }
    public required double? BreakEvenOverride { get; set; }
    public required string? Host { get; set; }
    [Required] public required double AverageCo2Intensity { get; set; }
    [Required] public required double DiscountedCashflow { get; set; }
    [Required] public required double Co2RemovedFromGas { get; set; }
    [Required] public required double Co2EmissionFromFuelGas { get; set; }
    [Required] public required double FlaredGasPerProducedVolume { get; set; }
    [Required] public required double Co2EmissionsFromFlaredGas { get; set; }
    [Required] public required double Co2Vented { get; set; }
    [Required] public required double DailyEmissionFromDrillingRig { get; set; }
    [Required] public required double AverageDevelopmentDrillingDays { get; set; }
    public required DateTime? DgaDate { get; set; }
    public required DateTime? DgbDate { get; set; }
    public required DateTime? DgcDate { get; set; }
    public required DateTime? ApboDate { get; set; }
    public required DateTime? BorDate { get; set; }
    public required DateTime? VpboDate { get; set; }
    public required DateTime? Dg0Date { get; set; }
    public required DateTime? Dg1Date { get; set; }
    public required DateTime? Dg2Date { get; set; }
    public required DateTime? Dg3Date { get; set; }
    [Required] public required DateTime Dg4Date { get; set; }
    public required string? SharepointFileId { get; set; }
    public required string? SharepointFileName { get; set; }
    public required string? SharepointFileUrl { get; set; }
}
