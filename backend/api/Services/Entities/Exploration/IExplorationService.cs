using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationService
{
    Task<Exploration> CreateExploration(Guid projectId, Guid sourceCaseId, CreateExplorationDto explorationDto);
    Task<Exploration> GetExploration(Guid explorationId);

    Task<ExplorationDto> UpdateExploration(
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto
    );

    Task<ExplorationWellDto> UpdateExplorationWell(
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        UpdateExplorationWellDto updatedExplorationWellDto
    );
}
