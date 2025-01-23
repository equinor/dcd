using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.ProjectMembers.Delete;

public class DeleteProjectMemberController(DeleteProjectMemberService deleteProjectMemberService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/members/{userId:guid}")]
    [AuthorizeActionType(ActionType.EditProjectMembers)]
    public async Task DeleteProjectMember(Guid projectId, Guid userId)
    {
        await deleteProjectMemberService.DeleteProjectMember(projectId, userId);
    }
}
