using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Api.Authorization;
using Api.Services.FusionIntegration;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class ProjectsController : ControllerBase
{
    private readonly IFusionService _fusionService;
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService, IFusionService fusionService)
    {
        _projectService = projectService;
        _fusionService = fusionService;
    }

    [HttpGet("{projectId}", Name = "GetProject")]
    public async Task<ProjectDto?> Get(Guid projectId)
    {
        try
        {
            return await _projectService.GetProjectDto(projectId);
        }
        catch (NotFoundInDBException)
        {
            return null;
        }
    }

    [HttpPost("createFromFusion", Name = "CreateProjectFromContextId")]
    public async Task<ProjectDto> CreateProjectFromContextId([FromQuery] Guid contextId)
    {
        var projectMaster = await _fusionService.ProjectMasterAsync(contextId);
        if (projectMaster != null)
        {
            var category = CommonLibraryProjectDtoAdapter.ConvertCategory(projectMaster.ProjectCategory ?? "");
            var phase = CommonLibraryProjectDtoAdapter.ConvertPhase(projectMaster.Phase ?? "");
            ProjectDto projectDto = new()
            {
                Name = projectMaster.Description ?? "",
                Description = projectMaster.Description ?? "",
                CommonLibraryName = projectMaster.Description ?? "",
                FusionProjectId = projectMaster.Identity,
                Country = projectMaster.Country ?? "",
                Currency = Currency.NOK,
                PhysUnit = PhysUnit.SI,
                ProjectId = projectMaster.Identity,
                ProjectCategory = category,
                ProjectPhase = phase,
            };
            var project = ProjectAdapter.Convert(projectDto);
            project.CreateDate = DateTimeOffset.UtcNow;
            return await _projectService.CreateProject(project);
        }

        return new ProjectDto();
    }

    [HttpPost(Name = "CreateProject")]
    public async Task<ProjectDto> CreateProject([FromBody] ProjectDto projectDto)
    {
        var project = ProjectAdapter.Convert(projectDto);
        project.CreateDate = DateTimeOffset.UtcNow;
        return await _projectService.CreateProject(project);
    }

    [HttpPut(Name = "UpdateProject")]
    public async Task<ProjectDto> UpdateProject([FromBody] ProjectDto projectDto)
    {
        return await _projectService.UpdateProject(projectDto);
    }

    [HttpPut("ReferenceCase", Name = "SetReferenceCase")]
    public async Task<ProjectDto> SetReferenceCase([FromBody] ProjectDto projectDto)
    {
        return await _projectService.SetReferenceCase(projectDto);
    }
}
