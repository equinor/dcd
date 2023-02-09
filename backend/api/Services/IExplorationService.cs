using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IExplorationService
    {
        ExplorationDto CopyExploration(Guid explorationId, Guid sourceCaseId);
        ProjectDto CreateExploration(ExplorationDto explorationDto, Guid sourceCaseId);
        ProjectDto DeleteExploration(Guid explorationId);
        Exploration GetExploration(Guid explorationId);
        Exploration NewCreateExploration(ExplorationDto explorationDto, Guid sourceCaseId);
        ExplorationDto NewUpdateExploration(ExplorationDto updatedExplorationDto);
        ProjectDto UpdateExploration(ExplorationDto updatedExplorationDto);
        ExplorationDto[] UpdateMultiple(ExplorationDto[] updatedExplorationDtos);
        ExplorationDto UpdateSingleExploration(ExplorationDto updatedExplorationDto);
    }
}
