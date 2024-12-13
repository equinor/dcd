using Azure.Storage.Blobs;

namespace api.AppInfrastructure;

public static class DcdBloBStorageConfiguration
{
    public static void AddDcdBloBStorage(this WebApplicationBuilder builder)
    {
        // new BlobServiceClient
        var azureBlobStorageConnectionString = builder.Configuration["AzureBlobStorageConnectionStringForImageUpload"];
        builder.Services.AddScoped(_ => new BlobServiceClient(azureBlobStorageConnectionString));
    }
}
