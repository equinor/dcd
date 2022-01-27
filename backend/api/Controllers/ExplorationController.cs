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
        private ProjectService _projectService;
        private readonly ILogger<ExplorationController> _logger;

        public ExplorationController(ILogger<ExplorationController> logger,
                ExplorationService explorationProjectService,
                ProjectService projectService)
        {
            _logger = logger;
            _explorationService = explorationProjectService;
            _projectService = projectService;
        }

        [HttpGet("{projectId}", Name = "GetExplorations")]
        public IEnumerable<Exploration> GetExplorations(Guid projectId)
        {
            return _explorationService.GetExplorations(projectId);
        }

        [HttpPost(Name = "CreateExploration")]
        public Exploration CreateExploration([FromBody] Exploration
                exploration)
        {
            return _explorationService.CreateExploration(exploration);
        }
    }
}
