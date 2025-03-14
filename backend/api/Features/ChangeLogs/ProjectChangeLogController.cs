using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.ChangeLogs;

public class ProjectChangeLogController(ProjectChangeLogService projectChangeLogService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/change-logs")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<List<ProjectChangeLogDto>> GetProjectChangeLogs(Guid projectId)
    {
        return await projectChangeLogService.GetProjectChangeLogs(projectId);
    }
}
