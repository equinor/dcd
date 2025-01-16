using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfileOverrides.Dtos;
using api.Features.Stea.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfileOverrides;

public class OnshorePowerSupplyCostProfileOverrideController(OnshorePowerSupplyTimeSeriesService onshorePowerSupplyTimeSeriesService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/onshore-power-supplies/{onshorePowerSupplyId:guid}/cost-profile-override")]
    public async Task<OnshorePowerSupplyCostProfileOverrideDto> CreateOnshorePowerSupplyCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid onshorePowerSupplyId,
        [FromBody] CreateOnshorePowerSupplyCostProfileOverrideDto dto)
    {
        return await onshorePowerSupplyTimeSeriesService.CreateOnshorePowerSupplyCostProfileOverride(projectId, caseId, onshorePowerSupplyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/onshore-power-supplies/{onshorePowerSupplyId:guid}/cost-profile-override/{costProfileId:guid}")]
    public async Task<OnshorePowerSupplyCostProfileOverrideDto> UpdateOnshorePowerSupplyCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid onshorePowerSupplyId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOnshorePowerSupplyCostProfileOverrideDto dto)
    {
        return await onshorePowerSupplyTimeSeriesService.UpdateOnshorePowerSupplyCostProfileOverride(projectId, caseId, onshorePowerSupplyId, costProfileId, dto);
    }
}
