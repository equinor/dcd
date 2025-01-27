using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Transports.TransportCostProfileOverrides;

public class TransportCostProfileOverrideController(
    CreateTimeSeriesProfileService createTimeSeriesProfileService,
    UpdateTimeSeriesProfileService updateTimeSeriesProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/transports/{transportId:guid}/cost-profile-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateTransportCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await createTimeSeriesProfileService.CreateTimeSeriesOverrideProfile(projectId, caseId, ProfileTypes.TransportCostProfileOverride, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/transports/{transportId:guid}/cost-profile-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateTransportCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await updateTimeSeriesProfileService.UpdateTimeSeriesOverrideProfile(projectId, caseId, costProfileId, ProfileTypes.TransportCostProfileOverride, dto);
    }
}
