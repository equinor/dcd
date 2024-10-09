using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class ProjectWithAssetsDto : ProjectDto, IEquatable<ProjectWithAssetsDto>
{
    [Required]
    public ExplorationOperationalWellCostsDto ExplorationOperationalWellCosts { get; set; } = new ExplorationOperationalWellCostsDto();
    [Required]
    public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCosts { get; set; } = new DevelopmentOperationalWellCostsDto();
    [Required]
    public ICollection<CaseWithProfilesDto> Cases { get; set; } = [];
    [Required]
    public ICollection<ProjectDto> Revisions { get; set; } = [];
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
    [Required]
    public DateTimeOffset ModifyTime { get; set; }

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
