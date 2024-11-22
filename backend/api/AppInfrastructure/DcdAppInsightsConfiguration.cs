using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace api.AppInfrastructure;

public static class DcdAppInsightsConfiguration
{
    public static void AddDcdAppInsights(this IServiceCollection services, IConfigurationRoot config)
    {
        var appInsightTelemetryOptions = new ApplicationInsightsServiceOptions
        {
            ConnectionString = config["ApplicationInsightInstrumentationConnectionString"],
        };

        services.AddApplicationInsightsTelemetry(appInsightTelemetryOptions);
    }
}
