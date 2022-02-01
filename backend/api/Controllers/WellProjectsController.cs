using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WellProjectsController : ControllerBase
    {
        private WellProjectService _wellProjectService;
        private readonly ILogger<WellProjectsController> _logger;

        public WellProjectsController(ILogger<WellProjectsController> logger, WellProjectService wellProjectService)
        {
            _logger = logger;
            _wellProjectService = wellProjectService;
        }
    }
}
