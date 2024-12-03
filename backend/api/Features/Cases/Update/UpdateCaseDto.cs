using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Cases.Update;

public class UpdateCaseDto
{
    [Required] public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool? ReferenceCase { get; set; }
    public bool Archived { get; set; }
    public ArtificialLift? ArtificialLift { get; set; }
    public ProductionStrategyOverview? ProductionStrategyOverview { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public double FacilitiesAvailability { get; set; }
    public double CapexFactorFeasibilityStudies { get; set; }
    public double CapexFactorFEEDStudies { get; set; }
    public double NPV { get; set; }
    public double? NPVOverride { get; set; }
    public double BreakEven { get; set; }
    public double? BreakEvenOverride { get; set; }
    public string? Host { get; set; }
    public DateTimeOffset DGADate { get; set; }
    public DateTimeOffset DGBDate { get; set; }
    public DateTimeOffset DGCDate { get; set; }
    public DateTimeOffset APBODate { get; set; }
    public DateTimeOffset BORDate { get; set; }
    public DateTimeOffset VPBODate { get; set; }
    public DateTimeOffset DG0Date { get; set; }
    public DateTimeOffset DG1Date { get; set; }
    public DateTimeOffset DG2Date { get; set; }
    public DateTimeOffset DG3Date { get; set; }
    public DateTimeOffset DG4Date { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
}
