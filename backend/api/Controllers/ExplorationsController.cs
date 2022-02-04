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
        public Project CreateExploration([FromBody] ExplorationDto
                explorationDto)
        {
            var exploration = _explorationAdapter.Convert(explorationDto);
            return _explorationService.CreateExploration(exploration, explorationDto.SourceCaseId);
        }

        [HttpDelete("{explorationId}", Name = "DeleteExploration")]
        public Project DeleteExploration(Guid drainageStrategyId)
        {
            return _explorationService.DeleteExploration(drainageStrategyId);
        }

        [HttpPatch("{explorationId}", Name = "UpdateExploration")]
        public Project UpdateExploration([FromRoute] Guid drainageStrategyId, [FromBody] ExplorationDto eplorationDto)
        {
            var exploration = _explorationAdapter.Convert(eplorationDto);
            return _explorationService.UpdateExploration(drainageStrategyId, exploration);
        }
    }
}
