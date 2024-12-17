using Azure.Storage;
using Azure.Storage.Blobs;

namespace api.AppInfrastructure;

public static class DcdBloBStorageConfiguration
{
    public static void AddDcdBloBStorage(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var azureBlobStorageConnectionString = "https://dcdstorageaccount.blob.core.windows.net/ci-image-storage?sp=racwd&st=2024-12-17T08:17:59Z&se=2024-12-20T16:17:59Z&spr=https&sv=2022-11-02&sr=c&sig=Mj8XB4xIioMStfPRgr5dC5bkPYpThzlV2ZywbgpLPRU%3D";
        builder.Services.AddScoped(_ => new BlobServiceClient(new Uri(azureBlobStorageConnectionString!)));

    }
}
