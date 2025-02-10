using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies;

public class UpdateOnshorePowerSupplyController(UpdateOnshorePowerSupplyService updateOnshorePowerSupplyService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/onshore-power-supplies/{onshorePowerSupplyId:guid}")]
    public async Task<NoContentResult> UpdateOnshorePowerSupply(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] UpdateOnshorePowerSupplyDto dto)
    {
        await updateOnshorePowerSupplyService.UpdateOnshorePowerSupply(projectId, caseId, dto);
        return NoContent();
    }
}
