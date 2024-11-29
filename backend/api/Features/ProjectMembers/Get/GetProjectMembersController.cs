using api.AppInfrastructure.Authorization;
using api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectMembers.Get;

[ApiController]
[Route("projects/{projectId:guid}/members")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class GetProjectMembersController(GetProjectMemberService getProjectMemberService) : ControllerBase
{
    [HttpGet]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [ActionType(ActionType.Read)]
    public async Task<List<ProjectMemberDto>> GetProjectMembers([FromRoute] Guid projectId)
    {
        return await getProjectMemberService.GetProjectMembers(projectId);
    }
}
