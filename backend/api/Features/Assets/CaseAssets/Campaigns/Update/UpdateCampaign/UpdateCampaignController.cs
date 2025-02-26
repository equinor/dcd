using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateCampaign;

public class UpdateCampaignController(UpdateCampaignService updateCampaignService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}")]
    public async Task<NoContentResult> UpdateCampaign(Guid projectId, Guid caseId, Guid campaignId, [FromBody] UpdateCampaignDto dto)
    {
        await updateCampaignService.UpdateCampaign(projectId, caseId, campaignId, dto);
        return NoContent();
    }
}
