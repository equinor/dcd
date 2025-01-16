using api.AppInfrastructure.ControllerAttributes;
using api.Features.Cases.GetWithAssets;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.DrainageStrategies;

public class UpdateDrainageStrategyController(UpdateDrainageStrategyService updateDrainageStrategyService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}")]
    public async Task<DrainageStrategyDto> UpdateDrainageStrategy(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] UpdateDrainageStrategyDto dto)
    {
        return await updateDrainageStrategyService.UpdateDrainageStrategy(projectId, caseId, drainageStrategyId, dto);
    }
}
