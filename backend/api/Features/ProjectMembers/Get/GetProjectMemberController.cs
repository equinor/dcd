using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectMembers.Get.Sync;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.ProjectMembers.Get;

public class GetProjectMemberController(
    FusionOrgChartProjectMemberService fusionOrgChartProjectMemberService,
    GetProjectMemberService getProjectMemberService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/members")]
    [AuthorizeActionType(ActionType.EditProjectMembers)]
    public async Task<List<ProjectMemberDto>> GetProjectMembersWithoutUpdatingPmt([FromRoute] Guid projectId)
    {
        return await getProjectMemberService.GetProjectMembers(projectId);
    }

    [HttpGet("projects/{projectId:guid}/members/context/{contextId:guid}")]
    [AuthorizeActionType(ActionType.EditProjectMembers)]
    public async Task<List<ProjectMemberDto>> GetProjectMembersWithUpdatingPmt([FromRoute] Guid projectId, [FromRoute] Guid contextId)
    {
        await fusionOrgChartProjectMemberService.SyncPmtMembersOnProject(projectId, contextId);

        return await getProjectMemberService.GetProjectMembers(projectId);
    }
}
