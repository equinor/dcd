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
    public class TransportsController : ControllerBase
    {
        private readonly ILogger<TransportsController> _logger;
        private readonly TransportService _transportService;
        private readonly TransportAdapter _transportAdapter;

        public TransportsController(ILogger<TransportsController> logger, TransportService transportService)
        {
            _logger = logger;
            _transportService = transportService;
            _transportAdapter = new TransportAdapter();


        }

        [HttpPatch("{transportId}", Name = "UpdateTransport")]
        public Project UpdateTransport([FromRoute] Guid transportId, [FromBody] TransportDto transportDto)
        {
            var transport = _transportAdapter.Convert(transportDto);
            return _transportService.UpdateTransport(transportId, transport);
        }

        [HttpPost(Name = "CreateTransport")]
        public Project CreateTransport([FromBody] TransportDto transportDto)
        {
            var transport = _transportAdapter.Convert(transportDto);
            return _transportService.CreateTransport(transport, transportDto.SourceCaseId);
        }

        [HttpDelete("{transportId}", Name = "DeleteTransport")]
        public Project DeleteTransport(Guid transportId)
        {
            return _transportService.DeleteTransport(transportId);
        }
    }
}
