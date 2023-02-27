using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IExplorationWellService
    {
        ExplorationWellDto[]? CopyExplorationWell(Guid sourceExplorationId, Guid targetExplorationId);
        ProjectDto CreateExplorationWell(ExplorationWellDto explorationWellDto);
        ExplorationWellDto[]? CreateMultipleExplorationWells(ExplorationWellDto[] explorationWellDtos);
        IEnumerable<ExplorationWell> GetAll();
        IEnumerable<ExplorationWellDto> GetAllDtos();
        ExplorationWell GetExplorationWell(Guid wellId, Guid caseId);
        ExplorationWellDto GetExplorationWellDto(Guid wellId, Guid caseId);
        ProjectDto UpdateExplorationWell(ExplorationWellDto updatedExplorationWellDto);
        ExplorationWellDto[]? UpdateMultpleExplorationWells(ExplorationWellDto[] updatedExplorationWellDtos, Guid caseId);
    }
}
