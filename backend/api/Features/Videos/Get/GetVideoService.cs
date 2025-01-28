using Azure.Storage.Blobs;

namespace api.Features.Videos.Get;

public class GetVideoService(BlobServiceClient blobServiceClient)
{
    public async Task<VideoDto> GetVideoStream(string videoName)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient("videos");
        var blobClient = blobContainerClient.GetBlobClient(videoName);
        var response = await blobClient.DownloadAsync();

        using var memoryStream = new MemoryStream();
        await response.Value.Content.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();

        return new VideoDto
        {
            VideoName = videoName,
            Base64EncodedData = "data:video/mp4;base64," + Convert.ToBase64String(bytes)
        };
    }
}
