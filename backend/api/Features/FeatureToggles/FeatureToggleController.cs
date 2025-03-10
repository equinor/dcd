using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.FeatureToggles;

public class FeatureToggleController : ControllerBase
{
    [HttpGet("feature-toggles")]
    [Authorize]
    public FeatureToggleDto GetFeatureToggles()
    {
        return FeatureToggleService.GetFeatureToggles();
    }
}
