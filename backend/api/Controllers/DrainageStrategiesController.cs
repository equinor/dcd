using api.Adapters;
using api.Dtos;
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
    public class DrainageStrategiesController : ControllerBase
    {
        private DrainageStrategyService _drainageStrategyService;
        private readonly ILogger<DrainageStrategiesController> _logger;

        public DrainageStrategiesController(ILogger<DrainageStrategiesController> logger, DrainageStrategyService drainageStrategyService)
        {
            _logger = logger;
            _drainageStrategyService = drainageStrategyService;
        }

        [HttpPost(Name = "CreateDrainageStrategy")]
        public ProjectDto CreateDrainageStrategy([FromQuery] Guid sourceCaseId, [FromBody] DrainageStrategyDto drainageStrategyDto)
        {
            var drainageStrategy = DrainageStrategyAdapter.Convert(drainageStrategyDto);
            return _drainageStrategyService.CreateDrainageStrategy(drainageStrategy, sourceCaseId);
        }

        [HttpDelete("{drainageStrategyId}", Name = "DeleteDrainageStrategy")]
        public ProjectDto DeleteDrainageStrategy(Guid drainageStrategyId)
        {
            return _drainageStrategyService.DeleteDrainageStrategy(drainageStrategyId);
        }

        [HttpPatch("{drainageStrategyId}", Name = "UpdateDrainageStrategy")]
        public ProjectDto UpdateDrainageStrategy([FromRoute] Guid drainageStrategyId, [FromBody] DrainageStrategyDto drainageStrategyDto)
        {
            var drainageStrategy = DrainageStrategyAdapter.Convert(drainageStrategyDto);
            return _drainageStrategyService.UpdateDrainageStrategy(drainageStrategyId, drainageStrategy);
        }
    }
}
