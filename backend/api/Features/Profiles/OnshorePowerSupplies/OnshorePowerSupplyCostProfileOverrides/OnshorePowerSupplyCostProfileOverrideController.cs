using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfileOverrides;

public class OnshorePowerSupplyCostProfileOverrideController(OnshorePowerSupplyTimeSeriesService onshorePowerSupplyTimeSeriesService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/onshore-power-supplies/{onshorePowerSupplyId:guid}/cost-profile-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateOnshorePowerSupplyCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid onshorePowerSupplyId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await onshorePowerSupplyTimeSeriesService.CreateOnshorePowerSupplyCostProfileOverride(projectId, caseId, onshorePowerSupplyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/onshore-power-supplies/{onshorePowerSupplyId:guid}/cost-profile-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateOnshorePowerSupplyCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid onshorePowerSupplyId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await onshorePowerSupplyTimeSeriesService.UpdateOnshorePowerSupplyCostProfileOverride(projectId, caseId, onshorePowerSupplyId, costProfileId, dto);
    }
}
