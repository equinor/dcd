using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class ProjectDto
{
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
    public ICollection<CaseDto> Cases { get; set; } = [];
    [Required]
    public ICollection<WellDto> Wells { get; set; } = [];
    [Required]
    public ICollection<ExplorationDto> Explorations { get; set; } = [];
    [Required]
    public ICollection<SurfDto> Surfs { get; set; } = [];
    [Required]
    public ICollection<SubstructureDto> Substructures { get; set; } = [];
    [Required]
    public ICollection<TopsideDto> Topsides { get; set; } = [];
    [Required]
    public ICollection<TransportDto> Transports { get; set; } = [];
    [Required]
    public ICollection<DrainageStrategyDto> DrainageStrategies { get; set; } = [];
    [Required]
    public ICollection<WellProjectDto>? WellProjects { get; set; } = [];
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
    public bool HasChanges { get; set; }

    public bool Equals(ProjectDto projectDto)
    {
        return Name == projectDto.Name &&
               CommonLibraryName == projectDto.CommonLibraryName && FusionProjectId == projectDto.FusionProjectId &&
               Country == projectDto.Country && Id == projectDto.Id &&
               ProjectCategory == projectDto.ProjectCategory && ProjectPhase == projectDto.ProjectPhase;
    }
}
