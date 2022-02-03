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
    public class ProjectsController : ControllerBase
    {
        private ProjectService _projectService;
        private readonly ProjectAdapter _projectAdapter;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ILogger<ProjectsController> logger, ProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
            _projectAdapter = new ProjectAdapter();
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

        [HttpPost(Name = "CreateProject")]
        public Project CreateProject([FromBody] ProjectDto projectDto)
        {
            var project = _projectAdapter.Convert(projectDto);
            return _projectService.CreateProject(project);
        }
    }
}
