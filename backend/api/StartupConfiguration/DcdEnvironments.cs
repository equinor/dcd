namespace api.StartupConfiguration;

public static class DcdEnvironments
{
    public static string CurrentEnvironment { get; set; } = null!;

    public const string LocalDev = "localdev";

    public const string Ci = "CI";

    public const string Dev = "dev";
    public const string RadixDev = "radix-dev";

    public const string Qa = "qa";
    public const string RadixQa = "radix-qa";

    public const string Prod = "prod";
    public const string RadixProd = "radix-prod";

    public static bool EnableVerboseEntityFrameworkLogging => false; // IsLocal() || IsCi();

    public static bool IsLocal() => CurrentEnvironment is LocalDev;
    public static bool IsCi() => CurrentEnvironment is Ci or Dev or RadixDev;
    public static bool IsQa() => CurrentEnvironment is Qa or RadixQa;
    public static bool IsProd() => CurrentEnvironment is Prod or RadixProd;
}
