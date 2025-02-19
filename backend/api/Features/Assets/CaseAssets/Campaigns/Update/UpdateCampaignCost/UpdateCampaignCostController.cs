using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateCampaignCost;

public class UpdateCampaignCostController(UpdateCampaignCostService updateCampaignCostService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}/cost")]
    public async Task<NoContentResult> UpdateCampaignCost(Guid projectId, Guid caseId, Guid campaignId, [FromBody] UpdateCampaignCostDto dto)
    {
        await updateCampaignCostService.UpdateCampaignCost(projectId, caseId, campaignId, dto);
        return NoContent();
    }
}
