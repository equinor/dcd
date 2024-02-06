using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IExplorationOperationalWellCostsService
    {
        Task<ExplorationOperationalWellCostsDto?> UpdateOperationalWellCosts(ExplorationOperationalWellCostsDto dto);
        Task<ExplorationOperationalWellCostsDto> CreateOperationalWellCosts(ExplorationOperationalWellCostsDto dto);
        Task<ExplorationOperationalWellCosts?> GetOperationalWellCostsByProjectId(Guid id);
        Task<ExplorationOperationalWellCosts?> GetOperationalWellCosts(Guid id);
    }
}
