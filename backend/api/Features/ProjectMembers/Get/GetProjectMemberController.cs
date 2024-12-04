using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectMembers.Get.Sync;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectMembers.Get;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class GetProjectMemberController(
    FusionOrgChartProjectMemberService fusionOrgChartProjectMemberService,
    GetProjectMemberService getProjectMemberService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/members")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<List<ProjectMemberDto>> GetProjectMembersWithoutUpdatingPmt([FromRoute] Guid projectId)
    {
        return await getProjectMemberService.GetProjectMembers(projectId);
    }

    [HttpGet("projects/{projectId:guid}/members/context/{contextId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<List<ProjectMemberDto>> GetProjectMembersWithUpdatingPmt([FromRoute] Guid projectId, [FromRoute] Guid contextId)
    {
        await fusionOrgChartProjectMemberService.SyncPmtMembersOnProject(projectId, contextId);

        return await getProjectMemberService.GetProjectMembers(projectId);
    }
}
