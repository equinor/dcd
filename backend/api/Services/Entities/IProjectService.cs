using api.Dtos;
using api.Models;

namespace api.Services;

public interface IProjectService
{
    Task<ProjectWithCasesDto> UpdateProject(Guid projectId, UpdateProjectDto projectDto);
    Task<ProjectWithAssetsDto> CreateProject(Project project);
    Task<Project> GetProject(Guid projectId);
    Task<Project> GetProjectWithoutAssets(Guid projectId);
    Task<Project> GetProjectWithoutAssetsNoTracking(Guid projectId);
    Task<Project> GetProjectWithCasesAndAssets(Guid projectId);
    Task<ProjectWithAssetsDto> GetProjectDto(Guid projectId);
    Task UpdateProjectFromProjectMaster();
    Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts(
        Guid projectId,
        Guid explorationOperationalWellCostsId,
        UpdateExplorationOperationalWellCostsDto dto
    );
    Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts(
        Guid projectId,
        Guid developmentOperationalWellCostsId,
        UpdateDevelopmentOperationalWellCostsDto dto
    );

    Task<ProjectMember> AddProjectMember(Guid projectId, Guid personId);
}
