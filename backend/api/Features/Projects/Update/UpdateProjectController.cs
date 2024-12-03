using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.Projects.GetWithCases;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Projects.Update;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateProjectController(UpdateProjectService updateProjectService, GetProjectWithCasesService getProjectWithCasesService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectWithCasesDto> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProjectDto projectDto)
    {
        await updateProjectService.UpdateProject(projectId, projectDto);

        return await getProjectWithCasesService.GetProjectWithCases(projectId);
    }
}
