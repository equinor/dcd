using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class ProjectWithCasesDto
{
    [Required]
    public ProjectClassification Classification { get; set; }
    [Required]
    public Guid Id { get; set; }
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
    public PhysUnit PhysicalUnit { get; set; }
    [Required]
    public DateTimeOffset CreateDate { get; set; }
    [Required]
    public ProjectPhase ProjectPhase { get; set; }
    [Required]
    public ProjectCategory ProjectCategory { get; set; }
    [Required]
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
    [Required]
    public ICollection<CaseDto> Cases { get; set; } = [];
    [Required]
    public double OilPriceUSD { get; set; }
    [Required]
    public double GasPriceNOK { get; set; }
    [Required]
    public double DiscountRate { get; set; }
    [Required]
    public double ExchangeRateUSDToNOK { get; set; }
}
