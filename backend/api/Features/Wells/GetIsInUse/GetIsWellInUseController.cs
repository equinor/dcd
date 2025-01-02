using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Wells.GetIsInUse;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class GetIsWellInUseController(GetIsWellInUseService getIsWellInUseService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/wells/{wellId:guid}/is-in-use")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public async Task<bool> IsWellInUse([FromRoute] Guid projectId, [FromRoute] Guid wellId)
    {
        return await getIsWellInUseService.IsWellInUse(wellId);
    }
}
