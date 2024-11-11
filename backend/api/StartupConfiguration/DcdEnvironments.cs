namespace api.StartupConfiguration;

public static class DcdEnvironments
{
    public static string CurrentEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

    public const string LocalDev = "localdev";
    public const string Ci = "CI";
    public const string RadixDev = "radix-dev";
    public const string Dev = "dev";
    public const string Qa = "qa";
    public const string RadixQa = "radix-qa";
    public const string Prod = "prod";
    public const string RadixProd = "radix-prod";
}
