using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class TopsideFacilitiesController : ControllerBase
    {
        private readonly ILogger<TopsideFacilitiesController> _logger;

        public TopsideFacilitiesController(ILogger<TopsideFacilitiesController> logger)
        {
            _logger = logger;
        }
    }
}
