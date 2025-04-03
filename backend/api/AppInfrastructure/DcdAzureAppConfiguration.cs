using Azure.Identity;

using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace api.AppInfrastructure;

public static class DcdAzureAppConfiguration
{
    public static void AddDcdAzureAppConfiguration(this WebApplicationBuilder builder)
    {
        var azureAppConfigConnectionString = builder.Configuration["AppConfiguration:ConnectionString"];

        var configuration = new ConfigurationBuilder().AddAzureAppConfiguration(
            options => options.Connect(azureAppConfigConnectionString)
                .ConfigureKeyVault(x => x.SetCredential(new DefaultAzureCredential()))
                .Select(KeyFilter.Any)
                .Select(KeyFilter.Any, DcdEnvironments.CurrentEnvironment)
        );

        try
        {
            var builtConfiguration = configuration.Build();

            builder.Configuration.AddConfiguration(builtConfiguration);
        }
        catch (KeyVaultReferenceException)
        {
            if (DcdEnvironments.ThrowCustomExceptionWhenNotLoggedInToAzure)
            {
                throw new Exception("You need to run \"az login\" in a terminal on your developer machine.");
            }

            throw;
        }
    }
}
