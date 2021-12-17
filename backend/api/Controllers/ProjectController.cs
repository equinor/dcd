using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using api.Models;

namespace api.Controllers;

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


    [HttpGet("GetProject/{projectId}")]
    public Project Get(string projectId)
    {
        return _projectService.GetProject(projectId);
    }

    [HttpGet("GetProjects")]
    public IEnumerable<Project> GetProjects()
    {
        return _projectService.GetAll();
    }
}
