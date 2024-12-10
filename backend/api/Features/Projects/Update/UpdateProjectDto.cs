using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Projects.Update;

public class UpdateProjectDto
{
    [Required] public required string Name { get; set; }
    [Required] public required Guid ReferenceCaseId { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required string Country { get; set; }
    [Required] public required Currency Currency { get; set; }
    [Required] public required PhysUnit PhysicalUnit { get; set; }
    [Required] public required ProjectClassification Classification { get; set; }
    [Required] public required ProjectPhase ProjectPhase { get; set; }
    [Required] public required InternalProjectPhase InternalProjectPhase { get; set; }
    [Required] public required ProjectCategory ProjectCategory { get; set; }
    public required string? SharepointSiteUrl { get; set; }
    [Required] public required double CO2RemovedFromGas { get; set; }
    [Required] public required double CO2EmissionFromFuelGas { get; set; }
    [Required] public required double FlaredGasPerProducedVolume { get; set; }
    [Required] public required double CO2EmissionsFromFlaredGas { get; set; }
    [Required] public required double CO2Vented { get; set; }
    [Required] public required double DailyEmissionFromDrillingRig { get; set; }
    [Required] public required double AverageDevelopmentDrillingDays { get; set; }
    [Required] public required double OilPriceUSD { get; set; }
    [Required] public required double GasPriceNOK { get; set; }
    [Required] public required double DiscountRate { get; set; }
    [Required] public required double ExchangeRateUSDToNOK { get; set; }
}
