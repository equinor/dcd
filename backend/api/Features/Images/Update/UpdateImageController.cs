using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Images.Update;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateImageController(UpdateImageService updateImageService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/images/{imageId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ActionResult> UpdateImage(Guid projectId, Guid caseId, Guid imageId, [FromBody] UpdateImageDto dto)
    {
        await updateImageService.UpdateCaseImage(projectId, caseId, imageId, dto);
        return NoContent();
    }

    [HttpPut("projects/{projectId:guid}/images/{imageId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ActionResult> UpdateImage(Guid projectId, Guid imageId, [FromBody] UpdateImageDto dto)
    {
        await updateImageService.UpdateProjectImage(projectId, imageId, dto);
        return NoContent();
    }
}
