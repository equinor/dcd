namespace api.AppInfrastructure;

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
                        "https://fusion.ci.fusion-dev.net",
                        "https://fusion.fqa.fusion-dev.net"
                    ).SetIsOriginAllowedToAllowWildcardSubdomains();
                });
        });
    }
}
