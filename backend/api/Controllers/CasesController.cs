
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

        public CasesController(CaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpPost(Name = "CreateCase")]
        public ProjectDto CreateCase([FromBody] CaseDto caseDto)
        {
            return _caseService.CreateCase(caseDto);
        }

        [HttpPut(Name = "UpdateCase")]
        public ProjectDto UpdateCase([FromBody] CaseDto caseDto)
        {
            return _caseService.UpdateCase(caseDto);
        }
    }
}
