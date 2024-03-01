using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationWellService
{
    Task<ProjectDto> CreateExplorationWell(CreateExplorationWellDto explorationWellDto);
    Task<ExplorationWellDto[]?> CreateMultipleExplorationWells(CreateExplorationWellDto[] explorationWellDtos);
    Task<ExplorationWell> GetExplorationWell(Guid wellId, Guid caseId);
    Task<List<ExplorationWell>> GetExplorationWellsForExploration(Guid explorationId);
    Task<ExplorationWellDto[]?> CopyExplorationWell(Guid sourceExplorationId, Guid targetExplorationId);
    Task<IEnumerable<ExplorationWell>> GetAll();
}
