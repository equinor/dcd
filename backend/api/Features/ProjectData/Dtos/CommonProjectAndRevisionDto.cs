using System.ComponentModel.DataAnnotations;

using api.Dtos;
using api.Features.Wells.Get;
using api.Models;

namespace api.Features.ProjectData.Dtos;

public class CommonProjectAndRevisionDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required DateTimeOffset ModifyTime { get; set; }
    [Required] public required ProjectClassification Classification { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required Guid CommonLibraryId { get; set; }
    [Required] public required Guid FusionProjectId { get; set; }
    [Required] public required Guid ReferenceCaseId { get; set; }
    [Required] public required string CommonLibraryName { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required string Country { get; set; }
    [Required] public required Currency Currency { get; set; }
    [Required] public required PhysUnit PhysicalUnit { get; set; }
    [Required] public required DateTimeOffset CreateDate { get; set; }
    [Required] public required ProjectPhase ProjectPhase { get; set; }
    [Required] public required InternalProjectPhase InternalProjectPhase { get; set; }
    [Required] public required ProjectCategory ProjectCategory { get; set; }
    public string? SharepointSiteUrl { get; set; }
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

    [Required] public required ExplorationOperationalWellCostsDto ExplorationOperationalWellCosts { get; set; }
    [Required] public required DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCosts { get; set; }
    [Required] public required List<CaseWithProfilesDto> Cases { get; set; }
    [Required] public required List<WellDto> Wells { get; set; }
    [Required] public required List<ExplorationWithProfilesDto> Explorations { get; set; }
    [Required] public required List<SurfWithProfilesDto> Surfs { get; set; }
    [Required] public required List<SubstructureWithProfilesDto> Substructures { get; set; }
    [Required] public required List<TopsideWithProfilesDto> Topsides { get; set; }
    [Required] public required List<TransportWithProfilesDto> Transports { get; set; }
    [Required] public required List<DrainageStrategyWithProfilesDto> DrainageStrategies { get; set; }
    [Required] public required List<WellProjectWithProfilesDto> WellProjects { get; set; }
}
