using api.Dtos;
using api.Services;

using api.Authorization;

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

    public CaseWithAssetsController(ICaseWithAssetsService caseWithAssetsService)
    {
        _caseWithAssetsService = caseWithAssetsService;
    }

    [HttpPost(Name = "UpdateCaseWithAssets")]
    public async Task<ActionResult<ProjectWithGeneratedProfilesDto>> UpdateCaseWithAssets(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CaseWithAssetsWrapperDto caseWrapperDto
    )
    {
        var dto = await _caseWithAssetsService.UpdateCaseWithAssets(projectId, caseId, caseWrapperDto);
        return Ok(dto);
    }
}
