using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;

    public BlobStorageController(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpGet("sas-token")]
    public async Task<IActionResult> GetSasToken()
    {
        var blobName = Guid.NewGuid().ToString(); // Generate a unique name for the blob
        var sasUrl = await _blobStorageService.GetBlobSasUrlAsync(blobName);
        return Ok(new { sasUrl, blobName });
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("No image provided.");
        }

        var blobName = Guid.NewGuid().ToString(); // Generate a unique name for the blob
        var contentType = image.ContentType;

        using var stream = new MemoryStream();
        await image.CopyToAsync(stream);
        var imageBytes = stream.ToArray();

        var imageUrl = await _blobStorageService.UploadImageAsync(imageBytes, contentType, blobName);

        return Ok(new { imageUrl });
    }
}