namespace api.StartupConfiguration.FeatureToggles;

public static class FeatureToggleService
{
    public static FeatureToggleDto GetFeatureToggles()
    {
        return new FeatureToggleDto
        {
            FeatureAEnabled = FeatureAEnabled,
            FeatureBEnabled = FeatureBEnabled,
            FeatureCEnabled = FeatureCEnabled
        };
    }

    public static readonly bool FeatureAEnabled = DcdEnvironments.CurrentEnvironment is DcdEnvironments.LocalDev;

    public static readonly bool FeatureBEnabled = DcdEnvironments.CurrentEnvironment is DcdEnvironments.LocalDev;

    public static readonly bool FeatureCEnabled = DcdEnvironments.CurrentEnvironment is DcdEnvironments.LocalDev
        or DcdEnvironments.Dev
        or DcdEnvironments.Qa;
}
