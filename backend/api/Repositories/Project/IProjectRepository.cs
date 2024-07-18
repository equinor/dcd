using api.Models;

namespace api.Repositories;

public interface IProjectRepository : IBaseRepository
{
    Task<Project?> GetProject(Guid projectId);
    Task<Project?> GetProjectWithCases(Guid projectId);
    Project UpdateProject(Project updatedProject);
    Task<ExplorationOperationalWellCosts?> GetExplorationOperationalWellCosts(Guid id);
    Task<DevelopmentOperationalWellCosts?> GetDevelopmentOperationalWellCosts(Guid id);
}
