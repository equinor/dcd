using api.Dtos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/images")]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IImageRepository _imageRepository;

    public BlobStorageController(IBlobStorageService blobStorageService, IImageRepository imageRepository)
    {
        _blobStorageService = blobStorageService;
        _imageRepository = imageRepository;
    }

    [HttpPost]
    public async Task<ActionResult<ImageDto>> UploadImage(Guid projectId, Guid caseId, IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("No image provided.");
        }

        var imageDto = await _blobStorageService.SaveImage(image, caseId);
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
