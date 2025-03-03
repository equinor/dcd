using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Images.Update;

public class UpdateImageController(
    UpdateCaseImageService updateCaseImageService,
    UpdateProjectImageService updateProjectImageService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/images/{imageId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ActionResult> UpdateCaseImage(Guid projectId, Guid caseId, Guid imageId, [FromBody] UpdateImageDto dto)
    {
        await updateCaseImageService.UpdateImage(projectId, caseId, imageId, dto);

        return NoContent();
    }

    [HttpPut("projects/{projectId:guid}/images/{imageId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ActionResult> UpdateProjectImage(Guid projectId, Guid imageId, [FromBody] UpdateImageDto dto)
    {
        await updateProjectImageService.UpdateImage(projectId, imageId, dto);

        return NoContent();
    }
}
