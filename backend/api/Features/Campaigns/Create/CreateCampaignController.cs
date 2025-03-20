using api.AppInfrastructure.ControllerAttributes;
using api.Features.Campaigns.Get;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Campaigns.Create;

public class CreateCampaignController(CreateCampaignService createCampaignService, GetCampaignService getCampaignService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/campaigns")]
    public async Task<CampaignDto> CreateCampaign(Guid projectId, Guid caseId, [FromBody] CreateCampaignDto dto)
    {
        var campaignId = await createCampaignService.CreateCampaign(projectId, caseId, dto);

        return await getCampaignService.Get(projectId, caseId, campaignId);
    }
}
