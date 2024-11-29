using api.AppInfrastructure.Authorization;
using api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectMembers.Delete;

[ApiController]
[Route("projects/{projectId:guid}/members")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class DeleteProjectMembersController(DeleteProjectMemberService deleteProjectMemberService) : ControllerBase
{
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [HttpDelete("{userId:guid}")]
    [ActionType(ActionType.Edit)]
    public async Task DeleteProjectMember(Guid projectId, Guid userId)
    {
        await deleteProjectMemberService.DeleteProjectMember(projectId, userId);
    }
}
