using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.CampaignWells.Get;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.CampaignWells.Save;

public class SaveCampaignWellController(SaveCampaignWellService saveCampaignWellService,
    GetCampaignWellService getCampaignWellService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/campaigns/{campaignId:guid}/wells/{wellId:guid}/save")]
    public async Task<CampaignWellDto> SaveCampaignWell(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid campaignId,
        [FromRoute] Guid wellId,
        [FromBody] SaveCampaignWellDto dto)
    {
        await saveCampaignWellService.SaveCampaignWell(projectId, caseId, campaignId, wellId, dto);
        return await getCampaignWellService.GetCampaignWell(projectId, caseId, campaignId, wellId);
    }
}
