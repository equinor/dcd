using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class SubstructuresController : ControllerBase
    {
        private readonly ILogger<SubstructuresController> _logger;

        public SubstructuresController(ILogger<SubstructuresController> logger)
        {
            _logger = logger;
        }
    }
}
