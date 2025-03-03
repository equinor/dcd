namespace api.AppInfrastructure;

public static class DcdFusionConfiguration
{
    public static void AddDcdFusionConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddFusionIntegration(options =>
        {
            options.UseServiceInformation("ConceptApp", DcdEnvironments.FusionEnvironment);

            options.UseMsalTokenProvider();
            options.UseDefaultEndpointResolver(DcdEnvironments.FusionEnvironment);

            options.UseDefaultTokenProvider(opts =>
            {
                opts.ClientId = builder.Configuration["AzureAd:ClientId"]!;
                opts.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
            });

            options.AddFusionRoles();
            options.ApplicationMode = true;
        });
    }
}
