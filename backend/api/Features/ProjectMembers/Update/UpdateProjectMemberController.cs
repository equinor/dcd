using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectMembers.Get;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.ProjectMembers.Update;

public class UpdateProjectMemberController(UpdateProjectMemberService updateProjectMemberService, GetProjectMemberService getProjectMemberService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/members")]
    [AuthorizeActionType(ActionType.EditProjectMembers)]
    public async Task<ProjectMemberDto> UpdateProjectMember([FromRoute] Guid projectId, [FromBody] UpdateProjectMemberDto updateProjectMemberDto)
    {
        await updateProjectMemberService.UpdateProjectMember(projectId, updateProjectMemberDto);

        return await getProjectMemberService.GetProjectMember(projectId, updateProjectMemberDto.UserId);
    }
}
