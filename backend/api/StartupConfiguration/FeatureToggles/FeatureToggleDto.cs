using System.ComponentModel.DataAnnotations;

namespace api.StartupConfiguration.FeatureToggles;

public class FeatureToggleDto
{
    [Required]
    public bool FeatureAEnabled { get; set; }
    [Required]
    public bool FeatureBEnabled { get; set; }
    [Required]
    public bool FeatureCEnabled { get; set; }
}
