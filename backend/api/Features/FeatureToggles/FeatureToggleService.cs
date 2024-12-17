using api.AppInfrastructure;

namespace api.Features.FeatureToggles;

public static class FeatureToggleService
{
    public static FeatureToggleDto GetFeatureToggles()
    {
        return new FeatureToggleDto
        {
            RevisionEnabled = DcdEnvironments.RevisionEnabled,
            EnvironmentName = DcdEnvironments.CurrentEnvironment
        };
    }
}
