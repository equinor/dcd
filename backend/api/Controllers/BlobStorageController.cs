using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Dtos;

[Authorize]
[ApiController]
[Route("projects/{projectId}")]
public class ImageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;

    public ImageController(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

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
                var imageDto = await _blobStorageService.SaveImage(projectId, projectName, image, caseId.Value);
                return Ok(imageDto);
            }
            else
            {
                var imageDto = await _blobStorageService.SaveImage(projectId, projectName, image);
                return Ok(imageDto);
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while uploading the image.");
        }
    }

    [HttpPost("cases/{caseId}/images")]
    public Task<ActionResult<ImageDto>> UploadCaseImage(Guid projectId, [FromForm] string projectName, Guid caseId, [FromForm] IFormFile image)
    {
        return UploadImage(projectId, projectName, caseId, image);
    }

    [HttpGet("cases/{caseId}/images")]
    public async Task<ActionResult<List<ImageDto>>> GetImages(Guid projectId, Guid caseId)
    {
        try
        {
            var imageDtos = await _blobStorageService.GetCaseImages(caseId);
            return Ok(imageDtos);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving images.");
        }
    }

    [HttpDelete("cases/{caseId}/images/{imageId}")]
    public async Task<ActionResult> DeleteImage(Guid projectId, Guid caseId, Guid imageId)
    {
        try
        {
            await _blobStorageService.DeleteImage(caseId, imageId);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the image.");
        }
    }

    [HttpPost("images")]
    public Task<ActionResult<ImageDto>> UploadProjectImage(Guid projectId, [FromForm] string projectName, [FromForm] IFormFile image)
    {
        return UploadImage(projectId, projectName, null, image);
    }

    [HttpGet("images")]
    public async Task<ActionResult<List<ImageDto>>> GetProjectImages(Guid projectId)
    {
        try
        {
            var imageDtos = await _blobStorageService.GetProjectImages(projectId);
            return Ok(imageDtos);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving images.");
        }
    }

    [HttpDelete("images/{imageId}")]
    public async Task<ActionResult> DeleteProjectImage(Guid projectId, Guid imageId)
    {
        try
        {
            await _blobStorageService.DeleteImage(projectId, imageId);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the image.");
        }
    }
}
