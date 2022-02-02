using api.Adapters;
using api.Dtos;
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
    public class ExplorationsController : ControllerBase
    {
        private ExplorationService _explorationService;
        private readonly ILogger<ExplorationsController> _logger;
        private readonly ExplorationAdapter _explorationAdapter;

        public ExplorationsController(ILogger<ExplorationsController> logger,
                ExplorationService explorationService)
        {
            _logger = logger;
            _explorationService = explorationService;
            _explorationAdapter = new ExplorationAdapter();
        }

        [HttpPost(Name = "CreateExploration")]
        public Exploration CreateExploration([FromBody] ExplorationDto
                explorationDto)
        {
            var exploration = _explorationAdapter.Convert(explorationDto);
            return _explorationService.CreateExploration(exploration);
        }
    }
}
