using api.AppInfrastructure;
using api.Context;
using api.Exceptions;
using api.Features.CaseProfiles.Repositories;
using api.Features.Images.Dto;
using api.Models;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Images.Service;

public class BlobStorageService(BlobServiceClient blobServiceClient,
    ICaseRepository caseRepository,
    IConfiguration configuration,
    DcdDbContext context)
    : IBlobStorageService
{
    private readonly string _containerName = GetContainerName(configuration);

    private static string GetContainerName(IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("AppConfiguration__Environment") ?? "default";

        var containerKey = environment switch
        {
            DcdEnvironments.LocalDev => "AzureStorageAccountImageContainerCI",
            DcdEnvironments.Ci => "AzureStorageAccountImageContainerCI",

            DcdEnvironments.Dev => "AzureStorageAccountImageContainerCI",
            DcdEnvironments.RadixDev => "AzureStorageAccountImageContainerCI",

            DcdEnvironments.Qa => "AzureStorageAccountImageContainerQA",
            DcdEnvironments.RadixQa => "AzureStorageAccountImageContainerQA",

            DcdEnvironments.Prod => "AzureStorageAccountImageContainerProd",
            DcdEnvironments.RadixProd => "AzureStorageAccountImageContainerProd",
            _ => throw new InvalidOperationException($"Unknown fusion environment: {environment}")
        };

        return configuration[containerKey]
               ?? throw new InvalidOperationException($"Container name configuration for {environment} is missing.");
    }

    private static string SanitizeBlobName(string name)
    {
        return name.Replace(" ", "-").Replace("/", "-").Replace("\\", "-");
    }

    public async Task<ImageDto> SaveImage(Guid projectId, string projectName, IFormFile image, Guid? caseId = null)
    {
        // var sasToken = configuration["CI-blob-storage-sas-token"] ?? string.Empty;
        var sanitizedProjectName = SanitizeBlobName(projectName);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

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

        context.Images.Add(imageEntity);

        if (imageEntity.CaseId.HasValue)
        {
            await caseRepository.UpdateModifyTime((Guid)imageEntity.CaseId);
        }

        await context.SaveChangesAsync();

        return MapToDto(imageEntity);
    }

    public async Task<List<ImageDto>> GetCaseImages(Guid caseId)
    {
        Console.WriteLine("GetCaseImages");
        var sasToken = configuration["CI-blob-storage-sas-token"];
        if (sasToken != null)
        {
            var images = await context.Images
                .Where(img => img.CaseId == caseId)
                .ToListAsync();
            Console.WriteLine("images.Count: " + images.Count);

            return images.Select(img => MapToDto(img)).ToList();
        }
        else
        {
            throw new InvalidOperationException("SAS Token not found in configuration.");
        }
    }

    public async Task<List<ImageDto>> GetProjectImages(Guid projectId)
    {
        Console.WriteLine("GetProjectImages");
        var sasToken = configuration["CI-blob-storage-sas-token"];
        if (sasToken != null)
        {
            var images = await context.Images
                .Where(img => img.ProjectId == projectId && img.CaseId == null)
                .ToListAsync();
            Console.WriteLine("images.Count: " + images.Count);
            return images.Select(img => MapToDto(img)).ToList();
        }
        else
        {
            throw new InvalidOperationException("SAS Token not found in configuration.");
        }
    }


    public async Task DeleteImage(Guid imageId)
    {
        var image = await context.Images.FindAsync(imageId);
        if (image == null)
        {
            throw new NotFoundInDBException("Image not found.");
        }

        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        var sanitizedProjectName = SanitizeBlobName(image.ProjectName);
        var blobName = image.CaseId.HasValue
            ? $"{sanitizedProjectName}/cases/{image.CaseId}/{image.Id}"
            : $"{sanitizedProjectName}/projects/{image.ProjectId}/{image.Id}";

        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();

        context.Images.Remove(image);
        await context.SaveChangesAsync();
    }

    public async Task<string> GetImageRaw(Guid projectId, Guid imageId)
    {
        var image = await context.Images.FindAsync(imageId);
        if (image == null)
        {
            throw new NotFoundInDBException("Image not found.");
        }

        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        var sanitizedProjectName = SanitizeBlobName(image.ProjectName);
        var blobName = image.CaseId.HasValue
            ? $"{sanitizedProjectName}/cases/{image.CaseId}/{image.Id}"
            : $"{sanitizedProjectName}/projects/{projectId}/{image.Id}";

        var blobClient = containerClient.GetBlobClient(blobName);

        var response = await blobClient.DownloadAsync();
        var stream = response.Value.Content;

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        return Convert.ToBase64String(bytes);
    }

    private static ImageDto MapToDto(Image imageEntity)
    {
        return new ImageDto
        {
            Id = imageEntity.Id,
            Url = imageEntity.Url,
            CreateTime = imageEntity.CreateTime,
            Description = imageEntity.Description,
            CaseId = imageEntity.CaseId,
            ProjectName = imageEntity.ProjectName,
            ProjectId = imageEntity.ProjectId
        };
    }
}
