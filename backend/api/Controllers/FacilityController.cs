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
    public class FacilityController : ControllerBase
    {
        private readonly FacilityService _facilityService;
        private readonly ILogger<FacilityController> _logger;

        public FacilityController(ILogger<FacilityController> logger, FacilityService facilityService)
        {
            _logger = logger;
            _facilityService = facilityService;
        }

        [HttpGet("surf/{projectId}", Name = "GetSurfsForProject")]
        public IEnumerable<Surf>? GetSurfsForProject(Guid projectId)
        {
            return _facilityService.GetSurfsForProject(projectId);
        }

        [HttpGet("substructure/{projectId}", Name = "GetSubstructuresForProject")]
        public IEnumerable<Substructure>? GetSubstructuresForProject(Guid projectId)
        {
            return _facilityService.GetSubstructuresForProject(projectId);
        }

        [HttpGet("topside/{projectId}", Name = "GetTopsidesForProject")]
        public IEnumerable<Topside>? GetTopsidesForProject(Guid projectId)
        {
            return _facilityService.GetTopsidesForProject(projectId);
        }

        [HttpGet("transport/{projectId}", Name = "GetTransportsForProject")]
        public IEnumerable<Transport>? GetTransportsForProject(Guid projectId)
        {
            return _facilityService.GetTransportsForProject(projectId);
        }
    }
}
