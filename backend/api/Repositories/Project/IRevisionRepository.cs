using api.Models;

namespace api.Repositories;

public interface IRevisionRepository : IBaseRepository
{
    Task<Project> AddRevision(Project project);
    Task<Project?> GetProjectAndAssetsNoTracking(Guid id);
}
