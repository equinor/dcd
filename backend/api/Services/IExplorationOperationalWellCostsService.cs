using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IExplorationOperationalWellCostsService
    {
        Task<ExplorationOperationalWellCostsDto?> UpdateOperationalWellCostsAsync(ExplorationOperationalWellCostsDto dto);
        Task<ExplorationOperationalWellCostsDto> CreateOperationalWellCostsAsync(ExplorationOperationalWellCostsDto dto);
        Task<ExplorationOperationalWellCosts?> GetOperationalWellCostsByProjectIdAsync(Guid id);
        Task<ExplorationOperationalWellCosts?> GetOperationalWellCostsAsync(Guid id);
    }
}
