using api.Adapters;
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
        private readonly CaseAdapter _caseAdapter;
        private readonly ILogger<CasesController> _logger;


        public CasesController(ILogger<CasesController> logger, CaseService caseService)
        {
            _logger = logger;
            _caseService = caseService;
            _caseAdapter = new CaseAdapter();
        }

        [HttpPost(Name = "CreateCase")]
        public ProjectDto CreateCase([FromBody] CaseDto caseDto)
        {
            var case_ = _caseAdapter.Convert(caseDto);
            return _caseService.CreateCase(case_);
        }

        [HttpPut(Name = "UpdateCase")]
        public ProjectDto UpdateCase([FromBody] CaseDto caseDto)
        {
            var case_ = _caseAdapter.Convert(caseDto);
            return _caseService.UpdateCase(case_);
        }
    }
}
