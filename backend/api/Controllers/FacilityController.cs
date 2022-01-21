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
        private FacilityService _facilityService;
        private readonly ILogger<ProjectController> _logger;

        public FacilityController(ILogger<ProjectController> logger, FacilityService facilityService)
        {
            _logger = logger;
            _facilityService = facilityService;
        }

        [HttpGet(Name = "GetAllSurfs")]
        public IEnumerable<Surf> GetAllSurfs()
        {
            return _facilityService.GetAllSurfs();
        }

        [HttpGet("{surfId}", Name = "GetSurf")]
        public Surf GetSurf(Guid surfId)
        {
            return _facilityService.GetSurf(surfId);
        }

        [HttpGet("project/{projectId}", Name = "GetSurfsForProject")]
        public IEnumerable<Surf> GetSurfsForProject(Guid projectId) 
        {
            return _facilityService.GetSurfsForProject(projectId);
        }
    }
}
