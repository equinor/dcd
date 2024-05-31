using api.Context;
using api.Models;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class BlobStorageService : IBlobStorageService
{
    private readonly DcdDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobStorageService(BlobServiceClient blobServiceClient, string containerName, DcdDbContext context)
    {
        _blobServiceClient = blobServiceClient;
        _containerName = containerName;
        _context = context;
    }

    public Task<string> GetBlobSasUrlAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerName,
            BlobName = blobName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            Protocol = SasProtocol.Https
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Create);

        var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;

        return Task.FromResult($"{blobClient.Uri}?{sasToken}");
    }
    private string GenerateSasTokenForBlob(BlobClient blobClient, BlobSasPermissions permissions)
    {
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
            BlobName = blobClient.Name,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            Protocol = SasProtocol.Https
        };
        sasBuilder.SetPermissions(permissions | BlobSasPermissions.Read); // Include read permissions

        var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;

        return sasToken;
    }
    public async Task<string> UploadImageAsync(byte[] imageBytes, string contentType, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var sasToken = GenerateSasTokenForBlob(blobClient, permissions: BlobSasPermissions.Write);

        var sasBlobUri = new Uri($"{blobClient.Uri}{sasToken}");
        var sasBlobClient = new BlobClient(sasBlobUri);

        using var stream = new MemoryStream(imageBytes, writable: false);
        await sasBlobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType });

        var imageUrl = blobClient.Uri.ToString();

        return imageUrl;
    }

    public async Task<IEnumerable<string>> GetImageUrlsAsync(Guid caseId)
    {
        var images = await _context.Images
            .Where(img => img.CaseId == caseId)
            .Select(img => img.Url)
            .ToListAsync();

        return images;
    }

    public async Task<string> SaveImageAsync(IFormFile image, Guid caseId)
    {
        var blobName = Guid.NewGuid().ToString();
        var contentType = image.ContentType;

        using var stream = new MemoryStream();
        await image.CopyToAsync(stream);
        var imageBytes = stream.ToArray();

        var imageUrl = await UploadImageAsync(imageBytes, contentType, blobName);

        // Save the image record to the database
        var newImage = new Image
        {
            CaseId = caseId,
            Url = imageUrl,
        };
        _context.Images.Add(newImage);
        await _context.SaveChangesAsync();

        return imageUrl;
    }
}