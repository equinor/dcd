namespace api.AppInfrastructure;

public static class DcdEnvironments
{
    public static string CurrentEnvironment { get; set; } = null!;

    public const string LocalDev = "localdev";
    public const string Ci = "CI";

    public const string RadixDev = "radix-dev";
    public const string RadixQa = "radix-qa";
    public const string RadixProd = "radix-prod";

    public static bool IsLocal() => CurrentEnvironment is LocalDev;
    public static bool IsCi() => CurrentEnvironment is Ci or RadixDev;
    public static bool IsQa() => CurrentEnvironment is RadixQa;
    public static bool IsProd() => CurrentEnvironment is RadixProd;

    public static bool EnableVerboseEntityFrameworkLogging => false; // IsLocal() || IsCi();
    public static bool DisplayAllFusionUsersAsPmt => IsLocal() || IsCi() || IsQa();
    public static bool ReturnExceptionDetails => IsLocal() || IsCi() || IsQa();
    public static bool AllowMigrationsToBeApplied => CurrentEnvironment is RadixDev or RadixQa or RadixProd;
}
