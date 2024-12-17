namespace api.AppInfrastructure;

public static class DcdFusionConfiguration
{
    public static void AddDcdFusionConfiguration(this IServiceCollection services, IConfigurationRoot azureConfig, IConfigurationRoot localConfig)
    {
        services.AddFusionIntegration(options =>
        {
            options.UseServiceInformation("ConceptApp", DcdEnvironments.FusionEnvironment);

            options.UseMsalTokenProvider();
            options.UseDefaultEndpointResolver(DcdEnvironments.FusionEnvironment);
            options.UseDefaultTokenProvider(opts =>
            {
                opts.ClientId = azureConfig["AzureAd:ClientId"] ?? localConfig["AzureAd:ClientId"] ?? throw new ArgumentNullException("AzureAd:ClientId");
                opts.ClientSecret = azureConfig["AzureAd:ClientSecret"] ?? localConfig["AzureAd:ClientSecret"];
            });
            options.AddFusionRoles();
            options.ApplicationMode = true;
        });
    }
}
