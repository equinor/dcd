using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectMembers.Get;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectMembers.Create;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CreateProjectMemberController(CreateProjectMemberService createProjectMemberService, GetProjectMemberService getProjectMemberService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/members")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectMemberDto> CreateProjectMember([FromRoute] Guid projectId, [FromBody] CreateProjectMemberDto createProjectMemberDto)
    {
        var projectMemberId = await createProjectMemberService.CreateProjectMember(projectId, createProjectMemberDto);

        return await getProjectMemberService.GetProjectMember(projectId, projectMemberId);
    }
}
