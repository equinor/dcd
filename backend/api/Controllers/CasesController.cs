
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
    public class CasesController : ControllerBase
    {

        private readonly CaseService _caseService;
        private readonly ILogger<CasesController> _logger;

        public CasesController(CaseService caseService, ILogger<CasesController> logger)
        {
            _caseService = caseService;
            _logger = logger;
        }

        [HttpPost(Name = "CreateCase")]
        public ProjectDto CreateCase([FromBody] CaseDto caseDto)
        {
            _logger.LogWarning("Case:" +caseDto.Name +" - " +caseDto.Id +" has been created.");
            return _caseService.CreateCase(caseDto);
        }

        [HttpPut(Name = "UpdateCase")]
        public ProjectDto UpdateCase([FromBody] CaseDto caseDto)
        {
            _logger.LogWarning("Case:" +caseDto.Name +" - " +caseDto.Id +" has been updated.");
            return _caseService.UpdateCase(caseDto);
        }
    }
}
