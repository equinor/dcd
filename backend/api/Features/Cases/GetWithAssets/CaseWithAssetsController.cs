using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.GetWithAssets;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CaseWithAssetsController(CaseWithAssetsService caseWithAssetsService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/case-with-assets")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<CaseWithAssetsDto> GetCaseWithAssets([FromRoute] Guid projectId, [FromRoute] Guid caseId)
    {
        return await caseWithAssetsService.GetCaseWithAssetsNoTracking(projectId, caseId);
    }
}
