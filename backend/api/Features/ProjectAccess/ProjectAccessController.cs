using api.AppInfrastructure.Authorization;
using api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectAccess;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectAccessController(IProjectAccessService projectAccessService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/access")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<AccessRightsDto> GetAccess(Guid projectId)
    {
        return await projectAccessService.GetUserProjectAccess(projectId);
    }
}
