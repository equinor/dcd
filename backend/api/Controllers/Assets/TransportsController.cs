using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/transports")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class TransportsController : ControllerBase
{
    private readonly ITransportService _transportService;

    public TransportsController(
        ITransportService transportService
    )
    {
        _transportService = transportService;
    }

    [HttpPut("{transportId}")]
    public async Task<TransportDto> UpdateTransport(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromBody] APIUpdateTransportDto dto)
    {
        return await _transportService.UpdateTransport(caseId, transportId, dto);
    }

    [HttpPut("{transportId}/cost-profile-override/{costProfileId}")]
    public async Task<TransportCostProfileOverrideDto> UpdateTransportCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTransportCostProfileOverrideDto dto)
    {
        return await _transportService.UpdateTransportCostProfileOverride(caseId, transportId, costProfileId, dto);
    }


}
