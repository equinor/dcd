using System.Linq.Expressions;

using api.Dtos;
using api.Models;

namespace api.Services;

public interface IDrainageStrategyService
{
    Task<DrainageStrategy> CreateDrainageStrategy(Guid projectId, Guid sourceCaseId, CreateDrainageStrategyDto drainageStrategyDto);
    Task<DrainageStrategy> GetDrainageStrategy(Guid drainageStrategyId);
    Task<DrainageStrategy> GetDrainageStrategyWithIncludes(
        Guid drainageStrategyId,
        params Expression<Func<DrainageStrategy, object>>[] includes
        );
    Task<DrainageStrategyDto> UpdateDrainageStrategy(Guid projectId, Guid caseId, Guid drainageStrategyId, UpdateDrainageStrategyDto updatedDrainageStrategyDto);
}
