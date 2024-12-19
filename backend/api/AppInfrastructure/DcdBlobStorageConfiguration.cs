using Azure.Storage.Blobs;

namespace api.AppInfrastructure;

public static class DcdBlobStorageConfiguration
{
    public static void AddDcdBlobStorage(this WebApplicationBuilder builder)
    {
        var blobStorageConnectionString = builder.Configuration["BlobStorageConnectionString"];
        builder.Services.AddScoped(_ => new BlobServiceClient(new Uri(blobStorageConnectionString!)));
    }
}
