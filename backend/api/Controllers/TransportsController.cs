using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class TransportsController : ControllerBase
    {
        private readonly ILogger<TransportsController> _logger;

        public TransportsController(ILogger<TransportsController> logger)
        {
            _logger = logger;
        }
    }
}
