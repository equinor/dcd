namespace api.StartupConfiguration;

public static class DcdCorsPolicyConfiguration
{
    public const string AccessControlPolicyName = "AllowSpecificOrigins";

    public static void AddDcdCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(AccessControlPolicyName,
                builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.WithExposedHeaders("Location");
                    builder.WithOrigins(
                        "http://localhost:3000",
                        "https://fusion.equinor.com",
                        "https://pro-s-portal-ci.azurewebsites.net",
                        "https://pro-s-portal-fqa.azurewebsites.net",
                        "https://pro-s-portal-fprd.azurewebsites.net",
                        "https://fusion-s-portal-ci.azurewebsites.net",
                        "https://fusion-s-portal-fqa.azurewebsites.net",
                        "https://fusion-s-portal-fprd.azurewebsites.net",
                        "https://pr-3422.fusion-dev.net",
                        "https://pr-*.fusion-dev.net"
                    ).SetIsOriginAllowedToAllowWildcardSubdomains();
                });
        });
    }
}
