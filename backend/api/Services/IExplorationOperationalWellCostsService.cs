using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IExplorationOperationalWellCostsService
    {
        Task<ExplorationOperationalWellCosts?> GetOperationalWellCosts(Guid id);
    }
}
