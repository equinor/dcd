using Azure.Identity;

using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace api.StartupConfiguration;

public static class DcdAzureAppConfiguration
{
    public static IConfigurationRoot CreateDcdConfigurationRoot(this WebApplicationBuilder builder, string? environment)
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
                .ConfigureKeyVault(kv =>
                {
                    kv.SetCredential(new DefaultAzureCredential());
                });
        });
    }
}
