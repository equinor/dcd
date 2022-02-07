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
    public class SubstructuresController : ControllerBase
    {
        private SubstructureService _substructureService;
        private readonly ILogger<SubstructuresController> _logger;
        private readonly SubstructureAdapter _substructureAdapter;

        public SubstructuresController(ILogger<SubstructuresController> logger, SubstructureService substructureService)
        {
            _logger = logger;
            _substructureService = substructureService;
            _substructureAdapter = new SubstructureAdapter();
        }

        [HttpPost(Name = "CreateSubstructure")]
        public Project CreateSubstructure([FromBody] SubstructureDto substructureDto)
        {
            var substructure = _substructureAdapter.Convert(substructureDto);
            return _substructureService.CreateSubstructure(substructure, substructureDto.SourceCaseId);
        }

        [HttpDelete("{substructureId}", Name = "DeleteSubstructure")]
        public Project DeleteSubstructure(Guid substructureId)
        {
            return _substructureService.DeleteSubstructure(substructureId);
        }

        [HttpPatch("{substructureId}", Name = "UpdateSubstructure")]
        public Project UpdateSubstructure([FromRoute] Guid substructureId, [FromBody] SubstructureDto substructureDto)
        {
            var substructure = _substructureAdapter.Convert(substructureDto);
            return _substructureService.UpdateSubstructure(substructureId, substructure);
        }
    }
}
