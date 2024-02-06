using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationService
{
    Task<ExplorationDto> CopyExploration(Guid explorationId, Guid sourceCaseId);
    Task<Exploration> NewCreateExploration(ExplorationDto explorationDto, Guid sourceCaseId);
    Task<ExplorationDto> NewUpdateExploration(ExplorationDto updatedExplorationDto);
    Task<ExplorationDto[]> UpdateMultiple(ExplorationDto[] updatedExplorationDtos);
    Task<ExplorationDto> UpdateSingleExploration(ExplorationDto updatedExplorationDto);
    Task<Exploration> GetExploration(Guid explorationId);
}