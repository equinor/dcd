using api.Dtos;
using api.Models;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IImageRepository _imageRepository;
    private readonly string _containerName;

    public BlobStorageService(BlobServiceClient blobServiceClient, IImageRepository imageRepository, IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _imageRepository = imageRepository;
        _containerName = configuration["azureStorageAccountImageContainerName"]
                         ?? throw new InvalidOperationException("Container name configuration is missing.");

        if (string.IsNullOrEmpty(_containerName))
        {
            throw new InvalidOperationException("Container name configuration is missing or empty.");
        }
    }

    public Task<string> GetBlobSasUrl(string blobName)
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
        sasBuilder.SetPermissions(permissions | BlobSasPermissions.Read);

        var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;

        return sasToken;
    }

    public async Task<string> UploadImage(byte[] imageBytes, string contentType, string blobName)
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

    public async Task<IEnumerable<string>> GetImageUrls(Guid caseId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var caseFolder = $"{caseId}/";
        var blobUrls = new List<string>();

        await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: caseFolder))
        {
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            var sasToken = GenerateSasTokenForBlob(blobClient, BlobSasPermissions.Read);
            var blobUrl = $"{blobClient.Uri}{sasToken}";
            blobUrls.Add(blobUrl);
        }

        return blobUrls;
    }

    public async Task<ImageDto> SaveImage(IFormFile image, Guid caseId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient($"{caseId}/{image.FileName}");

        await using var stream = image.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });

        var imageUrl = blobClient.Uri.ToString();
        var createTime = DateTimeOffset.UtcNow;

        var imageEntity = new Image
        {
            Url = imageUrl,
            CreateTime = createTime,
            CaseId = caseId,
        };

        await _imageRepository.AddImage(imageEntity);

        var imageDto = new ImageDto
        {
            Id = imageEntity.Id,
            Url = imageEntity.Url,
            CreateTime = imageEntity.CreateTime,
            Description = imageEntity.Description,
            CaseId = imageEntity.CaseId
        };

        return imageDto;
    }

    public async Task<List<ImageDto>> GetImagesByCaseIdAndMapToDto(Guid caseId)
{
    var images = await _imageRepository.GetImagesByCaseId(caseId);
    var imageDtos = images.Select(image => new ImageDto
    {
        Id = image.Id,
        Url = image.Url,
        CreateTime = image.CreateTime,
        Description = image.Description,
        CaseId = image.CaseId
    }).ToList();

    return imageDtos;
}
}
