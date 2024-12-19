using api.AppInfrastructure;

namespace api.Features.FeatureToggles;

public static class FeatureToggleService
{
    public static FeatureToggleDto GetFeatureToggles()
    {
        return new FeatureToggleDto
        {
            RevisionEnabled = DcdEnvironments.FeatureToggles.RevisionEnabled,
            EnvironmentName = DcdEnvironments.CurrentEnvironment
        };
    }
}
