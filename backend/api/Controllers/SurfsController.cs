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
            _surfAdapter = new SurfAdapter();
        }

        [HttpPatch("{surfId}", Name = "UpdateSurf")]
        public Project UpdateSurf([FromRoute] Guid surfId, [FromBody] SurfDto surfDto)
        {
            var surf = _surfAdapter.Convert(surfDto);
            return _surfService.UpdateSurf(surfId, surf);
        }

        [HttpPost(Name = "CreateSurf")]
        public Project CreateSurf([FromBody] SurfDto surfDto)
        {
            var surf = _surfAdapter.Convert(surfDto);
            return _surfService.CreateSurf(surf, surfDto.SourceCaseId);
        }

        [HttpDelete("{surfId}", Name = "DeleteSurf")]
        public Project DeleteSurf(Guid surfId)
        {
            return _surfService.DeleteSurf(surfId);
        }

    }
}
