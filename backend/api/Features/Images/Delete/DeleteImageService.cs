using api.AppInfrastructure;
using api.Context;
using api.Context.Extensions;
using api.Features.Images.Shared;

using Azure.Storage.Blobs;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Images.Delete;

public class DeleteImageService(DcdDbContext context, BlobServiceClient blobServiceClient)
{
    public async Task DeleteImage(Guid projectId, Guid imageId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var image = await context.Images.SingleAsync(x => x.ProjectId == projectPk && x.Id == imageId);

        var blobName = ImageHelper.GetBlobName(image.CaseId, image.ProjectId, image.Id);

        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();

        context.Images.Remove(image);

        await context.SaveChangesAsync();
    }
}
