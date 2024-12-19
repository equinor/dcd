using api.AppInfrastructure;

using Azure.Storage.Blobs;

namespace api.Features.Images.Copy;

public class CopyImageService(BlobServiceClient blobServiceClient)
{
    public async Task Copy(string sourcePath, string destinationPath)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(DcdEnvironments.BlobStorageContainerName);

        var sourceClient = containerClient.GetBlobClient(sourcePath);
        var destinationClient = containerClient.GetBlobClient(destinationPath);

        var operation = await destinationClient.StartCopyFromUriAsync(sourceClient.Uri);
        await operation.WaitForCompletionAsync();
    }
}
