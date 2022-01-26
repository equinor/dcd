using api.Models;
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
    public class ExplorationController : ControllerBase
    {
        private ExplorationService _explorationService;
        private readonly ILogger<ExplorationController> _logger;

        public ExplorationController(ILogger<ExplorationController> logger, ExplorationService explorationProjectService)
        {
            _logger = logger;
            _explorationService = explorationProjectService;
        }

        [HttpGet("{projectId}", Name = "GetExplorations")]
        public IEnumerable<Exploration> GetExplorations(Guid projectId)
        {
            return _explorationService.GetExplorations(projectId);
        }
    }
}
