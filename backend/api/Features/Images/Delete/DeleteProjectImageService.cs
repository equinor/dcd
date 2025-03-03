using api.AppInfrastructure;
using api.Context;
using api.Context.Extensions;
using api.Features.Images.Shared;
using api.Models;

using Azure.Storage.Blobs;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Images.Delete;

public class DeleteProjectImageService(DcdDbContext context, BlobServiceClient blobServiceClient)
{
    public async Task DeleteImage(Guid projectId, Guid imageId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var image = await context.ProjectImages.SingleAsync(x => x.ProjectId == projectPk && x.Id == imageId);

        var blobName = ImageHelper.GetProjectBlobName(image.ProjectId, image.Id);

        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();

        context.ProjectImages.Remove(image);

        await context.SaveChangesAsync();
    }

    public async Task DeleteRevisionImage(ProjectImage image)
    {
        var blobName = ImageHelper.GetProjectBlobName(image.ProjectId, image.Id);

        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}
