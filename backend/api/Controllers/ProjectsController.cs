using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Api.Authorization;
using Api.Services.FusionIntegration;

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
            // var category = CommonLibraryProjectDtoAdapter.ConvertCategory(projectMaster.ProjectCategory ?? "");
            // var phase = CommonLibraryProjectDtoAdapter.ConvertPhase(projectMaster.Phase ?? "");
            CreateProjectDto projectDto = new()
            {
                Name = projectMaster.Description ?? "",
                Description = projectMaster.Description ?? "",
                CommonLibraryName = projectMaster.Description ?? "",
                FusionProjectId = projectMaster.Identity,
                Country = projectMaster.Country ?? "",
                Currency = Currency.NOK,
                PhysUnit = PhysUnit.SI,
                // ProjectCategory = category,
                // ProjectPhase = phase,
            };
            var project = _mapper.Map<Project>(projectDto);

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            project.CreateDate = DateTimeOffset.UtcNow;

            return await _projectService.CreateProject(project);
        }

        return new ProjectDto();
    }

    [HttpPost(Name = "CreateProject")]
    public async Task<ProjectDto> CreateProject([FromBody] ProjectDto projectDto)
    {
        var project = _mapper.Map<Project>(projectDto);
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project));
        }
        project.CreateDate = DateTimeOffset.UtcNow;
        return await _projectService.CreateProject(project);
    }

    [HttpPut(Name = "UpdateProject")]
    public async Task<ProjectDto> UpdateProject([FromBody] ProjectDto projectDto)
    {
        return await _projectService.UpdateProject(projectDto);
    }

    [HttpGet("{projectId}/case-comparison")]
    public async Task<List<CompareCasesDto>> CaseComparison(Guid projectId)
    {
        return new List<CompareCasesDto>(await _compareCasesService.Calculate(projectId));
    }

    [HttpPut("{projectId}/technical-input")]
    public async Task<TechnicalInputDto> UpdateTechnicalInput([FromRoute] Guid projectId, [FromBody] TechnicalInputDto dto)
    {
        return await _technicalInputService.UpdateTehnicalInput(dto);
    }
}
