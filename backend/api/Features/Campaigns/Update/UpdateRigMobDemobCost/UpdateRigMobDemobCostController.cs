using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Campaigns.Update.UpdateRigMobDemobCost;

public class UpdateRigMobDemobCostController(UpdateRigMobDemobCostService updateRigMobDemobCostService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}/rig-mobdemob-cost")]
    public async Task<NoContentResult> UpdateRigMobDemobCost(Guid projectId, Guid caseId, Guid campaignId, [FromBody] UpdateRigMobDemobCostDto dto)
    {
        await updateRigMobDemobCostService.UpdateRigMobDemobCost(projectId, caseId, campaignId, dto);

        return NoContent();
    }
}
