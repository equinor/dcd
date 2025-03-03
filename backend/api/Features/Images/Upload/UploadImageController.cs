using api.AppInfrastructure.ControllerAttributes;
using api.Features.Images.Get;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Images.Upload;

public class UploadImageController(
    UploadCaseImageService uploadCaseImageService,
    UploadProjectImageService uploadProjectImageService,
    GetCaseImageService getCaseImageService,
    GetProjectImageService getProjectImageService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/images")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ImageDto> UploadCaseImage(Guid projectId, Guid caseId, IFormFile image)
    {
        UploadImageValidator.EnsureIsValid(image);

        var imageId = await uploadCaseImageService.SaveImage(image, projectId, caseId);

        return await getCaseImageService.GetImage(imageId);
    }

    [HttpPost("projects/{projectId:guid}/images")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ImageDto> UploadProjectImage(Guid projectId, IFormFile image)
    {
        UploadImageValidator.EnsureIsValid(image);

        var imageId = await uploadProjectImageService.SaveImage(image, projectId);

        return await getProjectImageService.GetImage(imageId);
    }
}
