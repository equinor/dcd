using System.ComponentModel.DataAnnotations;

using api.Dtos;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.ProjectMembers.Get;
using api.Features.Revisions.Get;
using api.Features.Wells.Get;

namespace api.Features.Projects.GetWithAssets;

public class ProjectWithAssetsDto : ProjectDto, IEquatable<ProjectWithAssetsDto>
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
    [Required] public List<DrainageStrategyWithProfilesDto> DrainageStrategies { get; set; } = [];
    [Required] public List<WellProjectWithProfilesDto>? WellProjects { get; set; } = [];
    [Required] public List<ProjectMemberDto> ProjectMembers { get; set; } = [];
    [Required] public DateTimeOffset ModifyTime { get; set; }
    [Required] public List<RevisionDetailsDto> RevisionsDetailsList { get; set; } = [];

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
