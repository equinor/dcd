using api.Enums;
using api.Models;

namespace api.Repositories;

public interface IExplorationRepository : IBaseRepository
{
    Task<Exploration?> GetExploration(Guid explorationId);
    Task<bool> ExplorationHasProfile(Guid ExplorationId, ExplorationProfileNames profileType);
    Exploration UpdateExploration(Exploration exploration);
    Task<ExplorationWell?> GetExplorationWell(Guid explorationId, Guid wellId);
    ExplorationWell UpdateExplorationWell(ExplorationWell explorationWell);
}
