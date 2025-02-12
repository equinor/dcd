using api.AppInfrastructure;
using api.Context;
using api.Context.Extensions;
using api.Features.Images.Shared;
using api.Models;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace api.Features.Images.Upload;

public class UploadCaseImageService(DcdDbContext context, BlobServiceClient blobServiceClient)
{
    public async Task<Guid> SaveImage(IFormFile image, Guid projectId, Guid caseId)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var imageId = Guid.NewGuid();

        var blobName = ImageHelper.GetCaseBlobName(caseId, imageId);

        var blobClient = containerClient.GetBlobClient(blobName);

        await using var stream = image.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });

        var imageEntity = new CaseImage
        {
            Id = imageId,
            Url = blobClient.Uri.ToString().Split('?')[0],
            CaseId = caseId,
            Description = null
        };

        context.CaseImages.Add(imageEntity);

        await context.UpdateCaseUpdatedUtc(imageEntity.CaseId);
        await context.SaveChangesAsync();

        return imageEntity.Id;
    }
}
