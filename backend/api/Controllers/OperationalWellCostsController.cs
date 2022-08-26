// using api.Adapters;
// using api.Dtos;
// using api.Models;
// using api.Services;

// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Identity.Web.Resource;

// namespace api.Controllers
// {
//     [Authorize]
//     [ApiController]
//     [Route("[controller]")]
//     [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
//     public class OperationalWellCostsController : ControllerBase
//     {
//         private readonly OperationalWellCostsService _operationalWellCostsService;
//         public OperationalWellCostsController(OperationalWellCostsService operationalWellCostsService)
//         {
//             _operationalWellCostsService = operationalWellCostsService;
//         }

//         [HttpPut(Name = "UpdateOperationalWellCosts")]
//         public OperationalWellCostsDto UpdateOperationalWellCosts([FromBody] OperationalWellCostsDto dto)
//         {
//             return _operationalWellCostsService.UpdateOperationalWellCosts(dto);
//         }
//     }
// }
