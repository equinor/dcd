using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Transports.TransportCostProfileOverrides;

public class TransportCostProfileOverrideController(TransportCostProfileOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/transports/{transportId:guid}/cost-profile-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateTransportCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await service.CreateTransportCostProfileOverride(projectId, caseId, transportId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/transports/{transportId:guid}/cost-profile-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateTransportCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await service.UpdateTransportCostProfileOverride(projectId, caseId, transportId, costProfileId, dto);
    }
}
