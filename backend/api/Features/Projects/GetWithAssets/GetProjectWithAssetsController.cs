using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Projects.GetWithAssets;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class GetProjectWithAssetsController(GetProjectWithAssetsService getProjectWithAssetsService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<ProjectWithAssetsDto> GetProjectWithAssets(Guid projectId)
    {
        return await getProjectWithAssetsService.GetProjectWithAssets(projectId);
    }
}
