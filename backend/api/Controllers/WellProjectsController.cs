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
        private readonly WellProjectAdapter _wellProjectAdapter;

        public WellProjectsController(WellProjectService wellProjectService)
        {
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

        [HttpPut(Name = "UpdateWellProject")]
        public ProjectDto UpdateWellProject([FromBody] WellProjectDto wellProjectDto)
        {
            return _wellProjectService.UpdateWellProject(wellProjectDto);
        }
    }
}
