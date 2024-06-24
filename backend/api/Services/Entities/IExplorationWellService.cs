using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationWellService
{
    Task<List<ExplorationWell>> GetExplorationWellsForExploration(Guid explorationId);
}
