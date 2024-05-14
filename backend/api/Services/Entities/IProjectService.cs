using api.Dtos;
using api.Models;

namespace api.Services;

public interface IProjectService
{
    Task<ProjectDto> UpdateProject(Guid projectId, UpdateProjectDto projectDto);
    Task<ProjectDto> UpdateProjectFromProjectMaster(ProjectDto projectDto);
    Task<ProjectDto> CreateProject(Project project);
    Task<IEnumerable<Project>> GetAll();
    Task<IEnumerable<ProjectDto>> GetAllDtos();
    Task<Project> GetProjectWithoutAssets(Guid projectId);
    Task<Project> GetProjectWithoutAssetsNoTracking(Guid projectId);
    Task<Project> GetProject(Guid projectId);
    Task<ProjectDto> GetProjectDto(Guid projectId);
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
