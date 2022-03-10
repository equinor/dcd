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
        private readonly TransportService _transportService;
        private readonly TransportAdapter _transportAdapter;

        public TransportsController(TransportService transportService)
        {
            _transportService = transportService;
            _transportAdapter = new TransportAdapter();


        }

        [HttpPut(Name = "UpdateTransport")]
        public ProjectDto UpdateTransport([FromBody] TransportDto transportDto)
        {
            var transport = _transportAdapter.Convert(transportDto);
            return _transportService.UpdateTransport(transport);
        }

        [HttpPost(Name = "CreateTransport")]
        public ProjectDto CreateTransport([FromQuery] Guid sourceCaseId, [FromBody] TransportDto transportDto)
        {
            var transport = _transportAdapter.Convert(transportDto);
            return _transportService.CreateTransport(transport, sourceCaseId);
        }

        [HttpDelete("{transportId}", Name = "DeleteTransport")]
        public ProjectDto DeleteTransport(Guid transportId)
        {
            return _transportService.DeleteTransport(transportId);
        }
    }
}
