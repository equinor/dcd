using api.Features.Images.Service;

using Azure.Storage.Blobs;

namespace api.AppInfrastructure;

public static class DcdBloBStorageConfiguration
{
    public static void AddDcdBloBStorage(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var azureBlobStorageConnectionString = "https://dcdstorageaccount.blob.core.windows.net/ci-image-storage?sp=r&st=2024-12-16T13:01:57Z&se=2024-12-16T21:01:57Z&spr=https&sv=2022-11-02&sr=c&sig=d3OsTk8jfNwH05waTA10o3Zb%2Fs6nkjRU3YftuDP5d4o%3D";
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
