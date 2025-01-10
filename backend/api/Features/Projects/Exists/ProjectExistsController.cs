using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Projects.Exists;

public class ProjectExistsController(ProjectExistsService projectExistsService) : ControllerBase
{
    [HttpGet("projects/exists")]
    [DisableLazyLoading]
    [Authorize]
    public async Task<ProjectExistsDto> CreateProject([FromQuery] Guid contextId)
    {
        return await projectExistsService.ProjectExists(contextId);
    }
}
