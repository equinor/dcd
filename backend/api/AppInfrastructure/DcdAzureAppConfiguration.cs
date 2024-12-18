using Azure.Identity;

using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace api.AppInfrastructure;

public static class DcdAzureAppConfiguration
{
    public static void AddDcdAzureAppConfiguration(this WebApplicationBuilder builder)
    {
        var azureAppConfigConnectionString = builder.Configuration["AppConfiguration:ConnectionString"];

        builder.Configuration.AddConfiguration(new ConfigurationBuilder().AddAzureAppConfiguration(options =>
            options.Connect(azureAppConfigConnectionString)
                .ConfigureKeyVault(x => x.SetCredential(new DefaultAzureCredential()))
                .Select(KeyFilter.Any)
                .Select(KeyFilter.Any, DcdEnvironments.CurrentEnvironment)
        ).Build());
    }
}
