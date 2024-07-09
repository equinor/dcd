using api.Dtos;
using api.Models;

namespace api.Services;

public interface IProjectService
{
    Task<ProjectWithAssetsDto> UpdateProject(Guid projectId, UpdateProjectDto projectDto);
    Task<ProjectWithAssetsDto> UpdateProjectFromProjectMaster(ProjectWithAssetsDto projectDto);
    Task<ProjectWithAssetsDto> CreateProject(Project project);
    Task<IEnumerable<Project>> GetAll();
    Task<IEnumerable<ProjectWithAssetsDto>> GetAllDtos();
    Task<Project> GetProjectWithoutAssets(Guid projectId);
    Task<Project> GetProjectWithoutAssetsNoTracking(Guid projectId);
    Task<Project> GetProject(Guid projectId);
    Task<ProjectWithAssetsDto> GetProjectDto(Guid projectId);
    Task UpdateProjectFromProjectMaster();
    Task<IEnumerable<Well>> GetWells(Guid projectId);
    Task<IEnumerable<Exploration>> GetExplorations(Guid projectId);
    Task<IEnumerable<Transport>> GetTransports(Guid projectId);
    Task<IEnumerable<Topside>> GetTopsides(Guid projectId);
    Task<IEnumerable<Surf>> GetSurfs(Guid projectId);
    Task<IEnumerable<DrainageStrategy>> GetDrainageStrategies(Guid projectId);
    Task<IEnumerable<WellProject>> GetWellProjects(Guid projectId);
    Task<IEnumerable<Substructure>> GetSubstructures(Guid projectId);
}
