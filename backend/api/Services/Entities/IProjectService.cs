using api.Dtos;
using api.Models;

namespace api.Services;

public interface IProjectService
{
    Task<ProjectWithCasesDto> UpdateProject(Guid projectId, UpdateProjectDto projectDto);
    Task<Project> GetProject(Guid projectId);
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
}
