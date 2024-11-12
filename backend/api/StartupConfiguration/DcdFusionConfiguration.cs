namespace api.StartupConfiguration;

public static class DcdFusionConfiguration
{
    public static void AddDcdFusionConfiguration(this IServiceCollection services, string? environment, IConfigurationRoot config)
    {
        services.AddFusionIntegration(options =>
        {
            var fusionEnvironment = environment switch
            {
                DcdEnvironments.Dev => "CI",
                DcdEnvironments.Qa => "FQA",
                DcdEnvironments.Prod => "FPRD",
                DcdEnvironments.RadixProd => "FPRD",
                DcdEnvironments.RadixQa => "FQA",
                DcdEnvironments.RadixDev => "CI",
                _ => "CI"
            };

            Console.WriteLine("Fusion environment: " + fusionEnvironment);
            options.UseServiceInformation("ConceptApp", fusionEnvironment);

            options.UseDefaultEndpointResolver(fusionEnvironment);
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
