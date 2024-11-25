using api.AppInfrastructure;

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

    public static readonly bool RevisionEnabled = true;//DcdEnvironments.IsLocal() || DcdEnvironments.IsCi();
}
