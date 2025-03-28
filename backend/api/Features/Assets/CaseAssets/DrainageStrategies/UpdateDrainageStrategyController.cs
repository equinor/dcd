using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.DrainageStrategies;

public class UpdateDrainageStrategyController(UpdateDrainageStrategyService updateDrainageStrategyService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategy")]
    public async Task<NoContentResult> UpdateDrainageStrategy(Guid projectId, Guid caseId, [FromBody] UpdateDrainageStrategyDto dto)
    {
        await updateDrainageStrategyService.UpdateDrainageStrategy(projectId, caseId, dto);

        return NoContent();
    }
}
