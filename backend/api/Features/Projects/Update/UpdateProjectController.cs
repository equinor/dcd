using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Projects.Update;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateProjectController(UpdateProjectService updateProjectService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectDataDto> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProjectDto updateProjectDto)
    {
        UpdateProjectDtoValidator.Validate(updateProjectDto);

        await updateProjectService.UpdateProject(projectId, updateProjectDto);

        return await getProjectDataService.GetProjectData(projectId);
    }
}
