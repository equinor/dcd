using api.Authorization;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Services;
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
    public async Task<ProjectWithAssetsDto?> Get(Guid projectId)
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
    public async Task<ProjectWithAssetsDto> CreateProject([FromQuery] Guid contextId)
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

        return new ProjectWithAssetsDto();
    }

    [HttpPut("{projectId}")]
    public async Task<ProjectWithCasesDto> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProjectDto projectDto)
    {
        return await _projectService.UpdateProject(projectId, projectDto);
    }

    [HttpPut("{projectId}/exploration-operational-well-costs/{explorationOperationalWellCostsId}")]
    public async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts([FromRoute] Guid projectId, [FromRoute] Guid explorationOperationalWellCostsId, [FromBody] UpdateExplorationOperationalWellCostsDto dto)
    {
        return await _projectService.UpdateExplorationOperationalWellCosts(projectId, explorationOperationalWellCostsId, dto);
    }

    [HttpPut("{projectId}/development-operational-well-costs/{developmentOperationalWellCostsId}")]
    public async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts([FromRoute] Guid projectId, [FromRoute] Guid developmentOperationalWellCostsId, [FromBody] UpdateDevelopmentOperationalWellCostsDto dto)
    {
        return await _projectService.UpdateDevelopmentOperationalWellCosts(projectId, developmentOperationalWellCostsId, dto);
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
