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
    DcdDbContext context)
    : IBlobStorageService
{
    public async Task<ImageDto> SaveImage(Guid projectId, IFormFile image, Guid? caseId = null)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var imageId = Guid.NewGuid();

        if (projectId == Guid.Empty || caseId == Guid.Empty)
        {
            throw new ArgumentException("ProjectId and/or CaseId cannot be empty.");
        }

        var blobName = GetBlobName(caseId, projectId, imageId);

        var blobClient = containerClient.GetBlobClient(blobName);

        await using var stream = image.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });

        var imageUrl = blobClient.Uri.ToString().Split('?')[0];
        var createTime = DateTimeOffset.UtcNow;

        var imageEntity = new Image
        {
            Id = imageId,
            Url = imageUrl,
            CreateTime = createTime,
            CaseId = caseId,
            ProjectId = projectId
        };

        context.Images.Add(imageEntity);

        if (imageEntity.CaseId.HasValue)
        {
            await caseRepository.UpdateModifyTime((Guid)imageEntity.CaseId);
        }

        await context.SaveChangesAsync();

        var imageContent = await GetImageContent(imageEntity);

        var dto = MapToDto(imageEntity, imageContent);

        return dto;
    }

    public async Task<List<ImageDto>> GetCaseImages(Guid caseId)
    {
        var images = await context.Images
            .Where(img => img.CaseId == caseId)
            .ToListAsync();

        var dtos = new List<ImageDto>();

        foreach (var image in images)
        {
            var imageContent = await GetImageContent(image);
            dtos.Add(MapToDto(image, imageContent));
        }

        return dtos;
    }

    public async Task<List<ImageDto>> GetProjectImages(Guid projectId)
    {
        var images = await context.Images
            .Where(img => img.ProjectId == projectId && img.CaseId == null)
            .ToListAsync();

        var dtos = new List<ImageDto>();

        foreach (var image in images)
        {
            var imageContent = await GetImageContent(image);
            dtos.Add(MapToDto(image, imageContent));
        }

        return dtos;
    }

    public async Task DeleteImage(Guid imageId)
    {
        var image = await context.Images.FindAsync(imageId);
        if (image == null)
        {
            throw new NotFoundInDBException("Image not found.");
        }

        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var blobName = GetBlobName(image.CaseId, image.ProjectId, image.Id);

        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();

        context.Images.Remove(image);
        await context.SaveChangesAsync();
    }

    private async Task<string> GetImageContent(Image image)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var blobName = GetBlobName(image.CaseId, image.ProjectId, image.Id);

        var blobClient = containerClient.GetBlobClient(blobName);

        var response = await blobClient.DownloadAsync();
        var stream = response.Value.Content;

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();

        return "data:image/jpeg;base64, " + Convert.ToBase64String(bytes);
    }

    private static string GetBlobName(Guid? caseId, Guid projectId, Guid imageId)
    {
        return caseId.HasValue
            ? $"cases/{caseId}/{imageId}"
            : $"projects/{projectId}/{imageId}";
    }

    private static ImageDto MapToDto(Image imageEntity, string base64EncodedImage)
    {
        return new ImageDto
        {
            ImageId = imageEntity.Id,
            CreateTime = imageEntity.CreateTime,
            Description = imageEntity.Description,
            CaseId = imageEntity.CaseId,
            ProjectId = imageEntity.ProjectId,
            ImageData = base64EncodedImage
        };
    }
}
