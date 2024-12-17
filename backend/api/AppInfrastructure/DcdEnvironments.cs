namespace api.AppInfrastructure;

public static class DcdEnvironments
{
    public static string CurrentEnvironment { get; set; } = null!;

    private const string LocalDev = "localdev";
    private const string Ci = "CI";
    private const string RadixDev = "radix-dev";
    private const string RadixQa = "radix-qa";
    private const string RadixProd = "radix-prod";

    public static bool UseSqlite => CurrentEnvironment is LocalDev;
    public static bool EnableSwagger => CurrentEnvironment is LocalDev or Ci or RadixDev;
    public static bool RunProjectMasterBackgroundServiceHourly => CurrentEnvironment is not RadixProd;
    public static bool EnableVerboseEntityFrameworkLogging => false; // CurrentEnvironment is LocalDev;
    public static bool ReturnExceptionDetails => CurrentEnvironment is not RadixProd;
    public static bool AllowMigrationsToBeApplied => CurrentEnvironment is RadixDev or RadixQa or RadixProd;

    // Feature toggles go here
    public static bool RevisionEnabled => CurrentEnvironment is not RadixProd;
    public static bool DisplayAllFusionUsersAsPmt => CurrentEnvironment is not RadixProd;

    public static string FusionEnvironment => CurrentEnvironment switch
    {
        RadixDev => "CI",
        RadixQa => "FQA",
        RadixProd => "FPRD",

        _ => "CI"
    };

    public static string BlobStorageContainerName => CurrentEnvironment switch
    {
        RadixDev => "ci-image-storage",
        RadixQa => "qa-image-storage",
        RadixProd => "prod-image-storage",

        _ => "ci-image-storage"
    };
}
