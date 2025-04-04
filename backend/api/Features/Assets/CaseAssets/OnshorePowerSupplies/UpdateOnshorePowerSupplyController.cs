using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies;

public class UpdateOnshorePowerSupplyController(UpdateOnshorePowerSupplyService updateOnshorePowerSupplyService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/onshore-power-supply")]
    public async Task<NoContentResult> UpdateOnshorePowerSupply(Guid projectId, Guid caseId, [FromBody] UpdateOnshorePowerSupplyDto dto)
    {
        await updateOnshorePowerSupplyService.UpdateOnshorePowerSupply(projectId, caseId, dto);

        return NoContent();
    }
}
