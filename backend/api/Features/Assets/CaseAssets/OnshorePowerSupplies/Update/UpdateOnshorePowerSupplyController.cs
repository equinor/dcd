using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Update;

public class UpdateOnshorePowerSupplyController(UpdateOnshorePowerSupplyService updateOnshorePowerSupplyService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/onshorePowerSupplys/{onshorePowerSupplyId:guid}")]
    public async Task<OnshorePowerSupplyDto> UpdateOnshorePowerSupply(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid onshorePowerSupplyId,
        [FromBody] UpdateOnshorePowerSupplyDto dto)
    {
        return await updateOnshorePowerSupplyService.UpdateOnshorePowerSupply(projectId, caseId, onshorePowerSupplyId, dto);
    }
}
