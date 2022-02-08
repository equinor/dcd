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

        public CaseController(ILogger<CaseController> logger, CaseService caseService)
        {
            _logger = logger;
            _caseService = caseService;
        }

        [HttpPost(Name = "CreateCase")]
        public Project CreateCase([FromBody] CaseDto caseDto)
        {
            var case_ = CaseAdapter.Convert(caseDto);
            return _caseService.CreateCase(case_);
        }
    }
}
