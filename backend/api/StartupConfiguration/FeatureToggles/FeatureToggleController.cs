using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.StartupConfiguration.FeatureToggles;

[ApiController]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class FeatureToggleController : ControllerBase
{
    [HttpGet("feature-toggles")]
    public FeatureToggleDto GetFeatureToggles()
    {
        return FeatureToggleService.GetFeatureToggles();
    }
}
