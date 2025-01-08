using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.FeatureToggles;

public class FeatureToggleController : ControllerBase
{
    [HttpGet("feature-toggles")]
    [Authorize]
    [DisableLazyLoading]
    public FeatureToggleDto GetFeatureToggles()
    {
        return FeatureToggleService.GetFeatureToggles();
    }
}
