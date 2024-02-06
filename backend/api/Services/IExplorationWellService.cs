using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IExplorationWellService
    {
        Task<ProjectDto> CreateExplorationWell(ExplorationWellDto explorationWellDto);
        Task<ProjectDto> UpdateExplorationWell(ExplorationWellDto updatedExplorationWellDto);
        Task<ExplorationWellDto[]?> UpdateMultpleExplorationWells(ExplorationWellDto[] updatedExplorationWellDtos, Guid caseId);
        Task<ExplorationWellDto[]?> CreateMultipleExplorationWells(ExplorationWellDto[] explorationWellDtos);
        Task<ExplorationWell> GetExplorationWell(Guid wellId, Guid caseId);
        Task<ExplorationWellDto[]?> CopyExplorationWell(Guid sourceExplorationId, Guid targetExplorationId);
        Task<ExplorationWellDto> GetExplorationWellDto(Guid wellId, Guid caseId);
        Task<IEnumerable<ExplorationWell>> GetAll();
        Task<IEnumerable<ExplorationWellDto>> GetAllDtos();
    }
}
