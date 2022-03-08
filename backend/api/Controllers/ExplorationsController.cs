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

        public ExplorationsController(ILogger<ExplorationsController> logger,
                ExplorationService explorationService)
        {
            _logger = logger;
            _explorationService = explorationService;
        }

        [HttpPost(Name = "CreateExploration")]
        public ProjectDto CreateExploration([FromQuery] Guid sourceCaseId, [FromBody] ExplorationDto explorationDto)
        {
            var exploration = ExplorationAdapter.Convert(explorationDto);
            return _explorationService.CreateExploration(exploration, sourceCaseId);
        }

        [HttpDelete("{explorationId}", Name = "DeleteExploration")]
        public ProjectDto DeleteExploration(Guid explorationId)
        {
            return _explorationService.DeleteExploration(explorationId);
        }

        [HttpPut("{explorationId}", Name = "UpdateExploration")]
        public ProjectDto UpdateExploration([FromRoute] Guid explorationId, [FromBody] ExplorationDto eplorationDto)
        {
            var exploration = ExplorationAdapter.Convert(eplorationDto);
            return _explorationService.UpdateExploration(explorationId, exploration);
        }
    }
}
