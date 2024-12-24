using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Projects.Create;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CreateProjectController(CreateProjectService createProjectService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPost("projects")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public async Task<ProjectDataDto> CreateProject([FromQuery] Guid contextId)
    {
        var projectId = await createProjectService.CreateProject(contextId);

        return await getProjectDataService.GetProjectData(projectId);
    }
}
