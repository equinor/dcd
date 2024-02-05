using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IExplorationService
    {
        Task<ExplorationDto> CopyExploration(Guid explorationId, Guid sourceCaseId);
        Task<ProjectDto> CreateExploration(ExplorationDto explorationDto, Guid sourceCaseId);
        Task<Exploration> NewCreateExploration(ExplorationDto explorationDto, Guid sourceCaseId);
        Task<ProjectDto> DeleteExploration(Guid explorationId);
        Task<ProjectDto> UpdateExploration(ExplorationDto updatedExplorationDto);
        Task<ExplorationDto> NewUpdateExploration(ExplorationDto updatedExplorationDto);
        Task<ExplorationDto[]> UpdateMultiple(ExplorationDto[] updatedExplorationDtos);
        Task<ExplorationDto> UpdateSingleExploration(ExplorationDto updatedExplorationDto);
        Task<Exploration> GetExploration(Guid explorationId);
    }
}
