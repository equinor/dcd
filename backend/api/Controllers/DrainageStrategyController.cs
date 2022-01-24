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

        public DrainageStrategyController(ILogger<DrainageStrategyController> logger, DrainageStrategyService drainageStrategyService)
        {
            _logger = logger;
            _drainageStrategyService = drainageStrategyService;
        }

        [HttpGet("Project/{drainageStrategyId}", Name = "GetDrainageStrategy")]
        public DrainageStrategy GetDrainageStrategy(Guid drainageStrategyId)
        {
            return _drainageStrategyService.GetDrainageStrategy(drainageStrategyId);
        }

        [HttpGet("{projectId}", Name = "GetDrainageStrategies")]
        public IEnumerable<DrainageStrategy> GetDrainageStrategies(Guid projectId)
        {
            return _drainageStrategyService.GetDrainageStrategies(projectId);
        }

        [HttpGet(Name = "GetAll")]
        public IEnumerable<DrainageStrategy>? Get()
        {
            return _drainageStrategyService.GetAll();
        }
    }
}
