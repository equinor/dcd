using Serilog;

namespace api.AppInfrastructure;

public static class DcdLogConfiguration
{
    public static void ConfigureDcdLogging(string environment, IConfigurationRoot config)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .ReadFrom.Configuration(config)
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", environment)
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();
    }
}
