using Azure.Storage.Blobs;

namespace api.AppInfrastructure;

public static class DcdBlobStorageConfiguration
{
    public static void AddDcdBlobStorage(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var blobStorageConnectionString = configuration["BlobStorageConnectionString"];
        builder.Services.AddScoped(_ => new BlobServiceClient(new Uri(blobStorageConnectionString!)));
    }
}
