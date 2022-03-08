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
    public class WellProjectsController : ControllerBase
    {
        private readonly WellProjectService _wellProjectService;
        private readonly ILogger<WellProjectsController> _logger;
        private readonly WellProjectAdapter _wellProjectAdapter;

        public WellProjectsController(ILogger<WellProjectsController> logger, WellProjectService wellProjectService)
        {
            _logger = logger;
            _wellProjectService = wellProjectService;
            _wellProjectAdapter = new WellProjectAdapter();
        }

        [HttpPost(Name = "CreateWellProject")]
        public ProjectDto CreateWellProject([FromQuery] Guid sourceCaseId, [FromBody] WellProjectDto wellProjectDto)
        {
            var wellProject = _wellProjectAdapter.Convert(wellProjectDto);
            return _wellProjectService.CreateWellProject(wellProject, sourceCaseId);
        }

        [HttpDelete("{wellProjectId}", Name = "DeleteWellProject")]
        public ProjectDto DeleteWellProject(Guid wellProjectId)
        {
            return _wellProjectService.DeleteWellProject(wellProjectId);
        }

        [HttpPut("{wellProjectId}", Name = "UpdateWellProject")]
        public ProjectDto UpdateWellProject([FromRoute] Guid wellProjectId, [FromBody] WellProjectDto wellProjectDto)
        {
            var wellProject = _wellProjectAdapter.Convert(wellProjectDto);
            return _wellProjectService.UpdateWellProject(wellProjectId, wellProject);
        }
    }
}
