using System.Linq.Expressions;

using api.Features.CaseProfiles.Enums;
using api.Models;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Repositories;

public interface IDrainageStrategyRepository
{
    Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId);
    Task<DrainageStrategy?> GetDrainageStrategyWithIncludes(Guid drainageStrategyId, params Expression<Func<DrainageStrategy, object>>[] includes);
    Task<bool> DrainageStrategyHasProfile(Guid drainageStrategyId, DrainageStrategyProfileNames profileType);
}
