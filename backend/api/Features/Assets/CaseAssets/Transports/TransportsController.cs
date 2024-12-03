using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Create;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;
using api.Features.Assets.CaseAssets.Transports.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Assets.CaseAssets.Transports;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/transports")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class TransportsController(
    ITransportService transportService,
    ITransportTimeSeriesService transportTimeSeriesService) : ControllerBase
{
    [HttpPut("{transportId}")]
    public async Task<TransportDto> UpdateTransport(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromBody] APIUpdateTransportDto dto)
    {
        return await transportService.UpdateTransport(projectId, caseId, transportId, dto);
    }

    [HttpPost("{transportId}/cost-profile-override")]
    public async Task<TransportCostProfileOverrideDto> CreateTransportCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromBody] CreateTransportCostProfileOverrideDto dto)
    {
        return await transportTimeSeriesService.CreateTransportCostProfileOverride(projectId, caseId, transportId, dto);
    }

    [HttpPut("{transportId}/cost-profile-override/{costProfileId}")]
    public async Task<TransportCostProfileOverrideDto> UpdateTransportCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTransportCostProfileOverrideDto dto)
    {
        return await transportTimeSeriesService.UpdateTransportCostProfileOverride(projectId, caseId, transportId, costProfileId, dto);
    }
}
