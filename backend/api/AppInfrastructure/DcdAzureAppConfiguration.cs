using Azure.Identity;

using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace api.AppInfrastructure;

public static class DcdAzureAppConfiguration
{
    public static IConfigurationRoot CreateDcdConfigurationRoot(this WebApplicationBuilder builder, string environment)
    {
        var azureAppConfigConnectionString = builder.Configuration.GetSection("AppConfiguration").GetValue<string>("ConnectionString");

        return new ConfigurationBuilder().AddAzureAppConfiguration(options =>
            options
                .Connect(azureAppConfigConnectionString)
                .ConfigureKeyVault(x => x.SetCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    ExcludeSharedTokenCacheCredential = true
                })))
                .Select(KeyFilter.Any)
                .Select(KeyFilter.Any, environment)
        ).Build();
    }

    public static void AddDcdAzureAppConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddAzureAppConfiguration(options =>
        {
            var azureAppConfigConnectionString = builder.Configuration["AppConfiguration:ConnectionString"];
            options.Connect(azureAppConfigConnectionString)
                .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()));
        });
    }

        public static string GetCiBlobStorageSasToken(this IConfiguration configuration)
    {
        var sasToken = configuration["CI-blob-storage-sas-token"];
        if (string.IsNullOrEmpty(sasToken))
        {
            throw new InvalidOperationException("SAS token for CI environment is not configured.");
        }
        return sasToken;
    }
}
