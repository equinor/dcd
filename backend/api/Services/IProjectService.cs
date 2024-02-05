using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IProjectService
    {
        ProjectDto CreateProject(Project project);
        IEnumerable<Project> GetAll();
        IEnumerable<ProjectDto> GetAllDtos();
        Project GetProject(Guid projectId);
        Task<Project> GetProjectAsync(Guid projectId);
        ProjectDto GetProjectDto(Guid projectId);
        Task<ProjectDto> GetProjectDtoAsync(Guid projectId);
        Project GetProjectWithoutAssets(Guid projectId);
        Project GetProjectWithoutAssetsNoTracking(Guid projectId);
        ProjectDto SetReferenceCase(ProjectDto projectDto);
        ProjectDto UpdateProject(ProjectDto projectDto);
        void UpdateProjectFromProjectMaster();
        ProjectDto UpdateProjectFromProjectMaster(ProjectDto projectDto);
    }
}
