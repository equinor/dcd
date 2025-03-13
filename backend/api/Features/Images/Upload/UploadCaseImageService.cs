using api.AppInfrastructure;
using api.Context;
using api.Context.Extensions;
using api.Features.Images.Shared;
using api.Models;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Images.Upload;

public class UploadCaseImageService(DcdDbContext context, BlobServiceClient blobServiceClient)
{
    public async Task<Guid> SaveImage(IFormFile image, Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);
        var imageCount = await CountImages(projectPk, caseId);
        UploadImageValidator.EnsureImageLimit(imageCount);

        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var imageId = Guid.NewGuid();

        var blobName = ImageHelper.GetCaseBlobName(caseId, imageId);

        var blobClient = containerClient.GetBlobClient(blobName);

        await using var stream = image.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });

        var caseItem = await context.Cases
            .Where(x => x.Id == caseId)
            .Where(x => x.ProjectId == projectPk)
            .SingleAsync();

        var imageEntity = new CaseImage
        {
            Id = imageId,
            Url = blobClient.Uri.ToString().Split('?')[0],
            CaseId = caseItem.Id,
            Description = null
        };

        context.CaseImages.Add(imageEntity);

        await context.UpdateCaseUpdatedUtc(imageEntity.CaseId);
        await context.SaveChangesAsync();

        return imageEntity.Id;
    }

    private async Task<int> CountImages(Guid projectPk, Guid caseId)
    {
        var imagesCount = await context.CaseImages
            .Include(x => x.Case)
            .Where(img => img.Case.ProjectId == projectPk && img.CaseId == caseId)
            .CountAsync();

        return imagesCount;
    }
}
