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
    public class WellProjectController : ControllerBase
    {
        private WellProjectService _wellProjectService;
        private readonly ILogger<WellProjectController> _logger;

        public WellProjectController(ILogger<WellProjectController> logger, WellProjectService wellProjectService)
        {
            _logger = logger;
            _wellProjectService = wellProjectService;
        }

        [HttpGet("Project/{wellProjectId}", Name = "GetWellProject")]
        public WellProject GetWellProject(Guid wellProjectId)
        {
            return _wellProjectService.GetWellProject(wellProjectId);
        }

        [HttpGet("{projectId}", Name = "GetWellProjects")]
        public IEnumerable<WellProject> GetWellProjects(Guid projectId)
        {
            return _wellProjectService.GetWellProjects(projectId);
        }

        [HttpGet(Name = "GetAllWellProjects")]
        public IEnumerable<WellProject>? Get()
        {
            return _wellProjectService.GetAll();
        }
    }
}
