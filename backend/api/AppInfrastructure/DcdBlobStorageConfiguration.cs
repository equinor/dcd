using api.Features.Images.Service;

using Azure.Storage.Blobs;

namespace api.AppInfrastructure;

public static class DcdBlobStorageConfiguration
{
    public static void AddDcdBlobStorage(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        BlobStorageService.ContainerName = GetContainerName(configuration);

        var blobStorageConnectionString = configuration["BlobStorageConnectionString"];
        builder.Services.AddScoped(_ => new BlobServiceClient(new Uri(blobStorageConnectionString!)));
    }

    private static string GetContainerName(IConfiguration configuration)
    {
        var containerKey = DcdEnvironments.CurrentEnvironment switch
        {
            DcdEnvironments.RadixDev => "AzureStorageAccountImageContainerCI",
            DcdEnvironments.RadixQa => "AzureStorageAccountImageContainerQA",
            DcdEnvironments.RadixProd => "AzureStorageAccountImageContainerProd",

            _ => "AzureStorageAccountImageContainerCI"
        };

        return configuration[containerKey]
               ?? throw new InvalidOperationException($"Container name configuration for {DcdEnvironments.CurrentEnvironment} is missing.");
    }
}
