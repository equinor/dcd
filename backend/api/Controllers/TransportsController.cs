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
            _transportAdapter = new TransportAdapter(_transportService);


        }

        [HttpPatch("{transportId}", Name = "transports")]
        public Project UpdateTransport([FromRoute] Guid drainageStrategyId, [FromBody] TransportDto transportDto)
        {
            var transport = _transportAdapter.Convert(transportDto);
            return _transportService.UpdateTransport(drainageStrategyId, transport);
        }

        [HttpPost("{projectId}", Name = "transport")]
        public Project CreateTransport([FromBody] TransportDto transportDto)
        {
            var transport = _transportAdapter.Convert(transportDto);
            return _transportService.CreateTransport(transport);
        }

        [HttpDelete("{transportId}", Name = "transports")]
        public Project DeleteSurf([FromBody] TransportDto transportDto)
        {
            var transport = _transportAdapter.Convert(transportDto);
            return _transportService.DeleteTransport(transport);
        }
    }
}
