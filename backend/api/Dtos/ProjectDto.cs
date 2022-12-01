using api.Models;

namespace api.Dtos;

public class ProjectDto
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = null!;
    public Guid CommonLibraryId { get; set; }
    public Guid FusionProjectId { get; set; }
    public string CommonLibraryName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Country { get; set; } = null!;
    public Currency Currency { get; set; }
    public PhysUnit PhysUnit { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public ProjectPhase ProjectPhase { get; set; }
    public ProjectCategory ProjectCategory { get; set; }
    public ExplorationOperationalWellCostsDto? ExplorationOperationalWellCosts { get; set; }
    public DevelopmentOperationalWellCostsDto? DevelopmentOperationalWellCosts { get; set; }
    public ICollection<CaseDto>? Cases { get; set; }
    public ICollection<WellDto>? Wells { get; set; }
    public ICollection<ExplorationDto>? Explorations { get; set; }
    public ICollection<SurfDto>? Surfs { get; set; }
    public ICollection<SubstructureDto>? Substructures { get; set; }
    public ICollection<TopsideDto>? Topsides { get; set; }
    public ICollection<TransportDto>? Transports { get; set; }
    public ICollection<DrainageStrategyDto>? DrainageStrategies { get; set; }
    public ICollection<WellProjectDto>? WellProjects { get; set; }
    public string? SharepointSiteUrl { get; set; }
    public double CO2RemovedFromGas { get; set; }
    public double CO2EmissionFromFuelGas { get; set; }
    public double FlaredGasPerProducedVolume { get; set; }
    public double CO2EmissionsFromFlaredGas { get; set; }
    public double CO2Vented { get; set; }
    public double DailyEmissionFromDrillingRig { get; set; }
    public double AverageDevelopmentDrillingDays { get; set; }
    public bool HasChanges { get; set; }

    public bool Equals(ProjectDto projectDto)
    {
        return (Name == projectDto.Name) && (Description == projectDto.Description) &&
               (CommonLibraryName == projectDto.CommonLibraryName) && (FusionProjectId == projectDto.FusionProjectId) &&
               (Country == projectDto.Country) && (ProjectId == projectDto.ProjectId) &&
               (ProjectCategory == projectDto.ProjectCategory) && (ProjectPhase == projectDto.ProjectPhase);

    }
}
