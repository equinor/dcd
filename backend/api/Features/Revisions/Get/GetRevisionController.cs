using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Revisions.Get;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class GetRevisionController(GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/revisions/{revisionId:guid}")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<RevisionDataDto> Get(Guid projectId, Guid revisionId)
    {
        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }
}
