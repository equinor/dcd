using api.Features.Images.Service;

using Azure.Storage;
using Azure.Storage.Blobs;

namespace api.AppInfrastructure;

public static class DcdBlobStorageConfiguration
{
    public static void AddDcdBlobStorage(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        BlobStorageService.ContainerName = GetContainerName(configuration);

        var azureBlobStorageConnectionString = "https://dcdstorageaccount.blob.core.windows.net/ci-image-storage?sp=racwd&st=2024-12-17T08:17:59Z&se=2024-12-20T16:17:59Z&spr=https&sv=2022-11-02&sr=c&sig=Mj8XB4xIioMStfPRgr5dC5bkPYpThzlV2ZywbgpLPRU%3D";
        builder.Services.AddScoped(_ => new BlobServiceClient(new Uri(azureBlobStorageConnectionString!)));

    }

    private static string GetContainerName(IConfiguration configuration)
    {
        var containerKey = DcdEnvironments.CurrentEnvironment switch
        {
            DcdEnvironments.LocalDev => "AzureStorageAccountImageContainerCI",
            DcdEnvironments.Ci => "AzureStorageAccountImageContainerCI",

            DcdEnvironments.Dev => "AzureStorageAccountImageContainerCI",
            DcdEnvironments.RadixDev => "AzureStorageAccountImageContainerCI",

            DcdEnvironments.Qa => "AzureStorageAccountImageContainerQA",
            DcdEnvironments.RadixQa => "AzureStorageAccountImageContainerQA",

            DcdEnvironments.Prod => "AzureStorageAccountImageContainerProd",
            DcdEnvironments.RadixProd => "AzureStorageAccountImageContainerProd",

            _ => throw new InvalidOperationException($"Unknown fusion environment: {DcdEnvironments.CurrentEnvironment}")
        };

        return configuration[containerKey]
               ?? throw new InvalidOperationException($"Container name configuration for {DcdEnvironments.CurrentEnvironment} is missing.");
    }
}
