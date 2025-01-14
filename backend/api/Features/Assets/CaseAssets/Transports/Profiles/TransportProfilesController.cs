using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Profiles.Dtos.Create;
using api.Features.Assets.CaseAssets.Transports.Profiles.Dtos.Update;
using api.Features.Assets.CaseAssets.Transports.Services;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Transports.Profiles;

[Route("projects/{projectId}/cases/{caseId}/transports")]
[AuthorizeActionType(ActionType.Edit)]
public class TransportProfilesController(TransportTimeSeriesService transportTimeSeriesService) : ControllerBase
{
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
