using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Services;

using api.Authorization;
using api.Services.FusionIntegration;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects")]
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
    private readonly ICompareCasesService _compareCasesService;
    private readonly ITechnicalInputService _technicalInputService;
    private readonly IMapper _mapper;

    public ProjectsController(
        IProjectService projectService,
        IFusionService fusionService,
        ICompareCasesService compareCasesService,
        ITechnicalInputService technicalInputService,
        IMapper mapper
    )
    {
        _projectService = projectService;
        _fusionService = fusionService;
        _compareCasesService = compareCasesService;
        _technicalInputService = technicalInputService;
        _mapper = mapper;
    }

    [HttpGet("{projectId}")]
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

    [HttpPost]
    public async Task<ProjectDto> CreateProject([FromQuery] Guid contextId)
    {
        var projectMaster = await _fusionService.ProjectMasterAsync(contextId);
        if (projectMaster != null)
        {
            var project = _mapper.Map<Project>(projectMaster);

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            project.CreateDate = DateTimeOffset.UtcNow;

            return await _projectService.CreateProject(project);
        }

        return new ProjectDto();
    }

    [HttpPut("{projectId}")]
    public async Task<ProjectDto> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProjectDto projectDto)
    {
        return await _projectService.UpdateProject(projectId, projectDto);
    }

    [HttpGet("{projectId}/case-comparison")]
    public async Task<List<CompareCasesDto>> CaseComparison(Guid projectId)
    {
        return new List<CompareCasesDto>(await _compareCasesService.Calculate(projectId));
    }

    [HttpPut("{projectId}/technical-input")]
    public async Task<TechnicalInputDto> UpdateTechnicalInput([FromRoute] Guid projectId, [FromBody] UpdateTechnicalInputDto dto)
    {
        return await _technicalInputService.UpdateTehnicalInput(projectId, dto);
    }
}
