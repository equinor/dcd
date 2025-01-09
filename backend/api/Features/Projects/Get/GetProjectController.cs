using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Projects.Get;

public class GetProjectController(GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}")]
    [AuthorizeActionType(ActionType.Read)]
    [DisableLazyLoading]
    public async Task<ProjectDataDto> GetProjectData(Guid projectId)
    {
        return await getProjectDataService.GetProjectData(projectId);
    }
}
