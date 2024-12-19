using Serilog;

namespace api.AppInfrastructure;

public static class DcdLogConfiguration
{
    public static void ConfigureDcdLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", DcdEnvironments.CurrentEnvironment)
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();

        builder.Host.UseSerilog();
    }
}
