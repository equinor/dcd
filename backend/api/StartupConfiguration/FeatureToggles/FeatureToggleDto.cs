using System.ComponentModel.DataAnnotations;

namespace api.StartupConfiguration.FeatureToggles;

public class FeatureToggleDto
{
    public bool RevisionEnabled { get; set; }
    public string? EnvironmentName { get; set; }
}
