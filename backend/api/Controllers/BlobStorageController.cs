using api.Authorization;
using api.Controllers;
using api.Dtos;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("projects/{projectId}")]
[RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
public class BlobStorageController(IBlobStorageService blobStorageService) : ControllerBase
{
    private async Task<ActionResult<ImageDto>> UploadImage(Guid projectId, string projectName, Guid? caseId, IFormFile image)
    {
        const int maxFileSize = 5 * 1024 * 1024; // 5MB
        string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        if (image == null || image.Length == 0)
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

        try
        {
            if (caseId.HasValue)
            {
                var imageDto = await blobStorageService.SaveImage(projectId, projectName, image, caseId.Value);
                return Ok(imageDto);
            }
            else
            {
                var imageDto = await blobStorageService.SaveImage(projectId, projectName, image);
                return Ok(imageDto);
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while uploading the image.");
        }
    }

    [HttpPost("cases/{caseId}/images")]
    [ActionType(ActionType.Edit)]
    public Task<ActionResult<ImageDto>> UploadCaseImage(Guid projectId, [FromForm] string projectName, Guid caseId, IFormFile image)
    {
        return UploadImage(projectId, projectName, caseId, image);
    }

    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User
    )]
    [HttpGet("cases/{caseId}/images")]
    [ActionType(ActionType.Read)]
    public async Task<ActionResult<List<ImageDto>>> GetImages(Guid projectId, Guid caseId)
    {
        try
        {
            var imageDtos = await blobStorageService.GetCaseImages(caseId);
            return Ok(imageDtos);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving images.");
        }
    }

    [HttpDelete("cases/{caseId}/images/{imageId}")]
    [ActionType(ActionType.Edit)]
    public async Task<ActionResult> DeleteCaseImage(Guid imageId)
    {
        try
        {
            await blobStorageService.DeleteImage(imageId);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the image.");
        }
    }

    [HttpPost("images")]
    [ActionType(ActionType.Edit)]
    public Task<ActionResult<ImageDto>> UploadProjectImage(Guid projectId, [FromForm] string projectName, IFormFile image)
    {
        return UploadImage(projectId, projectName, null, image);
    }

    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User
    )]
    [HttpGet("images")]
    [ActionType(ActionType.Read)]
    public async Task<ActionResult<List<ImageDto>>> GetProjectImages(Guid projectId)
    {
        try
        {
            var imageDtos = await blobStorageService.GetProjectImages(projectId);
            return Ok(imageDtos);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving images.");
        }
    }

    [HttpDelete("images/{imageId}")]
    [ActionType(ActionType.Edit)]
    public async Task<ActionResult> DeleteProjectImage(Guid imageId)
    {
        try
        {
            await blobStorageService.DeleteImage(imageId);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the image.");
        }
    }
}
