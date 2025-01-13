using api.Features.CaseProfiles.Enums;
using api.Models;

namespace api.Features.Assets.CaseAssets.Explorations.Repositories;

public interface IExplorationRepository
{
    Task<Exploration?> GetExploration(Guid explorationId);
    Task<Well?> GetWell(Guid wellId);
    Task<bool> ExplorationHasProfile(Guid explorationId, ExplorationProfileNames profileType);
    Task<Exploration?> GetExplorationWithDrillingSchedule(Guid drillingScheduleId);
    ExplorationWell CreateExplorationWellDrillingSchedule(ExplorationWell explorationWellWithDrillingSchedule);
}
