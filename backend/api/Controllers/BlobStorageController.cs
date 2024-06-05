using api.Dtos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<ImageDto>> UploadImage(Guid projectId, [FromForm] string projectName, Guid caseId, [FromForm] IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("No image provided.");
        }

        var imageDto = await _blobStorageService.SaveImage(projectId, projectName, image, caseId);
        return Ok(imageDto);
    }

    [HttpGet]
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
}
