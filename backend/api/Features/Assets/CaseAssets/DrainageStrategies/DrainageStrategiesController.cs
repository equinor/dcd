using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.DrainageStrategies.Services;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.DrainageStrategies;

[Route("projects/{projectId}/cases/{caseId}/drainage-strategies")]
[AuthorizeActionType(ActionType.Edit)]
public class DrainageStrategiesController(DrainageStrategyService drainageStrategyService)
    : ControllerBase
{
    [HttpPut("{drainageStrategyId}")]
    public async Task<DrainageStrategyDto> UpdateDrainageStrategy(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] UpdateDrainageStrategyDto dto)
    {
        return await drainageStrategyService.UpdateDrainageStrategy(projectId, caseId, drainageStrategyId, dto);
    }
}
