using api.Adapters;
using api.Dtos;
using api.Models;
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
    public class CaseController : ControllerBase
    {
        private CaseService _caseService;
        private readonly ILogger<CaseController> _logger;
        private readonly CaseAdapter _caseAdapter;

        public CaseController(ILogger<CaseController> logger, CaseService caseService)
        {
            _logger = logger;
            _caseService = caseService;
            _caseAdapter = new CaseAdapter();
        }

        [HttpPost(Name = "CreateCase")]
        public Project CreateCase([FromBody] CaseDto caseDto)
        {
            var case_ = _caseAdapter.Convert(caseDto);
            return _caseService.CreateCase(case_);
        }
    }
}
