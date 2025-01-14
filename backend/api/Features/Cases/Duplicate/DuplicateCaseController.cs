using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Cases.Duplicate;

public class DuplicateCaseController(DuplicateCaseService duplicateCaseService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/cases/copy")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableLazyLoading]
    public async Task<ProjectDataDto> DuplicateCase([FromRoute] Guid projectId, [FromQuery] Guid copyCaseId)
    {
        await duplicateCaseService.DuplicateCase(projectId, copyCaseId);

        return await getProjectDataService.GetProjectData(projectId);
    }
}
