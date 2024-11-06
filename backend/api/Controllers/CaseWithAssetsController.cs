using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/case-with-assets")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class CaseWithAssetsController(ICaseWithAssetsService caseWithAssetsService) : ControllerBase
{
    [HttpGet]
    [ActionType(ActionType.Read)]
    public async Task<ActionResult<CaseWithAssetsDto>> GetCaseWithAssets(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId
    )
    {
        var dto = await caseWithAssetsService.GetCaseWithAssetsNoTracking(projectId, caseId);
        return Ok(dto);
    }
}
