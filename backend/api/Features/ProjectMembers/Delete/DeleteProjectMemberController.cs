using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectMembers.Delete;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class DeleteProjectMemberController(DeleteProjectMemberService deleteProjectMemberService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/members/{userId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task DeleteProjectMember(Guid projectId, Guid userId)
    {
        await deleteProjectMemberService.DeleteProjectMember(projectId, userId);
    }
}
