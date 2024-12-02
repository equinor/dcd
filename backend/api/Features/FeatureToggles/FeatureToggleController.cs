using api.AppInfrastructure.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.FeatureToggles;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class FeatureToggleController : ControllerBase
{
    [HttpGet("feature-toggles")]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public FeatureToggleDto GetFeatureToggles()
    {
        return FeatureToggleService.GetFeatureToggles();
    }
}
