using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateCampaignWells;

public class UpdateCampaignWellsController(UpdateCampaignWellsService updateCampaignWellsService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}/wells")]
    public async Task<NoContentResult> UpdateCampaignWells(Guid projectId, Guid caseId, Guid campaignId, [FromBody] List<SaveCampaignWellDto> campaignWellDtos)
    {
        await updateCampaignWellsService.UpdateCampaignWells(projectId, caseId, campaignId, campaignWellDtos);

        return NoContent();
    }
}
