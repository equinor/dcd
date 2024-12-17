namespace api.AppInfrastructure;

public static class DcdFusionConfiguration
{
    public static void AddDcdFusionConfiguration(this IServiceCollection services, IConfigurationRoot azureConfig, IConfigurationRoot localConfig)
    {
        services.AddFusionIntegration(options =>
        {
            var fusionEnvironment = DcdEnvironments.CurrentEnvironment switch
            {
                DcdEnvironments.Dev => "CI",
                DcdEnvironments.RadixDev => "CI",

                DcdEnvironments.Qa => "FQA",
                DcdEnvironments.RadixQa => "FQA",

                DcdEnvironments.Prod => "FPRD",
                DcdEnvironments.RadixProd => "FPRD",

                _ => "CI"
            };

            Console.WriteLine("Fusion environment: " + fusionEnvironment);
            options.UseServiceInformation("ConceptApp", fusionEnvironment);

            options.UseMsalTokenProvider();
            options.UseDefaultEndpointResolver(fusionEnvironment);
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
