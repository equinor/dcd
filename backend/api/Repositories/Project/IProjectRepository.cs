using api.Models;

namespace api.Repositories;

public interface IProjectRepository : IBaseRepository
{
    Task<Project?> GetProject(Guid projectId);
    Project UpdateProject(Project updatedProject);
}
