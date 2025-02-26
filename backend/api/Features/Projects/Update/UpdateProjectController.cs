using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Projects.Update;

public class UpdateProjectController(UpdateProjectService updateProjectService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ProjectDataDto> UpdateProject(Guid projectId, [FromBody] UpdateProjectDto updateProjectDto)
    {
        UpdateProjectDtoValidator.Validate(updateProjectDto);

        await updateProjectService.UpdateProject(projectId, updateProjectDto);

        return await getProjectDataService.GetProjectData(projectId);
    }
}
