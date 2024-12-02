using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.Projects.GetWithAssets;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Projects.Create;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CreateProjectController(GetProjectWithAssetsService getProjectWithAssetsService, CreateProjectService createProjectService) : ControllerBase
{
    [HttpPost("projects")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectWithAssetsDto> CreateProject([FromQuery] Guid contextId)
    {
        var projectId = await createProjectService.CreateProject(contextId);

        return await getProjectWithAssetsService.GetProjectWithAssets(projectId);
    }
}
