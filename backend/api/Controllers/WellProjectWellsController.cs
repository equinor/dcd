
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
    public class WellProjectWellsController : ControllerBase
    {

        private readonly WellProjectWellService _wellProjectWellService;

        public WellProjectWellsController(WellProjectWellService wellProjectWellService)
        {
            _wellProjectWellService = wellProjectWellService;
        }

        [HttpGet(Name = "GetWellProjectWells")]
        public IEnumerable<WellProjectWellDto> GetWellProjectWells([FromQuery] Guid projectId)
        {
            if (projectId != Guid.Empty)
            {
                // return _wellProjectWellService.GetDtosForProject(projectId);
            }
            return _wellProjectWellService.GetAllDtos();
        }

        [HttpPost(Name = "CreateWellProjectWell")]
        public ProjectDto CreateWellProjectWell([FromBody] WellProjectWellDto wellDto)
        {
            return _wellProjectWellService.CreateWellProjectWell(wellDto);
        }

        [HttpPut(Name = "UpdateWellProjectWell")]
        public ProjectDto UpdateWellProjectWell([FromBody] WellProjectWellDto wellDto)
        {
            return _wellProjectWellService.UpdateWellProjectWell(wellDto);
        }
    }
}
