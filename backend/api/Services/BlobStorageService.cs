using api.Dtos;
using api.Exceptions;
using api.Models;

using AutoMapper;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;
    private readonly string _containerName;

    public BlobStorageService(BlobServiceClient blobServiceClient, IImageRepository imageRepository, IConfiguration configuration, IMapper mapper)
    {
        _blobServiceClient = blobServiceClient;
        _imageRepository = imageRepository;
        _mapper = mapper;
        _containerName = GetContainerName(configuration);
    }

    private string GetContainerName(IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("AppConfiguration__Environment") ?? "default";
        var containerKey = environment switch
        {
            "localdev" => "AzureStorageAccountImageContainerCI",
            "CI" => "AzureStorageAccountImageContainerCI",
            "radix-dev" => "AzureStorageAccountImageContainerCI",
            "dev" => "AzureStorageAccountImageContainerCI",
            "qa" => "AzureStorageAccountImageContainerQA",
            "radix-qa" => "AzureStorageAccountImageContainerQA",
            "prod" => "AzureStorageAccountImageContainerProd",
            "radix-prod" => "AzureStorageAccountImageContainerProd",
            _ => throw new InvalidOperationException($"Unknown fusion environment: {environment}")
        };

        return configuration[containerKey]
                             ?? throw new InvalidOperationException($"Container name configuration for {environment} is missing.");
    }

    private string SanitizeBlobName(string name)
    {
        return name.Replace(" ", "-").Replace("/", "-").Replace("\\", "-");
    }

    public async Task<ImageDto> SaveImage(Guid projectId, string projectName, IFormFile image, Guid? caseId = null)
    {
        var sanitizedProjectName = SanitizeBlobName(projectName);
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var imageId = Guid.NewGuid();

        if (projectId == Guid.Empty || caseId == Guid.Empty)
        {
            throw new ArgumentException("ProjectId and/or CaseId cannot be empty.");
        }

        var blobName = caseId.HasValue
            ? $"{sanitizedProjectName}/cases/{caseId}/{imageId}"
            : $"{sanitizedProjectName}/projects/{projectId}/{imageId}";

        var blobClient = containerClient.GetBlobClient(blobName);

        await using var stream = image.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });

        var imageUrl = blobClient.Uri.ToString();
        var createTime = DateTimeOffset.UtcNow;

        var imageEntity = new Image
        {
            Id = imageId,
            Url = imageUrl,
            CreateTime = createTime,
            CaseId = caseId,
            ProjectId = projectId,
            ProjectName = sanitizedProjectName
        };

        await _imageRepository.AddImage(imageEntity);

        var imageDto = _mapper.Map<ImageDto>(imageEntity);

        if (imageDto == null)
        {
            throw new InvalidOperationException("Image mapping failed.");
        }

        return imageDto;
    }

    public async Task<List<ImageDto>> GetCaseImages(Guid caseId)
    {
        var images = await _imageRepository.GetImagesByCaseId(caseId);

        var imageDtos = _mapper.Map<List<ImageDto>>(images);

        if (imageDtos == null)
        {
            throw new InvalidOperationException("Image mapping failed.");
        }
        return imageDtos;
    }

    public async Task<List<ImageDto>> GetProjectImages(Guid projectId)
    {
        var images = await _imageRepository.GetImagesByProjectId(projectId);

        var imageDtos = _mapper.Map<List<ImageDto>>(images);

        if (imageDtos == null)
        {
            throw new InvalidOperationException("Image mapping failed.");
        }
        return imageDtos;
    }

    public async Task DeleteImage(Guid caseId, Guid imageId)
    {
        var image = await _imageRepository.GetImageById(imageId);
        if (image == null)
        {
            throw new NotFoundInDBException("Image not found.");
        }

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var sanitizedProjectName = SanitizeBlobName(image.ProjectName);
        var blobName = image.CaseId.HasValue
            ? $"{sanitizedProjectName}/cases/{image.CaseId}/{image.Id}"
            : $"{sanitizedProjectName}/projects/{image.ProjectId}/{image.Id}";

        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();
        await _imageRepository.DeleteImage(image);
    }
}
