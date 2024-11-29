using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.ProjectMembers.Get;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectMembers.Update;

[ApiController]
[Route("projects/{projectId:guid}/members")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateProjectMembersController(UpdateProjectMemberService updateProjectMemberService) : ControllerBase
{
    [HttpPut]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [ActionType(ActionType.Edit)]
    public async Task<ProjectMemberDto> UpdateProjectMember([FromRoute] Guid projectId, [FromBody] UpdateProjectMemberDto updateProjectMemberDto)
    {
        return await updateProjectMemberService.UpdateProjectMember(projectId, updateProjectMemberDto);
    }
}
