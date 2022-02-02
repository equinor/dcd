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
    public class TopsidesController : ControllerBase
    {
        private readonly TopsideService _topsideService;

        private readonly TopsideAdapter _topsideAdapter;

        private readonly ILogger<TopsidesController> _logger;

        public TopsidesController(ILogger<TopsidesController> logger, TopsideService topsideService)
        {
            _logger = logger;
            _topsideService = topsideService;
            _topsideAdapter = new TopsideAdapter();
        }

        [HttpPost("{projectId}", Name = "topside")]
        public Topside CreateTopside([FromBody] TopsideDto topsideDto)
        {
            var topside = _topsideAdapter.Convert(topsideDto);
            return _topsideService.CreateTopside(topside);
        }

    }
}
