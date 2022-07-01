
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WellCasesController : ControllerBase
    {

        private readonly WellCaseService _wellCaseService;

        public WellCasesController(WellCaseService wellCaseService)
        {
            _wellCaseService = wellCaseService;
        }

        // [HttpGet("{wellId}", Name = "GetWellCase")]
        // public WellCaseDto GetWellCase(Guid wellId)
        // {
        //     return _wellCaseService.GetWellCaseDto(wellId);
        // }

        [HttpGet(Name = "GetWellCases")]
        public IEnumerable<WellCaseDto> GetWellCases([FromQuery] Guid projectId)
        {
            if (projectId != Guid.Empty)
            {
                // return _wellCaseService.GetDtosForProject(projectId);
            }
            return _wellCaseService.GetAllDtos();
        }

        [HttpPost(Name = "CreateWellCase")]
        public ProjectDto CreateWellCase([FromBody] WellCaseDto wellDto)
        {
            return _wellCaseService.CreateWellCase(wellDto);
        }

        [HttpPut(Name = "UpdateWellCase")]
        public ProjectDto UpdateWellCase([FromBody] WellCaseDto wellDto)
        {
            return _wellCaseService.UpdateWellCase(wellDto);
        }
    }
}
