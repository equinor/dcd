using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.ProjectData;

[ApiController]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectDataController(GetProjectDataService getProjectDataService) : ControllerBase
{
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User, ApplicationRole.ReadOnly)]
    [HttpGet("projects/{projectId:guid}/get-full-graph")]
    public async Task<ProjectDataDto> GetProjectData([FromRoute] Guid projectId)
    {
        return await getProjectDataService.GetProjectData(projectId);
    }

    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User, ApplicationRole.ReadOnly)]
    [HttpGet("projects/{projectId:guid}/revisions/{revisionId:guid}/get-full-graph")]
    public async Task<RevisionDataDto> GetRevisionData([FromRoute] Guid projectId, [FromRoute] Guid revisionId)
    {
        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }
}
