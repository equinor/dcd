using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Api.Services.FusionIntegration;

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
        private readonly ProjectService _projectService;
        private readonly FusionService _fusionService;

        public ProjectsController(ProjectService projectService, FusionService fusionService)
        {
            _projectService = projectService;
            _fusionService = fusionService;
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public ProjectDto? Get(Guid projectId)
        {
            try
            {
                return _projectService.GetProjectDto(projectId);

            }
            catch (NotFoundInDBException)
            {
                return null;
            }
        }

        [HttpPost("createFromFusion", Name = "CreateProjectFromContextId")]
        public async Task<ProjectDto> CreateProjectFromContextIdAsync([FromQuery] Guid contextId)
        {
            var projectMaster = await _fusionService.ProjectMasterAsync(contextId);
            if (projectMaster != null)
            {
                DateTimeOffset createDate = DateTimeOffset.UtcNow;

                var category = CommonLibraryProjectDtoAdapter.ConvertCategory(projectMaster.ProjectCategory ?? "");
                var phase = CommonLibraryProjectDtoAdapter.ConvertPhase(projectMaster.Phase ?? "");
                ProjectDto projectDto = new()
                {
                    Name = projectMaster.Description ?? "",
                    Description = projectMaster.Description ?? "",
                    CommonLibraryName = projectMaster.Description ?? "",
                    CreateDate = createDate,
                    FusionProjectId = projectMaster.Identity,
                    Country = projectMaster.Country ?? "",
                    Currency = Currency.NOK,
                    PhysUnit = PhysUnit.SI,
                    ProjectId = projectMaster.Identity,
                    ProjectCategory = category,
                    ProjectPhase = phase,
                    ExplorationOperationalWellCosts = new ExplorationOperationalWellCostsDto(),
                    DevelopmentOperationalWellCosts = new DevelopmentOperationalWellCostsDto(),
                };
                var project = ProjectAdapter.Convert(projectDto);
                return _projectService.CreateProject(project);
            }
            return new ProjectDto();
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

        [HttpPut(Name = "UpdateProject")]
        public ProjectDto UpdateProject([FromBody] ProjectDto projectDto)
        {
            return _projectService.UpdateProject(projectDto);
        }
    }
}
