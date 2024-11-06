using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/members")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectMembersController(IProjectMemberService projectMemberService) : ControllerBase
{
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [HttpDelete("{userId}")]
    [ActionType(ActionType.Edit)]
    public async Task Get(Guid projectId, Guid userId)
    {
        await projectMemberService.DeleteProjectMember(projectId, userId);
    }

    [HttpPost]
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [ActionType(ActionType.Edit)]
    public async Task<ProjectMemberDto> CreateProjectMember([FromRoute] Guid projectId, [FromBody] CreateProjectMemberDto createProjectMemberDto)
    {
        return await projectMemberService.CreateProjectMember(projectId, createProjectMemberDto);
    }

}
