using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace api.AppInfrastructure;

public static class DcdAppInsightsConfiguration
{
    public static void AddDcdAppInsights(this WebApplicationBuilder builder)
    {
        var appInsightTelemetryOptions = new ApplicationInsightsServiceOptions
        {
            ConnectionString = builder.Configuration["ApplicationInsightInstrumentationConnectionString"]
        };

        builder.Services.AddApplicationInsightsTelemetry(appInsightTelemetryOptions);
    }
}
