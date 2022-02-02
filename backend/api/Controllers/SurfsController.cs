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
    public class SurfsController : ControllerBase
    {
        private readonly SurfService _surfService;

        private readonly SurfAdapter _surfAdapter;

        private readonly ILogger<SurfsController> _logger;

        public SurfsController(ILogger<SurfsController> logger, SurfService surfService)
        {
            _logger = logger;
            _surfService = surfService;
            _surfAdapter = new SurfAdapter(_surfService);
        }

        [HttpPatch("{surfId}", Name = "surfs")]
        public Surf UpdateSurf([FromBody] SurfDto surfDto)
        {
            var surf = _surfAdapter.Convert(surfDto);
            return _surfService.UpdateSurf(surf);
        }

        [HttpPost("{projectId}", Name = "surf")]
        public Surf CreateSurf([FromBody] SurfDto surfDto)
        {
            var surf = _surfAdapter.Convert(surfDto);
            return _surfService.CreateSurf(surf);
        }

        [HttpDelete("{surfId}", Name = "surfs")]
        public bool DeleteSurf([FromBody] SurfDto surfDto)
        {
            var surf = _surfAdapter.Convert(surfDto);
            if (_surfService.DeleteSurf(surf))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
