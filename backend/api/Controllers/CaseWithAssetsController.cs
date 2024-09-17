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
public class CaseWithAssetsController : ControllerBase
{
    private readonly ICaseWithAssetsService _caseWithAssetsService;

    public CaseWithAssetsController(
        ICaseWithAssetsService caseWithAssetsService
    )
    {
        _caseWithAssetsService = caseWithAssetsService;
    }

    [HttpGet]
    [ActionType(ActionType.Read)]
    public async Task<ActionResult<CaseWithAssetsDto>> GetCaseWithAssets(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId
    )
    {
        var dto = await _caseWithAssetsService.GetCaseWithAssetsNoTracking(projectId, caseId);
        return Ok(dto);
    }
}
