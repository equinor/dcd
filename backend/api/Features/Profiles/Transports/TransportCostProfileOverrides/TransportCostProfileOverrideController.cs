using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Transports.TransportCostProfileOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Transports.TransportCostProfileOverrides;

public class TransportCostProfileOverrideController(TransportCostProfileOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/transports/{transportId:guid}/cost-profile-override")]
    public async Task<TransportCostProfileOverrideDto> CreateTransportCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromBody] CreateTransportCostProfileOverrideDto dto)
    {
        return await service.CreateTransportCostProfileOverride(projectId, caseId, transportId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/transports/{transportId:guid}/cost-profile-override/{costProfileId:guid}")]
    public async Task<TransportCostProfileOverrideDto> UpdateTransportCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTransportCostProfileOverrideDto dto)
    {
        return await service.UpdateTransportCostProfileOverride(projectId, caseId, transportId, costProfileId, dto);
    }
}
