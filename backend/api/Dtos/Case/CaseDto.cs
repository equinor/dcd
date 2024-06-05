using System.ComponentModel.DataAnnotations;

using api.Dtos;
using api.Models;

namespace api.Dtos;

public class CaseDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public bool ReferenceCase { get; set; }
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    [Required]
    public int ProducerCount { get; set; }
    [Required]
    public int GasInjectorCount { get; set; }
    [Required]
    public int WaterInjectorCount { get; set; }
    [Required]
    public double FacilitiesAvailability { get; set; }
    [Required]
    public double CapexFactorFeasibilityStudies { get; set; }
    [Required]
    public double CapexFactorFEEDStudies { get; set; }
    [Required]
    public double NPV { get; set; }
    [Required]
    public double BreakEven { get; set; }
    public string? Host { get; set; }

    [Required]
    public DateTimeOffset DGADate { get; set; }
    [Required]
    public DateTimeOffset DGBDate { get; set; }
    [Required]
    public DateTimeOffset DGCDate { get; set; }
    [Required]
    public DateTimeOffset APXDate { get; set; }
    [Required]
    public DateTimeOffset APZDate { get; set; }
    [Required]
    public DateTimeOffset DG0Date { get; set; }
    [Required]
    public DateTimeOffset DG1Date { get; set; }
    [Required]
    public DateTimeOffset DG2Date { get; set; }
    [Required]
    public DateTimeOffset DG3Date { get; set; }
    [Required]
    public DateTimeOffset DG4Date { get; set; }
    [Required]
    public DateTimeOffset CreateTime { get; set; }
    [Required]
    public DateTimeOffset ModifyTime { get; set; }

    [Required]
    public Guid DrainageStrategyLink { get; set; }
    [Required]
    public Guid WellProjectLink { get; set; }
    [Required]
    public Guid SurfLink { get; set; }
    [Required]
    public Guid SubstructureLink { get; set; }
    [Required]
    public Guid TopsideLink { get; set; }
    [Required]
    public Guid TransportLink { get; set; }
    [Required]
    public Guid ExplorationLink { get; set; }

    [Required]
    public double Capex { get; set; }
    public CapexYear? CapexYear { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
}
