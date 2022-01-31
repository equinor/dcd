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
    public class DrainageStrategyController : ControllerBase
    {
        private DrainageStrategyService _drainageStrategyService;
        private readonly ILogger<DrainageStrategyController> _logger;
        private readonly DrainageStrategyAdapter _drainageStrategyAdapter;

        public DrainageStrategyController(ILogger<DrainageStrategyController> logger, DrainageStrategyService drainageStrategyService, ProjectService projectService)
        {
            _logger = logger;
            _drainageStrategyService = drainageStrategyService;
            _drainageStrategyAdapter = new DrainageStrategyAdapter(projectService);
        }

        [HttpGet("{projectId}", Name = "GetDrainageStrategies")]
        public IEnumerable<DrainageStrategy> GetDrainageStrategies(Guid projectId)
        {
            return _drainageStrategyService.GetDrainageStrategies(projectId);
        }

        [HttpPost(Name = "CreateDrainageStrategy")]
        public DrainageStrategy CreateDrainageStrategy([FromBody] DrainageStrategyDto drainageStrategyDto)
        {
            var drainageStrategy = _drainageStrategyAdapter.Convert(drainageStrategyDto);
            return _drainageStrategyService.CreateDrainageStrategy(drainageStrategy);
        }
    }
}
