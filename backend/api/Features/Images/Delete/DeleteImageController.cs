using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Images.Delete;

public class DeleteImageController(
    DeleteCaseImageService deleteCaseImageService,
    DeleteProjectImageService deleteProjectImageService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/cases/{caseId:guid}/images/{imageId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ActionResult> DeleteImage(Guid projectId, Guid caseId, Guid imageId)
    {
        await deleteCaseImageService.DeleteImage(projectId, caseId, imageId);

        return NoContent();
    }

    [HttpDelete("projects/{projectId:guid}/images/{imageId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ActionResult> DeleteImage(Guid projectId, Guid imageId)
    {
        await deleteProjectImageService.DeleteImage(projectId, imageId);

        return NoContent();
    }
}
