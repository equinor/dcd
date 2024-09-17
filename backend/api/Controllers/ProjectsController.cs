using api.Authorization;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Models.Fusion;
using api.Services;
using api.Services.FusionIntegration;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectsController : ControllerBase
{
    private readonly IFusionService _fusionService;
    private readonly IProjectService _projectService;
    private readonly ICompareCasesService _compareCasesService;
    private readonly ITechnicalInputService _technicalInputService;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IFusionPeopleService _fusionPeopleService;

    public ProjectsController(
        IProjectService projectService,
        IFusionService fusionService,
        ICompareCasesService compareCasesService,
        ITechnicalInputService technicalInputService,
        IMapper mapper,
        IAuthorizationService authorizationService,
        IFusionPeopleService fusionPeopleService
    )
    {
        _projectService = projectService;
        _fusionService = fusionService;
        _compareCasesService = compareCasesService;
        _technicalInputService = technicalInputService;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _fusionPeopleService = fusionPeopleService;
    }

    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User
    )]
    [HttpGet("{projectId}")]
    [ActionType(ActionType.Read)]
    public async Task<ProjectWithAssetsDto?> Get(Guid projectId)
    {
        try
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new Project(), new ProjectAccessRequirement());
            return await _projectService.GetProjectDto(projectId);
        }
        catch (NotFoundInDBException)
        {
            return null;
        }
    }

    [RequiresApplicationRoles(
        ApplicationRole.Admin
    )]
    [HttpGet("{projectId}/members")]
    public async Task<List<FusionPersonV1>> GetMembers(Guid projectId)
    {
        var result = await _fusionPeopleService.GetAllPersonsOnProject(projectId, "", 100, 0);
        return result;
    }

    [RequiresApplicationRoles(
        ApplicationRole.Admin
    )]
    [HttpPut("{projectId}/members/{personId}")]
    [ActionType(ActionType.Edit)]
    public async Task<ProjectMember> AddMember(Guid projectId, Guid personId)
    {
        return await _projectService.AddProjectMember(projectId, personId);
    }

    [HttpPost]
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [ActionType(ActionType.Edit)]
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

    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [HttpPut("{projectId}")]
    [ActionType(ActionType.Edit)]
    public async Task<ProjectWithCasesDto> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProjectDto projectDto)
    {
        return await _projectService.UpdateProject(projectId, projectDto);
    }

    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [HttpPut("{projectId}/exploration-operational-well-costs/{explorationOperationalWellCostsId}")]
    [ActionType(ActionType.Edit)]
    public async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts([FromRoute] Guid projectId, [FromRoute] Guid explorationOperationalWellCostsId, [FromBody] UpdateExplorationOperationalWellCostsDto dto)
    {
        return await _projectService.UpdateExplorationOperationalWellCosts(projectId, explorationOperationalWellCostsId, dto);
    }

    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [HttpPut("{projectId}/development-operational-well-costs/{developmentOperationalWellCostsId}")]
    [ActionType(ActionType.Edit)]
    public async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts([FromRoute] Guid projectId, [FromRoute] Guid developmentOperationalWellCostsId, [FromBody] UpdateDevelopmentOperationalWellCostsDto dto)
    {
        return await _projectService.UpdateDevelopmentOperationalWellCosts(projectId, developmentOperationalWellCostsId, dto);
    }

    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User
    )]
    [HttpGet("{projectId}/case-comparison")]
    [ActionType(ActionType.Read)]
    public async Task<List<CompareCasesDto>> CaseComparison(Guid projectId)
    {
        return new List<CompareCasesDto>(await _compareCasesService.Calculate(projectId));
    }

    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [HttpPut("{projectId}/technical-input")]
    [ActionType(ActionType.Edit)]
    public async Task<TechnicalInputDto> UpdateTechnicalInput([FromRoute] Guid projectId, [FromBody] UpdateTechnicalInputDto dto)
    {
        return await _technicalInputService.UpdateTehnicalInput(projectId, dto);
    }
}
