using Microsoft.AspNetCore.Mvc;
using api.Context;
using Microsoft.EntityFrameworkCore;
using api.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/images")]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly DcdDbContext _context;

    public BlobStorageController(IBlobStorageService blobStorageService, DcdDbContext context)
    {
        _blobStorageService = blobStorageService;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(Guid projectId, Guid caseId, IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("No image provided.");
        }

        var blobName = Guid.NewGuid().ToString();
        var contentType = image.ContentType;

        using var stream = new MemoryStream();
        await image.CopyToAsync(stream);
        var imageBytes = stream.ToArray();

        var imageUrl = await _blobStorageService.UploadImageAsync(imageBytes, contentType, blobName);

        var newImage = new Image
        {
            CaseId = caseId,
            Url = imageUrl,
        };
        _context.Images.Add(newImage);
        await _context.SaveChangesAsync();

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
            // Log the exception details if necessary
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving images.");
        }
    }
}