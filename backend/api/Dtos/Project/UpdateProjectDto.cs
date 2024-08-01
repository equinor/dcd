using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateProjectDto
{
    public string Name { get; set; } = null!;
    public Guid ReferenceCaseId { get; set; }
    public string CommonLibraryName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Country { get; set; } = null!;
    public Currency Currency { get; set; }
    public PhysUnit PhysicalUnit { get; set; }
    public ProjectClassification Classification { get; set; }
    public ProjectPhase ProjectPhase { get; set; }
    public ProjectCategory ProjectCategory { get; set; }
    public string? SharepointSiteUrl { get; set; }
    public double CO2RemovedFromGas { get; set; }
    public double CO2EmissionFromFuelGas { get; set; }
    public double FlaredGasPerProducedVolume { get; set; }
    public double CO2EmissionsFromFlaredGas { get; set; }
    public double CO2Vented { get; set; }
    public double DailyEmissionFromDrillingRig { get; set; }
    public double AverageDevelopmentDrillingDays { get; set; }
}
