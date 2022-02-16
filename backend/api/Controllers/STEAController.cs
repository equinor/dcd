

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
    public class STEAController : ControllerBase
    {
        private STEAService _sTEAService;
        private readonly ILogger<STEAController> _logger;

        public STEAController(ILogger<STEAController> logger, STEAService sTEAService)
        {
            _logger = logger;
            _sTEAService = sTEAService;
        }

        [HttpGet("{ProjectId}", Name = "GetInputToSTEA")]
        public STEAProjectDto GetInputToSTEA(Guid ProjectId)
        {
            return _sTEAService.GetInputToSTEA(ProjectId);
        }
    }
}

