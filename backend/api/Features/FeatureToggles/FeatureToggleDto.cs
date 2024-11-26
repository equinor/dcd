namespace api.Features.FeatureToggles;

public class FeatureToggleDto
{
    public bool RevisionEnabled { get; set; }
    public string? EnvironmentName { get; set; }
}
