using System.ComponentModel.DataAnnotations;

using api.Features.ProjectData.Dtos.AssetDtos;
using api.Models.Enums;

namespace api.Features.ProjectData.Dtos;

public class CommonProjectAndRevisionDto
{
    [Required] public required DateTime UpdatedUtc { get; set; }
    [Required] public required ProjectClassification Classification { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required Guid FusionProjectId { get; set; }
    [Required] public required Guid ReferenceCaseId { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required string Country { get; set; }
    [Required] public required Currency Currency { get; set; }
    [Required] public required PhysUnit PhysicalUnit { get; set; }
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
    [Required] public required double NglPriceUsd { get; set; }
    [Required] public required double DiscountRate { get; set; }
    [Required] public required double ExchangeRateUSDToNOK { get; set; }
    [Required] public required int NpvYear { get; set; }

    [Required] public required ExplorationOperationalWellCostsOverviewDto ExplorationOperationalWellCosts { get; set; }
    [Required] public required DevelopmentOperationalWellCostsOverviewDto DevelopmentOperationalWellCosts { get; set; }
    [Required] public required List<CaseOverviewDto> Cases { get; set; }
    [Required] public required List<WellOverviewDto> Wells { get; set; }
}
