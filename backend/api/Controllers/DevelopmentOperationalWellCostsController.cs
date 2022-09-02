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
    public class DevelopmentOperationalWellCostsController : ControllerBase
    {
        private readonly DevelopmentOperationalWellCostsService _operationalWellCostsService;
        public DevelopmentOperationalWellCostsController(DevelopmentOperationalWellCostsService operationalWellCostsService)
        {
            _operationalWellCostsService = operationalWellCostsService;
        }

        [HttpPut(Name = "UpdateOperationalWellCosts")]
        public DevelopmentOperationalWellCostsDto UpdateOperationalWellCosts([FromBody] DevelopmentOperationalWellCostsDto dto)
        {
            return _operationalWellCostsService.UpdateOperationalWellCosts(dto);
        }
    }
}
