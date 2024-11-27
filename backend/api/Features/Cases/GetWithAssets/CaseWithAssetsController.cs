using api.AppInfrastructure.Authorization;
using api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.GetWithAssets;

[ApiController]
[Route("projects/{projectId:guid}/cases/{caseId:guid}/case-with-assets")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
public class CaseWithAssetsController(CaseWithAssetsService caseWithAssetsService) : ControllerBase
{
    [HttpGet]
    [ActionType(ActionType.Read)]
    public async Task<CaseWithAssetsDto> GetCaseWithAssets([FromRoute] Guid projectId, [FromRoute] Guid caseId)
    {
        return await caseWithAssetsService.GetCaseWithAssetsNoTracking(projectId, caseId);
    }
}
