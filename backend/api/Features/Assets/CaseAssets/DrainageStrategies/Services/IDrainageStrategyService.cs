using System.Linq.Expressions;

using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Models;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Services;

public interface IDrainageStrategyService
{
    Task<DrainageStrategy> GetDrainageStrategyWithIncludes(
        Guid drainageStrategyId,
        params Expression<Func<DrainageStrategy, object>>[] includes
        );
    Task<DrainageStrategyDto> UpdateDrainageStrategy(Guid projectId, Guid caseId, Guid drainageStrategyId, UpdateDrainageStrategyDto updatedDrainageStrategyDto);
}
