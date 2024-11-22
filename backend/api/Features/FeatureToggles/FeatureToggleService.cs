using api.StartupConfiguration;

namespace api.Features.FeatureToggles;

public static class FeatureToggleService
{
    public static FeatureToggleDto GetFeatureToggles()
    {
        return new FeatureToggleDto
        {
            RevisionEnabled = RevisionEnabled,
            EnvironmentName = DcdEnvironments.CurrentEnvironment
        };
    }

    public static readonly bool RevisionEnabled = DcdEnvironments.IsLocal() || DcdEnvironments.IsCi();
}
