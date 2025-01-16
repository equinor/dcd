using api.AppInfrastructure.ControllerAttributes;
using api.Features.Cases.GetWithAssets;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies;

public class UpdateOnshorePowerSupplyController(UpdateOnshorePowerSupplyService updateOnshorePowerSupplyService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/onshore-power-supplies/{onshorePowerSupplyId:guid}")]
    public async Task<OnshorePowerSupplyDto> UpdateOnshorePowerSupply(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid onshorePowerSupplyId,
        [FromBody] UpdateOnshorePowerSupplyDto dto)
    {
        return await updateOnshorePowerSupplyService.UpdateOnshorePowerSupply(projectId, caseId, onshorePowerSupplyId, dto);
    }
}
