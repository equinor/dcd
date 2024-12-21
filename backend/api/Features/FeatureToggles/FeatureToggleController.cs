using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.FeatureToggles;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class FeatureToggleController : ControllerBase
{
    [HttpGet("feature-toggles")]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public FeatureToggleDto GetFeatureToggles()
    {
        return FeatureToggleService.GetFeatureToggles();
    }
}
