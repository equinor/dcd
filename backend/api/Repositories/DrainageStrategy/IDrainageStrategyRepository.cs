using System.Linq.Expressions;

using api.Enums;
using api.Models;

namespace api.Repositories;

public interface IDrainageStrategyRepository : IBaseRepository
{
    Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId);
    Task<DrainageStrategy?> GetDrainageStrategyWithIncludes(Guid drainageStrategyId, params Expression<Func<DrainageStrategy, object>>[] includes);
    Task<bool> DrainageStrategyHasProfile(Guid drainageStrategyId, DrainageStrategyProfileNames profileType);
}
