using Azure.Storage.Blobs;

namespace api.StartupConfiguration;

public static class DcdBlogStorageConfiguration
{
    public static void AddDcdBlogStorage(this WebApplicationBuilder builder)
    {
        var azureBlobStorageConnectionString = builder.Configuration["AzureBlobStorageConnectionStringForImageUpload"];
        builder.Services.AddScoped(_ => new BlobServiceClient(azureBlobStorageConnectionString));
    }
}
