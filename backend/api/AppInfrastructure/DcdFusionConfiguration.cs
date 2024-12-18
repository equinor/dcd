namespace api.AppInfrastructure;

public static class DcdFusionConfiguration
{
    public static void AddDcdFusionConfiguration(this IServiceCollection services, IConfigurationRoot config)
    {
        services.AddFusionIntegration(options =>
        {
            options.UseServiceInformation("ConceptApp", DcdEnvironments.FusionEnvironment);

            options.UseMsalTokenProvider();
            options.UseDefaultEndpointResolver(DcdEnvironments.FusionEnvironment);
            options.UseDefaultTokenProvider(opts =>
            {
                opts.ClientId = config["AzureAd:ClientId"] ?? throw new ArgumentNullException("AzureAd:ClientId");
                opts.ClientSecret = config["AzureAd:ClientSecret"];
            });
            options.AddFusionRoles();
            options.ApplicationMode = true;
        });
    }
}
