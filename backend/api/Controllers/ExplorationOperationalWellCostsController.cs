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
        private readonly IExplorationOperationalWellCostsService _operationalWellCostsService;
        public ExplorationOperationalWellCostsController(IExplorationOperationalWellCostsService operationalWellCostsService)
        {
            _operationalWellCostsService = operationalWellCostsService;
        }
        [HttpPut(Name = "UpdateExplorationOperationalWellCosts")]
        public async Task<ExplorationOperationalWellCostsDto?> UpdateExplorationOperationalWellCosts([FromBody] ExplorationOperationalWellCostsDto dto)
        {
            return await _operationalWellCostsService.UpdateOperationalWellCostsAsync(dto);
        }

        [HttpPost(Name = "CreateExplorationOperationalWellCosts")]
        public async Task<ExplorationOperationalWellCostsDto> CreateExplorationOperationalWellCosts([FromBody] ExplorationOperationalWellCostsDto dto)
        {
            return await _operationalWellCostsService.CreateOperationalWellCostsAsync(dto);
        }
    }
}
