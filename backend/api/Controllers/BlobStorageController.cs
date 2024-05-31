using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/images")]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;

    public BlobStorageController(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(Guid projectId, Guid caseId, IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("No image provided.");
        }

        var imageUrl = await _blobStorageService.SaveImageAsync(image, caseId);

        return Ok(new { imageUrl });
    }

    [HttpGet]
    public async Task<IActionResult> GetImages(Guid projectId, Guid caseId)
    {
        try
        {
            var imageUrls = await _blobStorageService.GetImageUrlsAsync(caseId);
            return Ok(imageUrls);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving images.");
        }
    }
}
