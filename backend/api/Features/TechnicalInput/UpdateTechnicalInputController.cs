using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;
using api.Features.TechnicalInput.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.TechnicalInput;

public class UpdateTechnicalInputController(TechnicalInputService technicalInputService,
    UpdateProjectAndOperationalWellsCostService updateProjectAndOperationalWellsCostService,
    GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/technical-input")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ProjectDataDto> UpdateTechnicalInput([FromRoute] Guid projectId, [FromBody] UpdateTechnicalInputDto dto)
    {
        await updateProjectAndOperationalWellsCostService.UpdateProjectAndOperationalWellsCosts(projectId, dto);
        await technicalInputService.UpdateTechnicalInput(projectId, dto);
        return await getProjectDataService.GetProjectData(projectId);
    }
}
