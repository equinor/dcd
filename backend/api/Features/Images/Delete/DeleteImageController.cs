using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Images.Delete;

public class DeleteImageController(DeleteImageService deleteImageService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/images/{imageId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableLazyLoading]
    public async Task<ActionResult> DeleteImage(Guid projectId, Guid imageId)
    {
        await deleteImageService.DeleteImage(projectId, imageId);
        return NoContent();
    }
}
