using api.Adapters;
using api.Dtos;
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
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ILogger<ProjectsController> logger, ProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public ProjectDto Get(Guid projectId)
        {
            return _projectService.GetProjectDto(projectId);
        }

        [HttpGet(Name = "GetProjects")]
        public IEnumerable<ProjectDto>? GetProjects()
        {
            return _projectService.GetAllDtos();
        }

        [HttpPost(Name = "CreateProject")]
        public ProjectDto CreateProject([FromBody] ProjectDto projectDto)
        {
            var project = ProjectAdapter.Convert(projectDto);
            return _projectService.CreateProject(project);
        }
    }
}
