using api.Dtos;
using api.Services;

using Api.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User
    )]
    public class ExplorationOperationalWellCostsController : ControllerBase
    {
        private readonly ExplorationOperationalWellCostsService _operationalWellCostsService;
        public ExplorationOperationalWellCostsController(ExplorationOperationalWellCostsService operationalWellCostsService)
        {
            _operationalWellCostsService = operationalWellCostsService;
        }

        [HttpPut(Name = "UpdateExplorationOperationalWellCosts")]
        public ExplorationOperationalWellCostsDto? UpdateExplorationOperationalWellCosts([FromBody] ExplorationOperationalWellCostsDto dto)
        {
            return _operationalWellCostsService.UpdateOperationalWellCosts(dto);
        }

        [HttpPost(Name = "CreateExplorationOperationalWellCosts")]
        public ExplorationOperationalWellCostsDto CreateExplorationOperationalWellCosts([FromBody] ExplorationOperationalWellCostsDto dto)
        {
            return _operationalWellCostsService.CreateOperationalWellCosts(dto);
        }
    }
}
