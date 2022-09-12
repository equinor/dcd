using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class TransportsController : ControllerBase
{
    private readonly TransportService _transportService;

    public TransportsController(TransportService transportService)
    {
        _transportService = transportService;
    }

    [HttpPut(Name = "UpdateTransport")]
    public ProjectDto UpdateTransport([FromBody] TransportDto transportDto)
    {
        return _transportService.UpdateTransport(transportDto);
    }

    [HttpPost(Name = "CreateTransport")]
    public ProjectDto CreateTransport([FromQuery] Guid sourceCaseId, [FromBody] TransportDto transportDto)
    {
        return _transportService.CreateTransport(transportDto, sourceCaseId);
    }

    [HttpDelete("{transportId}", Name = "DeleteTransport")]
    public ProjectDto DeleteTransport(Guid transportId)
    {
        return _transportService.DeleteTransport(transportId);
    }
}