using api.Models;

namespace api.Repositories;

public interface IProjectRepository : IBaseRepository
{
    Task<Project?> GetProject(Guid projectId);
    Task<ExplorationOperationalWellCosts?> GetExplorationOperationalWellCosts(Guid id);
    Task<DevelopmentOperationalWellCosts?> GetDevelopmentOperationalWellCosts(Guid id);
    Task UpdateModifyTime(Guid projectId);
}
