using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/[controller]")]
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
    public async Task<ActionResult<CaseWithAssetsDto>> GetCaseWithAssets(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId
    )
    {
        var dto = await _caseWithAssetsService.GetCaseWithAssets(projectId, caseId);
        return Ok(dto);
    }
}
