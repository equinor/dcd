using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Campaigns.Update;

public class UpdateCampaignController(UpdateCampaignService updateCampaignService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}")]
    public async Task<NoContentResult> UpdateCampaign(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid campaignId,
        [FromBody] UpdateCampaignDto dto)
    {
        await updateCampaignService.UpdateCampaign(projectId, caseId, campaignId, dto);
        return NoContent();
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}/cost")]
    public async Task<NoContentResult> UpdateCampaignCost(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid campaignId,
        [FromBody] UpdateCampaignCostDto dto)
    {
        await updateCampaignService.UpdateCampaignCost(projectId, caseId, campaignId, dto);
        return NoContent();
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}/wells")]
    public async Task<NoContentResult> UpdateCampaignWells(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid campaignId,
        [FromBody] UpdateCampaignWellsDto dto)
    {
        await updateCampaignService.UpdateCampaignWells(projectId, caseId, campaignId, dto);
        return NoContent();
    }
}
