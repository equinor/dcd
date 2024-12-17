namespace api.AppInfrastructure;

public static class DcdEnvironments
{
    public static string CurrentEnvironment { get; set; } = null!;

    private const string LocalDev = "localdev";
    private const string Ci = "CI";

    public const string RadixDev = "radix-dev";
    public const string RadixQa = "radix-qa";
    public const string RadixProd = "radix-prod";

    private static bool IsLocal() => CurrentEnvironment is LocalDev;
    private static bool IsCi() => CurrentEnvironment is Ci or RadixDev;
    private static bool IsQa() => CurrentEnvironment is RadixQa;
    private static bool IsProd() => CurrentEnvironment is RadixProd;

    public static bool UseSqlite => IsLocal();
    public static bool EnableSwagger => IsLocal() || IsCi();
    public static bool RunProjectMasterBackgroundServiceHourly => IsProd();
    public static bool EnableVerboseEntityFrameworkLogging => false; // IsLocal() || IsCi();
    public static bool DisplayAllFusionUsersAsPmt => IsLocal() || IsCi() || IsQa();
    public static bool ReturnExceptionDetails => IsLocal() || IsCi() || IsQa();
    public static bool AllowMigrationsToBeApplied => CurrentEnvironment is RadixDev or RadixQa or RadixProd;

    // Feature toggles go here
    public static readonly bool RevisionEnabled = IsLocal() || IsCi() || IsQa();
}
