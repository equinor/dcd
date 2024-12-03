using System.Linq.Expressions;

using api.Features.CaseProfiles.Enums;
using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Explorations.Repositories;

public interface IExplorationRepository : IBaseRepository
{
    Task<Exploration?> GetExploration(Guid explorationId);
    Task<Exploration?> GetExplorationWithIncludes(Guid caseId, params Expression<Func<Exploration, object>>[] includes);
    Task<Well?> GetWell(Guid wellId);
    Task<bool> ExplorationHasProfile(Guid explorationId, ExplorationProfileNames profileType);
    Task<Exploration?> GetExplorationWithDrillingSchedule(Guid drillingScheduleId);
    ExplorationWell CreateExplorationWellDrillingSchedule(ExplorationWell explorationWellWithDrillingSchedule);
}
