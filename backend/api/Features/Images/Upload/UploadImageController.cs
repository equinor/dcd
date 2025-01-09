using api.AppInfrastructure.ControllerAttributes;
using api.Features.Images.Get;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Images.Upload;

public class UploadImageController(UploadImageService uploadImageService, GetImageService getImageService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/images")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableLazyLoading]
    public async Task<ImageDto> UploadCaseImage(Guid projectId, Guid caseId, IFormFile image)
    {
        UploadImageValidator.EnsureIsValid(image);

        var imageId = await uploadImageService.SaveImage(image, projectId, caseId);
        return await getImageService.GetImage(imageId);
    }

    [HttpPost("projects/{projectId:guid}/images")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableLazyLoading]
    public async Task<ImageDto> UploadProjectImage(Guid projectId, IFormFile image)
    {
        UploadImageValidator.EnsureIsValid(image);

        var imageId = await uploadImageService.SaveImage(image, projectId, null);
        return await getImageService.GetImage(imageId);
    }
}
