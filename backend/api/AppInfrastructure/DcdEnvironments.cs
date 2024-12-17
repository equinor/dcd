namespace api.AppInfrastructure;

public static class DcdEnvironments
{
    public static string CurrentEnvironment { get; set; } = null!;

    public const string LocalDev = "localdev";
    public const string Ci = "CI";
    public const string RadixDev = "radix-dev";
    public const string RadixQa = "radix-qa";
    public const string RadixProd = "radix-prod";

    public static bool EnableVerboseEntityFrameworkLogging => false; // CurrentEnvironment is LocalDev or Ci;
    public static bool UseSqlite => CurrentEnvironment is LocalDev;
    public static bool DisplaySwagger => CurrentEnvironment is LocalDev or Ci;
    public static bool ReturnExceptionDetails => CurrentEnvironment is LocalDev or Ci or RadixDev or RadixQa;
    public static bool AllowMigrationsToBeApplied => CurrentEnvironment is RadixDev or RadixQa or RadixProd;
    public static bool RunProjectMasterBackgroundServiceHourly => CurrentEnvironment is LocalDev or Ci or RadixDev;

    // Feature toggles likely to change frequently go below this comment.
    public static bool RevisionEnabled => CurrentEnvironment is LocalDev or Ci or RadixDev or RadixQa;
    public static bool DisplayAllFusionUsersAsPmt => CurrentEnvironment is LocalDev or Ci or RadixDev or RadixQa;
}
