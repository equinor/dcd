using api.AppInfrastructure;
using api.Context;
using api.Context.Extensions;
using api.Features.Images.Shared;
using api.Models;

using Azure.Storage.Blobs;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Images.Delete;

public class DeleteCaseImageService(DcdDbContext context, BlobServiceClient blobServiceClient)
{
    public async Task DeleteImage(Guid projectId, Guid caseId, Guid imageId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var image = await context.CaseImages.SingleAsync(x => x.Case.ProjectId == projectPk && x.CaseId == caseId && x.Id == imageId);

        var blobName = ImageHelper.GetCaseBlobName(image.CaseId, image.Id);

        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();

        context.CaseImages.Remove(image);

        await context.SaveChangesAsync();
    }

    public async Task DeleteRevisionImage(CaseImage image)
    {
        var blobName = ImageHelper.GetCaseBlobName(image.CaseId, image.Id);

        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}
