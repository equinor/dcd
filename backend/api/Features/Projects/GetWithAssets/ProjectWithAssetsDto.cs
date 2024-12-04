using System.ComponentModel.DataAnnotations;

using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.CaseProfiles.Dtos;
using api.Features.ProjectMembers.Get;
using api.Features.Revisions.Get;
using api.Features.Wells.Get;
using api.Models;

namespace api.Features.Projects.GetWithAssets;

public class ProjectWithAssetsDto : IEquatable<ProjectWithAssetsDto>
{
    [Required] public ExplorationOperationalWellCostsDto ExplorationOperationalWellCosts { get; set; } = new();
    [Required] public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCosts { get; set; } = new();
    [Required] public List<CaseWithProfilesDto> Cases { get; set; } = [];
    [Required] public List<WellDto> Wells { get; set; } = [];
    [Required] public List<ExplorationWithProfilesDto> Explorations { get; set; } = [];
    [Required] public List<SurfWithProfilesDto> Surfs { get; set; } = [];
    [Required] public List<SubstructureWithProfilesDto> Substructures { get; set; } = [];
    [Required] public List<TopsideWithProfilesDto> Topsides { get; set; } = [];
    [Required] public List<TransportWithProfilesDto> Transports { get; set; } = [];
    [Required] public List<OnshorePowerSupplyWithProfilesDto> OnshorePowerSupplies { get; set; } = [];
    [Required] public List<DrainageStrategyWithProfilesDto> DrainageStrategies { get; set; } = [];
    [Required] public List<WellProjectWithProfilesDto>? WellProjects { get; set; } = [];
    [Required] public List<ProjectMemberDto> ProjectMembers { get; set; } = [];
    [Required] public DateTimeOffset ModifyTime { get; set; }
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
    [Required] public DateTimeOffset CreateDate { get; set; }
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

    public bool Equals(ProjectWithAssetsDto? projectDto)
    {
        return Name == projectDto?.Name &&
               CommonLibraryName == projectDto?.CommonLibraryName &&
               FusionProjectId == projectDto?.FusionProjectId &&
               Country == projectDto?.Country &&
               Id == projectDto?.Id &&
               ProjectCategory == projectDto?.ProjectCategory &&
               ProjectPhase == projectDto?.ProjectPhase &&
               InternalProjectPhase == projectDto?.InternalProjectPhase;
    }
}
