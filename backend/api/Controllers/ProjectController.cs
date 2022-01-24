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
    public class ProjectController : ControllerBase
    {
        private ProjectService _projectService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger, ProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public Project Get(Guid projectId)
        {
            return _projectService.GetProject(projectId);
        }

        [HttpGet(Name = "GetProjects")]
        public IEnumerable<Project>? GetProjects()
        {
            return _projectService.GetAll();
        }
    }
}
