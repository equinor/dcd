using api.AppInfrastructure.Authorization;
using api.Dtos;
using api.Models.Fusion;
using api.Services;
using api.Services.Fusion;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/members")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectMembersController(
    IProjectMemberService projectMemberService,
    IOrgChartMemberService fusionPeopleService) : ControllerBase
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
    public async Task<ProjectMemberDto> CreateProjectMember([FromRoute] Guid projectId,
        [FromBody] CreateProjectMemberDto createProjectMemberDto)
    {
        return await projectMemberService.CreateProjectMember(projectId, createProjectMemberDto);
    }

    [HttpGet("/context/{contextId:guid}")]
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [ActionType(ActionType.Edit)]
    public async Task<List<FusionPersonV1>> GetOrgChartMembers([FromRoute] Guid contextId)
    {
        return await fusionPeopleService.GetAllPersonsOnProject(contextId, 100, 0);
    }
}
