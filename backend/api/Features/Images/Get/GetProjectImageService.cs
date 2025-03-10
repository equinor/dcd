using api.AppInfrastructure;
using api.Context;
using api.Context.Extensions;
using api.Features.Images.Shared;
using api.Models;

using Azure.Storage.Blobs;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Images.Get;

public class GetProjectImageService(DcdDbContext context, BlobServiceClient blobServiceClient)
{
    public async Task<ImageDto> GetImage(Guid imageId)
    {
        var image = await context.ProjectImages.SingleAsync(img => img.Id == imageId);

        var imageContent = await GetImageContent(image);

        return MapToDto(image, imageContent);
    }

    public async Task<List<ImageDto>> GetImages(Guid projectId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        var images = await context.ProjectImages
            .Where(img => img.ProjectId == projectPk)
            .OrderBy(c => c.CreatedUtc)
            .ToListAsync();

        var dtos = new List<ImageDto>();

        foreach (var image in images)
        {
            var imageContent = await GetImageContent(image);
            dtos.Add(MapToDto(image, imageContent));
        }

        return dtos;
    }

    private async Task<string> GetImageContent(ProjectImage image)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var blobName = ImageHelper.GetProjectBlobName(image.ProjectId, image.Id);

        var blobClient = containerClient.GetBlobClient(blobName);

        var response = await blobClient.DownloadAsync();

        using var memoryStream = new MemoryStream();
        await response.Value.Content.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();

        return "data:image/jpeg;base64, " + Convert.ToBase64String(bytes);
    }

    private static ImageDto MapToDto(ProjectImage imageEntity, string base64EncodedImage)
    {
        return new ImageDto
        {
            ImageId = imageEntity.Id,
            CreateTime = imageEntity.CreatedUtc,
            Description = imageEntity.Description,
            CaseId = null,
            ProjectId = imageEntity.ProjectId,
            ImageData = base64EncodedImage
        };
    }
}
