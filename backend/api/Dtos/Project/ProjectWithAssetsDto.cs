using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class ProjectWithAssetsDto
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
    public ExplorationOperationalWellCostsDto ExplorationOperationalWellCosts { get; set; } = new ExplorationOperationalWellCostsDto();
    [Required]
    public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCosts { get; set; } = new DevelopmentOperationalWellCostsDto();
    [Required]
    public ICollection<CaseWithProfilesDto> Cases { get; set; } = [];
    [Required]
    public ICollection<WellDto> Wells { get; set; } = [];
    [Required]
    public ICollection<ExplorationWithProfilesDto> Explorations { get; set; } = [];
    [Required]
    public ICollection<SurfWithProfilesDto> Surfs { get; set; } = [];
    [Required]
    public ICollection<SubstructureWithProfilesDto> Substructures { get; set; } = [];
    [Required]
    public ICollection<TopsideWithProfilesDto> Topsides { get; set; } = [];
    [Required]
    public ICollection<TransportWithProfilesDto> Transports { get; set; } = [];
    [Required]
    public ICollection<DrainageStrategyWithProfilesDto> DrainageStrategies { get; set; } = [];
    [Required]
    public ICollection<WellProjectWithProfilesDto>? WellProjects { get; set; } = [];
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

    public bool Equals(ProjectWithAssetsDto projectDto)
    {
        return Name == projectDto.Name &&
               CommonLibraryName == projectDto.CommonLibraryName &&
               FusionProjectId == projectDto.FusionProjectId &&
               Country == projectDto.Country && Id == projectDto.Id &&
               ProjectCategory == projectDto.ProjectCategory &&
               ProjectPhase == projectDto.ProjectPhase;
    }
}
