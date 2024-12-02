using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.Images.Dto;
using api.Features.Images.Service;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Images;

public class ImageController(IBlobStorageService blobStorageService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/images")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public Task<ActionResult<ImageDto>> UploadCaseImage(Guid projectId, [FromForm] string projectName, Guid caseId, IFormFile image)
    {
        return UploadImage(projectId, projectName, caseId, image);
    }

    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/images")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<ActionResult<List<ImageDto>>> GetImages(Guid projectId, Guid caseId)
    {
        var imageDtos = await blobStorageService.GetCaseImages(caseId);
        return Ok(imageDtos);
    }

    [HttpDelete("projects/{projectId:guid}/cases/{caseId:guid}/images/{imageId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ActionResult> DeleteCaseImage(Guid projectId, Guid caseId, Guid imageId)
    {
        await blobStorageService.DeleteImage(imageId);
        return NoContent();
    }

    [HttpPost("projects/{projectId:guid}/images")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public Task<ActionResult<ImageDto>> UploadProjectImage(Guid projectId, [FromForm] string projectName, IFormFile image)
    {
        return UploadImage(projectId, projectName, null, image);
    }

    [HttpGet("projects/{projectId:guid}/images")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<ActionResult<List<ImageDto>>> GetProjectImages(Guid projectId)
    {
        var imageDtos = await blobStorageService.GetProjectImages(projectId);
        return Ok(imageDtos);
    }

    [HttpDelete("projects/{projectId:guid}/images/{imageId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ActionResult> DeleteProjectImage(Guid projectId, Guid imageId)
    {
        await blobStorageService.DeleteImage(imageId);
        return NoContent();
    }

    private async Task<ActionResult<ImageDto>> UploadImage(Guid projectId, string projectName, Guid? caseId, IFormFile image)
    {
        const int maxFileSize = 5 * 1024 * 1024; // 5MB
        string[] permittedExtensions = [".jpg", ".jpeg", ".png", ".gif"];

        if (image.Length == 0)
        {
            return BadRequest("No image provided or the file is empty.");
        }

        if (image.Length > maxFileSize)
        {
            return BadRequest($"File {image.FileName} exceeds the maximum allowed size of 5MB.");
        }

        var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            return BadRequest($"File {image.FileName} has an invalid extension. Only image files are allowed.");
        }

        return Ok(await blobStorageService.SaveImage(projectId, projectName, image, caseId));
    }
}
