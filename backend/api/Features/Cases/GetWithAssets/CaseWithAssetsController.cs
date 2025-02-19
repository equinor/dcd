using api.AppInfrastructure.ControllerAttributes;
using api.Features.Cases.GetWithAssets.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Cases.GetWithAssets;

public class CaseWithAssetsController(CaseWithAssetsService caseWithAssetsService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/case-with-assets")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<CaseWithAssetsDto> GetCaseWithAssets([FromRoute] Guid projectId, [FromRoute] Guid caseId)
    {
        return await caseWithAssetsService.GetCaseWithAssets(projectId, caseId);
    }
}
