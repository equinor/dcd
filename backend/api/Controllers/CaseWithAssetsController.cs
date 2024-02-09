using api.Dtos;
using api.Services;

using Api.Authorization;

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

    [HttpPut(Name = "UpdateCaseWithAssets")]
    public async Task<ActionResult<ProjectWithGeneratedProfilesDto>> UpdateCaseWithAssetsAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CaseWithAssetsWrapperDto caseWrapperDto
        )
    {
        var dto = await _caseWithAssetsService.UpdateCaseWithAssetsAsync(projectId, caseId, caseWrapperDto);
        return Ok(dto);
    }
}
