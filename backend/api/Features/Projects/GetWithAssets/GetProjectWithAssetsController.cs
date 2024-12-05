using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Projects.GetWithAssets;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class GetProjectWithAssetsController(GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<ProjectDataDto> GetProjectWithAssets(Guid projectId)
    {
        return await getProjectDataService.GetProjectData(projectId);
    }
}
