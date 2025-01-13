using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Services;

public interface IDrainageStrategyService
{
    Task<DrainageStrategyDto> UpdateDrainageStrategy(Guid projectId, Guid caseId, Guid drainageStrategyId, UpdateDrainageStrategyDto updatedDrainageStrategyDto);
}
