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
    public class TopsidesController : ControllerBase
    {
        private readonly TopsideService _topsideService;

        public TopsidesController(TopsideService topsideService)
        {
            _topsideService = topsideService;
        }

        [HttpPost(Name = "CreateTopside")]
        public ProjectDto CreateTopside([FromQuery] Guid sourceCaseId, [FromBody] TopsideDto topsideDto)
        {
            return _topsideService.CreateTopside(topsideDto, sourceCaseId);
        }

        [HttpDelete("{topsideId}", Name = "DeleteTopside")]
        public ProjectDto DeleteTopside(Guid topsideId)
        {
            return _topsideService.DeleteTopside(topsideId);
        }

        [HttpPut(Name = "UpdateTopside")]
        public ProjectDto UpdateTopside([FromBody] TopsideDto topsideDto)
        {
            return _topsideService.UpdateTopside(topsideDto);
        }
    }
}
