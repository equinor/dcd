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
    public static bool ReturnExceptionDetails => CurrentEnvironment is not RadixProd;
    public static bool AllowMigrationsToBeApplied => CurrentEnvironment is RadixDev or RadixQa or RadixProd;
    public static bool RunBackgroundJobsOnLocalMachine => CurrentEnvironment is LocalDev or Ci;
    public static bool WriteTypescriptFiles => CurrentEnvironment is LocalDev or Ci;
    public static bool ThrowCustomExceptionWhenNotLoggedInToAzure => CurrentEnvironment is LocalDev or Ci;

    public static string FusionEnvironment => CurrentEnvironment switch
    {
        RadixProd => "FPRD",
        RadixQa => "FQA",
        _ => "CI"
    };

    public static string BlobStorageContainerName => CurrentEnvironment switch
    {
        RadixProd => "prod-image-storage",
        RadixQa => "qa-image-storage",
        _ => "ci-image-storage"
    };

    public static class FeatureToggles
    {
        public static bool RevisionEnabled => true;
    }
}
