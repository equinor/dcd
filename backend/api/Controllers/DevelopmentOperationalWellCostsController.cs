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
        private readonly DevelopmentOperationalWellCostsService _operationalWellCostsService;
        public DevelopmentOperationalWellCostsController(DevelopmentOperationalWellCostsService operationalWellCostsService)
        {
            _operationalWellCostsService = operationalWellCostsService;
        }

        [HttpPut(Name = "UpdateDevelopmentOperationalWellCosts")]
        public DevelopmentOperationalWellCostsDto? UpdateDevelopmentOperationalWellCosts([FromBody] DevelopmentOperationalWellCostsDto dto)
        {
            return _operationalWellCostsService.UpdateOperationalWellCosts(dto);
        }

        [HttpPost(Name = "CreateDevelopmentOperationalWellCosts")]
        public DevelopmentOperationalWellCostsDto CreateDevelopmentOperationalWellCosts([FromBody] DevelopmentOperationalWellCostsDto dto)
        {
            return _operationalWellCostsService.CreateOperationalWellCosts(dto);
        }
    }
}
