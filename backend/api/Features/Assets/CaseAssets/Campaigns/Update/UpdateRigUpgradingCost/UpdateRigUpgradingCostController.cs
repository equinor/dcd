using api.AppInfrastructure.ControllerAttributes;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateRigUpgradingCost;

public class UpdateRigUpgradingCostController(UpdateRigUpgradingCostService updateRigUpgradingCostService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}/rig-upgrading-cost")]
    public async Task<NoContentResult> UpdateRigUpgradingCost(
        Guid projectId, 
        Guid caseId, 
        Guid campaignId, 
        [FromBody] UpdateRigUpgradingCostDto updateRigUpgradingCostDto)
    {
        await updateRigUpgradingCostService.UpdateRigUpgradingCost(projectId, caseId, campaignId, updateRigUpgradingCostDto);
        return NoContent();
    }
} 