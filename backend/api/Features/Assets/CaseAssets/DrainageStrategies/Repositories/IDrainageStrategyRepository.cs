using api.Features.CaseProfiles.Enums;
using api.Models;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Repositories;

public interface IDrainageStrategyRepository
{
    Task<DrainageStrategy?> GetDrainageStrategy(Guid drainageStrategyId);
    Task<bool> DrainageStrategyHasProfile(Guid drainageStrategyId, DrainageStrategyProfileNames profileType);
}
