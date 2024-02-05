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
    public class DevelopmentOperationalWellCostsController : ControllerBase
    {
        private readonly IDevelopmentOperationalWellCostsService _operationalWellCostsService;
        public DevelopmentOperationalWellCostsController(IDevelopmentOperationalWellCostsService operationalWellCostsService)
        {
            _operationalWellCostsService = operationalWellCostsService;
        }

        [HttpPut(Name = "UpdateDevelopmentOperationalWellCosts")]
        public async Task<DevelopmentOperationalWellCostsDto?> UpdateDevelopmentOperationalWellCosts([FromBody] DevelopmentOperationalWellCostsDto dto)
        {
            return await _operationalWellCostsService.UpdateOperationalWellCostsAsync(dto);
        }

        [HttpPost(Name = "CreateDevelopmentOperationalWellCosts")]
        public async Task<DevelopmentOperationalWellCostsDto> CreateDevelopmentOperationalWellCosts([FromBody] DevelopmentOperationalWellCostsDto dto)
        {
            return await _operationalWellCostsService.CreateOperationalWellCostsAsync(dto);
        }
    }
}
