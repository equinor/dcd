using api.Enums;
using api.Models;

namespace api.Repositories;

public interface IDrainageStrategyRepository : IBaseRepository
{
    Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId);
    Task<bool> DrainageStrategyHasProfile(Guid drainageStrategyId, DrainageStrategyProfileNames profileType);
    DrainageStrategy UpdateDrainageStrategy(DrainageStrategy drainageStrategy);
}
