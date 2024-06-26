using api.Enums;
using api.Models;

namespace api.Repositories;

public interface IExplorationRepository : IBaseRepository
{
    Task<Exploration?> GetExploration(Guid explorationId);
    Task<Well?> GetWell(Guid wellId);
    Task<bool> ExplorationHasProfile(Guid ExplorationId, ExplorationProfileNames profileType);
    Exploration UpdateExploration(Exploration exploration);
    Task<DrillingSchedule?> GetExplorationWellDrillingSchedule(Guid drillingScheduleId);
    DrillingSchedule UpdateExplorationWellDrillingSchedule(DrillingSchedule drillingSchedule);
    ExplorationWell CreateExplorationWellDrillingSchedule(ExplorationWell explorationWellWithDrillingSchedule);
}
