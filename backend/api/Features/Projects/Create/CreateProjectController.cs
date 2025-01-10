using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Projects.Create;

public class CreateProjectController(CreateProjectService createProjectService, GetProjectDataService getProjectDataService, CurrentUser currentUser) : ControllerBase
{
    [HttpPost("projects")]
    [DisableLazyLoading]
    [Authorize]
    public async Task<ActionResult<ProjectDataDto>> CreateProject([FromQuery] Guid contextId)
    {
        if (!currentUser.ApplicationRoles.Contains(ApplicationRole.Admin) && !currentUser.ApplicationRoles.Contains(ApplicationRole.User))
        {
            return Unauthorized();
        }

        var projectId = await createProjectService.CreateProject(contextId);

        return await getProjectDataService.GetProjectData(projectId);
    }
}
