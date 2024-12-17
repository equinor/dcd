namespace api.AppInfrastructure;

public static class DcdEnvironments
{
    public static string CurrentEnvironment { get; set; } = null!;

    private const string LocalDev = "localdev";
    private const string Ci = "CI";

    public const string RadixDev = "radix-dev";
    public const string RadixQa = "radix-qa";
    public const string RadixProd = "radix-prod";

    public static bool UseSqlite => CurrentEnvironment is LocalDev;
    public static bool EnableSwagger => CurrentEnvironment is LocalDev or Ci or RadixDev;
    public static bool RunProjectMasterBackgroundServiceHourly => CurrentEnvironment is not RadixProd;
    public static bool EnableVerboseEntityFrameworkLogging => false; // CurrentEnvironment is LocalDev || CurrentEnvironment is Ci or RadixDev;
    public static bool ReturnExceptionDetails => CurrentEnvironment is LocalDev or Ci or RadixDev or RadixQa;
    public static bool AllowMigrationsToBeApplied => CurrentEnvironment is RadixDev or RadixQa or RadixProd;

    // Feature toggles go here
    public static bool RevisionEnabled => CurrentEnvironment is LocalDev or Ci or RadixDev or RadixQa;
    public static bool DisplayAllFusionUsersAsPmt => CurrentEnvironment is LocalDev or Ci or RadixDev or RadixQa;
}
