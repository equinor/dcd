using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.Images.Get;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Images.Upload;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UploadImageController(UploadImageService uploadImageService, GetImageService getImageService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/images")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public async Task<ImageDto> UploadCaseImage(Guid projectId, Guid caseId, IFormFile image)
    {
        UploadImageValidator.EnsureIsValid(image);

        var imageId = await uploadImageService.SaveImage(image, projectId, caseId);
        return await getImageService.GetImage(imageId);
    }

    [HttpPost("projects/{projectId:guid}/images")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public async Task<ImageDto> UploadProjectImage(Guid projectId, IFormFile image)
    {
        UploadImageValidator.EnsureIsValid(image);

        var imageId = await uploadImageService.SaveImage(image, projectId, null);
        return await getImageService.GetImage(imageId);
    }
}
