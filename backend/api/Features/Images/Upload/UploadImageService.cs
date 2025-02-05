using api.AppInfrastructure;
using api.Context;
using api.Context.Extensions;
using api.Features.Images.Shared;
using api.Models;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace api.Features.Images.Upload;

public class UploadImageService(DcdDbContext context, BlobServiceClient blobServiceClient)
{
    public async Task<Guid> SaveImage(IFormFile image, Guid projectId, Guid? caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var imageId = Guid.NewGuid();

        var blobName = ImageHelper.GetBlobName(caseId, projectPk, imageId);

        var blobClient = containerClient.GetBlobClient(blobName);

        await using var stream = image.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });

        var imageEntity = new Image
        {
            Id = imageId,
            Url = blobClient.Uri.ToString().Split('?')[0],
            CaseId = caseId,
            ProjectId = projectPk,
            Description = null
        };

        context.Images.Add(imageEntity);

        if (imageEntity.CaseId.HasValue)
        {
            await context.UpdateCaseUpdatedUtc(imageEntity.CaseId!.Value);
        }

        await context.SaveChangesAsync();

        return imageEntity.Id;
    }
}
