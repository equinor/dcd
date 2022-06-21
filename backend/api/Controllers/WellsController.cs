
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
    public class WellsController : ControllerBase
    {

        private readonly WellService _wellService;

        public WellsController(WellService wellService)
        {
            _wellService = wellService;
        }

        [HttpPost(Name = "CreateWell")]
        public ProjectDto CreateWell([FromBody] WellDto wellDto)
        {
            return _wellService.CreateWell(wellDto);
        }

        [HttpPut(Name = "UpdateWell")]
        public ProjectDto UpdateWell([FromBody] WellDto wellDto)
        {
            return _wellService.UpdateWell(wellDto);
        }
    }
}