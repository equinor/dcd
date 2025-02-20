using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectMembers.Get;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.ProjectMembers.Create;

public class CreateProjectMemberController(CreateProjectMemberService createProjectMemberService, GetProjectMemberService getProjectMemberService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/members")]
    [AuthorizeActionType(ActionType.EditProjectMembers)]
    public async Task<ProjectMemberDto> CreateProjectMember(Guid projectId, [FromBody] CreateProjectMemberDto createProjectMemberDto)
    {
        var projectMemberId = await createProjectMemberService.CreateProjectMember(projectId, createProjectMemberDto);

        return await getProjectMemberService.GetProjectMember(projectId, projectMemberId);
    }
}
