using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationService
{
    Task<ExplorationDto> CopyExploration(Guid explorationId, Guid sourceCaseId);
    Task<Exploration> CreateExploration(Guid projectId, Guid sourceCaseId, CreateExplorationDto explorationDto);
    Task<ExplorationDto> UpdateExploration(ExplorationDto updatedExplorationDto);
    Task<Exploration> GetExploration(Guid explorationId);
}
