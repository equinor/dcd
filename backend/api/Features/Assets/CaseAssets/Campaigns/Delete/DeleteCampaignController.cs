using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Campaigns.Delete;

public class DeleteCampaignController(DeleteCampaignService deleteCampaignService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpDelete("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}")]
    public async Task<NoContentResult> DeleteCampaign(Guid projectId, Guid caseId, Guid campaignId)
    {
        await deleteCampaignService.DeleteCampaign(projectId, caseId, campaignId);
        return NoContent();
    }
}
