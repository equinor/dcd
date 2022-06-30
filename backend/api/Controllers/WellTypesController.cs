
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
    public class WellTypesController : ControllerBase
    {

        private readonly WellTypesService _wellTypesService;

        public WellTypesController(WellService wellTypesService)
        {
            _wellTypesService = wellTypesService;
        }

        [HttpGet("{wellTypeId}", Name = "GetWellType")]
        public WellTypeDto GetWell(Guid wellTypeId)
        {
            return _wellTypesService.GetWellTypeDto(wellTypeId);
        }

        [HttpGet(Name = "GetWellTypes")]
        public IEnumerable<WellTypeDto> GetWells([FromQuery] Guid projectId)
        {
            if (projectId != Guid.Empty)
            {
                return _wellTypesService.GetDtosForProject(projectId);
            }
            return _wellTypesService.GetAllDtos();
        }

        [HttpPost(Name = "CreateWellType")]
        public ProjectDto CreateWell([FromBody] WellTypeDto wellTypeDto)
        {
            return _wellTypesService.CreateWellType(wellTypeDto);
        }

        [HttpPut(Name = "UpdateWellType")]
        public ProjectDto UpdateWell([FromBody] WellTypeDto wellTypeDto)
        {
            return _wellTypesService.UpdateWellType(wellTypeDto);
        }
    }
}
