using api.Models;

namespace api.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetProject(Guid projectId);
}
