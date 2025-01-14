using System.ComponentModel.DataAnnotations;

using api.Features.Assets.CaseAssets.DrainageStrategies.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.ProjectData.Dtos;
using api.Features.Wells.Get;
using api.Models;

namespace api.Features.Stea.Dtos;

public class ProjectWithAssetsWrapperDto
{
    public required ProjectWithAssetsDto Project { get; set; }
    public required List<DrainageStrategyWithProfilesDto> DrainageStrategies { get; set; }
    public required List<ExplorationWithProfilesDto> Explorations { get; set; }
    public required List<OnshorePowerSupplyWithProfilesDto> OnshorePowerSupplies { get; set; }
    public required List<SubstructureWithProfilesDto> Substructures { get; set; }
    public required List<SurfWithProfilesDto> Surfs { get; set; }
    public required List<TopsideWithProfilesDto> Topsides { get; set; }
    public required List<TransportWithProfilesDto> Transports { get; set; }
    public required List<WellDto> Wells { get; set; }
    public required List<WellProjectWithProfilesDto>? WellProjects { get; set; }
}

public class ProjectWithAssetsDto
{
    [Required] public ExplorationOperationalWellCostsDto ExplorationOperationalWellCosts { get; set; } = new();
    [Required] public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCosts { get; set; } = new();
    [Required] public List<CaseWithProfilesDto> Cases { get; set; } = [];
    [Required] public DateTime ModifyTime { get; set; }
    [Required] public List<RevisionDetailsDto> RevisionsDetailsList { get; set; } = [];

    [Required] public ProjectClassification Classification { get; set; }
    [Required] public Guid Id { get; set; }
    [Required] public bool IsRevision { get; set; }
    [Required] public Guid? OriginalProjectId { get; set; }
    [Required] public string Name { get; set; } = null!;
    [Required] public Guid CommonLibraryId { get; set; }
    [Required] public Guid FusionProjectId { get; set; }
    [Required] public Guid ReferenceCaseId { get; set; }
    [Required] public string CommonLibraryName { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public string Country { get; set; } = null!;
    [Required] public Currency Currency { get; set; }
    [Required] public PhysUnit PhysicalUnit { get; set; }
    [Required] public DateTime CreateDate { get; set; }
    [Required] public ProjectPhase ProjectPhase { get; set; }
    [Required] public InternalProjectPhase InternalProjectPhase { get; set; }
    [Required] public ProjectCategory ProjectCategory { get; set; }
    public string? SharepointSiteUrl { get; set; }
    [Required] public double CO2RemovedFromGas { get; set; }
    [Required] public double CO2EmissionFromFuelGas { get; set; }
    [Required] public double FlaredGasPerProducedVolume { get; set; }
    [Required] public double CO2EmissionsFromFlaredGas { get; set; }
    [Required] public double CO2Vented { get; set; }
    [Required] public double DailyEmissionFromDrillingRig { get; set; }
    [Required] public double AverageDevelopmentDrillingDays { get; set; }
    [Required] public double OilPriceUSD { get; set; }
    [Required] public double GasPriceNOK { get; set; }
    [Required] public double DiscountRate { get; set; }
    [Required] public double ExchangeRateUSDToNOK { get; set; }
}
