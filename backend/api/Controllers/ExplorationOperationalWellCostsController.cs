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
    public class ExplorationOperationalWellCostsController : ControllerBase
    {
        private readonly ExplorationOperationalWellCostsService _operationalWellCostsService;
        public ExplorationOperationalWellCostsController(ExplorationOperationalWellCostsService operationalWellCostsService)
        {
            _operationalWellCostsService = operationalWellCostsService;
        }

        [HttpPut(Name = "UpdateOperationalWellCosts")]
        public ExplorationOperationalWellCostsDto UpdateOperationalWellCosts([FromBody] ExplorationOperationalWellCostsDto dto)
        {
            return _operationalWellCostsService.UpdateOperationalWellCosts(dto);
        }
    }
}
