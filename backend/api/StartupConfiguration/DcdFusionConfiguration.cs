namespace api.StartupConfiguration;

public static class DcdFusionConfiguration
{
    public static void AddDcdFusionConfiguration(this IServiceCollection services, string? environment, IConfigurationRoot config)
    {
        services.AddFusionIntegration(options =>
        {
            var fusionEnvironment = environment switch
            {
                "dev" => "CI",
                "qa" => "FQA",
                "prod" => "FPRD",
                "radix-prod" => "FPRD",
                "radix-qa" => "FQA",
                "radix-dev" => "CI",
                _ => "CI",
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
