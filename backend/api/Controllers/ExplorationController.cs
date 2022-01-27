using System.Text.Json;
using System.Text.Json.Serialization;

using api.Models;
using api.Services;

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

        [HttpPost(Name = "CreateExploration")]
        [Consumes("text/plain")]
        public Exploration CreateExploration([FromBody] String
                explorationJson)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            };

            var exploration =
                JsonSerializer.Deserialize<Exploration>(explorationJson, options);
            return _explorationService.CreateExploration(exploration);
        }
    }
}
