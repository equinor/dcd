using api.AppInfrastructure.Authorization;
using api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectMembers.Create;

[ApiController]
[Route("projects/{projectId:guid}/members")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CreateProjectMembersController(CreateProjectMemberService createProjectMemberService) : ControllerBase
{
    [HttpPost]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [ActionType(ActionType.Edit)]
    public async Task<ProjectMemberDto> CreateProjectMember([FromRoute] Guid projectId, [FromBody] CreateProjectMemberDto createProjectMemberDto)
    {
        return await createProjectMemberService.CreateProjectMember(projectId, createProjectMemberDto);
    }
}
