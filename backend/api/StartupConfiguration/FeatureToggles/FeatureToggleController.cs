using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

using api.Authorization;

namespace api.StartupConfiguration.FeatureToggles;

[ApiController]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
public class FeatureToggleController : ControllerBase
{
    [HttpGet("feature-toggles")]
    public FeatureToggleDto GetFeatureToggles()
    {
        return FeatureToggleService.GetFeatureToggles();
    }
}
