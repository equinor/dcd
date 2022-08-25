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
    public class OperationalWellCostsController : ControllerBase
    {

        public OperationalWellCostsController()
        {
        }

        [HttpPut(Name = "UpdateOperationalWellCosts")]
        public ProjectDto UpdateOperationalWellCosts([FromBody] OperationalWellCostsDto dto)
        {
            return new ProjectDto();
        }
    }
}
