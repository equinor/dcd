using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class CreateProjectDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public Guid CommonLibraryId { get; set; }
    [Required]
    public Guid FusionProjectId { get; set; }
    [Required]
    public Guid ReferenceCaseId { get; set; }
    [Required]
    public string CommonLibraryName { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public string Country { get; set; } = null!;
    [Required]
    public Currency Currency { get; set; }
    [Required]
    public PhysUnit PhysUnit { get; set; }
    [Required]
    public DateTimeOffset CreateDate { get; set; }
    [Required]
    public ProjectPhase ProjectPhase { get; set; }
    [Required]
    public ProjectCategory ProjectCategory { get; set; }
    public string? SharepointSiteUrl { get; set; }
    [Required]
    public double CO2RemovedFromGas { get; set; }
    [Required]
    public double CO2EmissionFromFuelGas { get; set; }
    [Required]
    public double FlaredGasPerProducedVolume { get; set; }
    [Required]
    public double CO2EmissionsFromFlaredGas { get; set; }
    [Required]
    public double CO2Vented { get; set; }
    [Required]
    public double DailyEmissionFromDrillingRig { get; set; }
    [Required]
    public double AverageDevelopmentDrillingDays { get; set; }
}
