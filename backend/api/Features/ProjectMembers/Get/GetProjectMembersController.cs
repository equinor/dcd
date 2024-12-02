using api.AppInfrastructure.Authorization;
using api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectMembers.Get;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class GetProjectMembersController(GetProjectMemberService getProjectMemberService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/members")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<List<ProjectMemberDto>> GetProjectMembers([FromRoute] Guid projectId)
    {
        return await getProjectMemberService.GetProjectMembers(projectId);
    }
}
