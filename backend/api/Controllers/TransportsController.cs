using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Api.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User

    )]
public class TransportsController : ControllerBase
{
    private readonly ITransportService _transportService;

    public TransportsController(ITransportService transportService)
    {
        _transportService = transportService;
    }

    [HttpPut(Name = "UpdateTransport")]
    public async Task<ProjectDto> UpdateTransport([FromBody] TransportDto transportDto)
    {
        return await _transportService.UpdateTransport(transportDto);
    }

    [HttpPut("new", Name = "NewUpdateTransport")]
    public async Task<TransportDto> NewUpdateTransport([FromBody] TransportDto transportDto)
    {
        return await _transportService.NewUpdateTransport(transportDto);
    }

    [HttpPost(Name = "CreateTransport")]
    public async Task<ProjectDto> CreateTransport([FromQuery] Guid sourceCaseId, [FromBody] TransportDto transportDto)
    {
        return await _transportService.CreateTransport(transportDto, sourceCaseId);
    }

    [HttpDelete("{transportId}", Name = "DeleteTransport")]
    public async Task<ProjectDto> DeleteTransport(Guid transportId)
    {
        return await _transportService.DeleteTransport(transportId);
    }

    [HttpPost("{transportId}/copy", Name = "CopyTransport")]
    public async Task<TransportDto> CopyTransport([FromQuery] Guid caseId, Guid transportId)
    {
        return await _transportService.CopyTransport(transportId, caseId);
    }
}
