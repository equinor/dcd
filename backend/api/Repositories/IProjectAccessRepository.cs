using api.Models;

namespace api.Repositories;

public interface IProjectAccessRepository
{
    Task<T?> Get<T>(Guid id) where T : class;
    Task<Project?> GetProjectByExternalId(Guid externalId);
    Task<Project?> GetProjectById(Guid id);
}