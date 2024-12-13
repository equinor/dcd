using api.Features.Images.Service;

using Azure.Storage.Blobs;

namespace api.AppInfrastructure;

public static class DcdBlogStorageConfiguration
{
    public static void AddDcdBlogStorage(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        BlobStorageService.ContainerName = GetContainerName(configuration);

        var azureBlobStorageConnectionString = builder.Configuration["AzureBlobStorageConnectionStringForImageUpload"];
        builder.Services.AddScoped(_ => new BlobServiceClient(azureBlobStorageConnectionString));
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
