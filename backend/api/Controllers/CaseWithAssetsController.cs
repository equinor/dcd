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
    private readonly ICaseAndAssetsService _caseAndAssetsService;
    private readonly ICaseWithAssetsService _caseWithAssetsService;

    public CaseWithAssetsController(
        ICaseAndAssetsService caseAndAssetsService,
        ICaseWithAssetsService caseWithAssetsService
    )
    {
        _caseWithAssetsService = caseWithAssetsService;
        _caseAndAssetsService = caseAndAssetsService;
    }

    [HttpPost(Name = "UpdateCaseWithAssets")]
    public async Task<ActionResult<ProjectWithGeneratedProfilesDto>> UpdateCaseWithAssets(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CaseWithAssetsWrapperDto caseWrapperDto
    )
    {
        var dto = await _caseAndAssetsService.UpdateCaseWithAssets(projectId, caseId, caseWrapperDto);
        return Ok(dto);
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
