using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Images.Delete;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class DeleteImageController(DeleteImageService deleteImageService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/images/{imageId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ActionResult> DeleteImage(Guid projectId, Guid imageId)
    {
        await deleteImageService.DeleteImage(projectId, imageId);
        return NoContent();
    }
}
