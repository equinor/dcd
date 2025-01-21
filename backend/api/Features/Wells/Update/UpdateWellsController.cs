using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;
using api.Features.Wells.Update.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Wells.Update;

public class UpdateWellsController(UpdateWellsService updateWellsService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/wells")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ProjectDataDto> UpdateWells([FromRoute] Guid projectId, [FromBody] UpdateWellsDto dto)
    {
        await updateWellsService.UpdateWells(projectId, dto);
        return await getProjectDataService.GetProjectData(projectId);
    }
}
