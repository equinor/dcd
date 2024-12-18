using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectMembers.Get;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectMembers.Update;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateProjectMemberController(UpdateProjectMemberService updateProjectMemberService, GetProjectMemberService getProjectMemberService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/members")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectMemberDto> UpdateProjectMember([FromRoute] Guid projectId, [FromBody] UpdateProjectMemberDto updateProjectMemberDto)
    {
        await updateProjectMemberService.UpdateProjectMember(projectId, updateProjectMemberDto);

        return await getProjectMemberService.GetProjectMember(updateProjectMemberDto.UserId);
    }
}
