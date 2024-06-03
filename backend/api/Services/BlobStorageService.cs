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
