using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IExplorationOperationalWellCostsService
    {
        ExplorationOperationalWellCostsDto CreateOperationalWellCosts(ExplorationOperationalWellCostsDto dto);
        ExplorationOperationalWellCosts? GetOperationalWellCosts(Guid id);
        ExplorationOperationalWellCosts? GetOperationalWellCostsByProjectId(Guid id);
        ExplorationOperationalWellCostsDto? UpdateOperationalWellCosts(ExplorationOperationalWellCostsDto dto);
    }
}
